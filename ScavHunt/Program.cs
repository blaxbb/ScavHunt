using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
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

builder.Services.AddDefaultIdentity<ScavhuntUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<ApplicationDbContext>(p => p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
var authBuilder = builder.Services.AddAuthentication();
if (builder.Configuration["Authentication:Google:ClientId"] != null)
{
    authBuilder.AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });
}

if (builder.Configuration["Authentication:Microsoft:ClientId"] != null)
{
    authBuilder.AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
        microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
    });
}

if (builder.Configuration["Authentication:Twitter:ConsumerKey"] != null)
{
    authBuilder.AddTwitter(twitterOptions =>
    {
        twitterOptions.ConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerKey"];
        twitterOptions.ConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"];
    });
}

if (builder.Configuration["Authentication:Apple:ClientId"] != null)
{
    authBuilder
        .AddApple(options =>
        {
            options.ClientId = builder.Configuration["Authentication:Apple:ClientId"];
            options.KeyId = builder.Configuration["Authentication:Apple:KeyId"];
            options.TeamId = builder.Configuration["Authentication:Apple:TeamId"];
            options.GenerateClientSecret = true;
            options.PrivateKey = (keyId, _) =>
            {
                var appleKeySecret = builder.Configuration[$"Authentication:Apple:PrivateKey"];
                var withPem = string.Format("-----BEGIN PRIVATE KEY-----\n{0}\n-----END PRIVATE KEY-----", appleKeySecret);
                return Task.FromResult(withPem.AsMemory());
            };
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

builder.Services.AddScoped<PrizeAdminService>();

builder.Services.AddScoped<JSInterop>();
builder.Services.AddSingleton<MarkdownService>();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<EmailSender>();
builder.Services.Configure<SendGridOptions>(builder.Configuration.GetSection("SendGrid"));
builder.Services.AddTransient<StatsAdminService>();

builder.Services.AddOptions<BadgeOptions>();
builder.Services.Configure<BadgeOptions>(builder.Configuration.GetSection(BadgeOptions.SectionName));

if (builder.Configuration.GetSection("Azure").GetSection("SignalR").GetValue<string>("ConnectionString") != null)
{
    builder.Services.AddSignalR().AddAzureSignalR();
}

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/admin/players/{username}", "/_Host");
    endpoints.MapFallbackToPage("/_Host");

});


app.UseHttpLogging();

if (app.Environment.IsDevelopment())
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
