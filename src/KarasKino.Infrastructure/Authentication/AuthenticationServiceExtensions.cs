using System.Text;
using KarasKino.Core.Authentication;
using KarasKino.Core.Interfaces;
using KarasKino.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace KarasKino.Infrastructure;

public static class AuthenticationServiceExtensions
{
  public static IServiceCollection AddAuthenticationServices(
    this IServiceCollection services,
    ConfigurationManager config)
  {
    services.AddOptions<JwtConfiguration>()
            .BindConfiguration(JwtConfiguration.SectionName)
            .ValidateDataAnnotations();

    services.AddOptions<GoogleConfiguration>()
            .BindConfiguration(GoogleConfiguration.SectionName)
            .ValidateDataAnnotations();

    services.AddScoped<IJwtService, JwtService>();
    services.AddHttpContextAccessor();

    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddCookie("External", options =>
    {
      options.Cookie.SameSite = SameSiteMode.None;
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
      options.Cookie.HttpOnly = true;
      options.Cookie.Path = "/";
      options.Cookie.Domain = config["Auth:CookieDomain"];
    })
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(config["Jwt:Secret"]!))
      };
      options.Events = new JwtBearerEvents
      {
        OnMessageReceived = ctx =>
        {
          ctx.Token = ctx.Request.Cookies["auth"];
          return Task.CompletedTask;
        }
      };
    })
    .AddGoogle(options =>
    {
      options.ClientId = config["Google:ClientId"]!;
      options.ClientSecret = config["Google:ClientSecret"]!;
      options.CallbackPath = "/authentication/google/redirect";
      options.SignInScheme = "External";
      
      options.CorrelationCookie.SameSite = SameSiteMode.None;
      options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
      options.CorrelationCookie.HttpOnly = true;
    });

    services.AddAuthorization();

    return services;
  }
}
