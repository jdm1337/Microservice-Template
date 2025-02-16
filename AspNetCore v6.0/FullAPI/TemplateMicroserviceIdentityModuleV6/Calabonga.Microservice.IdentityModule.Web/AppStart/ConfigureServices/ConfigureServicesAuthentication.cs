﻿using $ext_projectname$.Data;
using $safeprojectname$.Infrastructure.Services;

using IdentityServer4.AccessTokenValidation;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace $safeprojectname$.AppStart.ConfigureServices;

/// <summary>
/// ASP.NET Core services registration and configurations
/// Authentication path
/// </summary>
public static class ConfigureServicesAuthentication
{
    /// <summary>
    /// Configure Authentication & Authorization
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration.GetSection("IdentityServer").GetValue<string>("Url");
        services.AddAuthentication()
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(
                options =>
                {
                    options.SupportedTokens = SupportedTokens.Jwt;
                    options.Authority = url;
                    options.EnableCaching = true;
                    options.RequireHttpsMetadata = false;
                });

        services.AddIdentityServer(options =>
            {
                options.Authentication.CookieSlidingExpiration = true;
                options.IssuerUri = url;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.UserInteraction.LoginUrl = "/Authentication/Login";
                options.UserInteraction.LogoutUrl = "/Authentication/Logout";
            })
            .AddInMemoryPersistedGrants()
            .AddDeveloperSigningCredential()
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
            .AddInMemoryClients(IdentityServerConfig.GetClients())
            .AddInMemoryApiScopes(IdentityServerConfig.GetAPiScopes())
            .AddAspNetIdentity<ApplicationUser>()
            .AddJwtBearerClientAuthentication()
            .AddProfileService<IdentityProfileService>();
    }
}