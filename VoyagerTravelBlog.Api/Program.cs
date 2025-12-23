using Azure.Identity;
using VoyagerTravelBlog.Application;
using VoyagerTravelBlog.Application.Options;
using VoyagerTravelBlog.Infrastructure;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    var keyVaultConnectionString = builder.Configuration["AzureKeyVault:ConnectionString"];
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultConnectionString),
        new DefaultAzureCredential());
}

// Add services to the container.
builder.Services.AddPersistence(builder.Configuration.GetConnectionString("BlogConnectionString"));
builder.Services.AddApplication(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddControllers(options =>
{
    if (!builder.Environment.IsDevelopment())
    {
        options.Filters.Add<GlobalAntiforgeryFilter>();
    }
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Add AddAntiforgery
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddAntiforgery(options =>
    {
        options.HeaderName = "X-XSRF-TOKEN";
    });
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddApplicationInsights(
        configureTelemetryConfiguration: (config) =>
            config.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"],
            configureApplicationInsightsLoggerOptions: (options) => { }
    );

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                          .WithOrigins(["http://localhost:4200", "https://wonderful-island-0565ee10f.4.azurestaticapps.net"])
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.LoginPath = "/api/users/login";
        options.LogoutPath = "/api/users/logout";
    })
        .AddJwtBearer(options =>
        {
            var jwtSettings = builder.Configuration.GetSection(JWTSettingsOptions.JwtSettings).Get<JWTSettingsOptions>();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["jwt"];
                    return Task.CompletedTask;
                }
            };
        });

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Voyager Blog API v1");
        options.ConfigObject.AdditionalItems["withCredentials"] = true;
    });
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
    var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
    app.Use((context, next) =>
    {
        var tokens = antiforgery.GetAndStoreTokens(context);

        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions
        {
            HttpOnly = false,
            Secure = false,
            SameSite = SameSiteMode.None
        });

        return next(context);
    });
}

app.MapControllers();

app.Run();
