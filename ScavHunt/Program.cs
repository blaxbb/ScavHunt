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

    authBuilder.AddOpenIdConnect("apple", "Apple", options =>
    {
        options.Authority = "https://appleid.apple.com"; // disco doc: https://appleid.apple.com/.well-known/openid-configuration

        options.ClientId = builder.Configuration["Authentication:Apple:ClientId"];
        options.CallbackPath = "/signin-apple"; // corresponding to your redirect URI

        options.ResponseType = "code id_token"; // hybrid flow due to lack of PKCE support
        options.ResponseMode = "form_post"; // form post due to prevent PII in the URL
        options.DisableTelemetry = true;

        options.Scope.Clear(); // apple does not support the profile scope
        options.Scope.Add("openid");
        options.Scope.Add("email");
        options.Scope.Add("name");

        // custom client secret generation - secret can be re-used for up to 6 months
        options.Events.OnAuthorizationCodeReceived = context =>
        {
            context.TokenEndpointRequest.ClientSecret = TokenGenerator.CreateNewToken(builder.Configuration);
            return Task.CompletedTask;
        };

        options.UsePkce = false; // apple does not currently support PKCE (April 2021)
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
