using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using ScavHunt;
using ScavHunt.Areas.Identity;
using ScavHunt.Data;
using ScavHunt.Data.Models;
using ScavHunt.Data.Services;
using ScavHunt.Services;

var builder = WebApplication.CreateBuilder(args);

var secrets = builder.Configuration.GetSection(Secrets.SecretsName).Get<Secrets>();

// Add services to the container.
var connectionString = string.IsNullOrWhiteSpace(secrets?.DbPassword) ? builder.Configuration.GetConnectionString("DefaultConnection") :
        $"Server=db;Database=scavhunt;User=sa;Password={secrets?.DbPassword};";
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ScavhuntUser>(options => { options.SignIn.RequireConfirmedAccount = true; })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<ApplicationDbContext>(p => p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

if(builder.Configuration["Authentication:Google:ClientId"] != null) {
    builder.Services.AddAuthentication().AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });
}

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ScavhuntUser>>();

builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<PlayerAdminService>();

builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<QuestionAdminService>();

builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<LogAdminService>();

builder.Services.AddScoped<PointService>();
builder.Services.AddScoped<PointAdminService>();

builder.Services.AddScoped<LeaderboardService>();

builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<AlertAdminService>();

builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<JSInterop>();
builder.Services.AddSingleton<MarkdownService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next(context);
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var db = app.Services.CreateScope().ServiceProvider.GetService<ApplicationDbContext>();
if (db != null)
{
    db.Database.Migrate();
}

app.Run();
