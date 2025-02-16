﻿using $ext_projectname$.Data;
using $safeprojectname$.Features.Account;
using $safeprojectname$.Infrastructure.Auth;
using $safeprojectname$.Infrastructure.Services;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;

namespace $safeprojectname$.Infrastructure.DependencyInjection;

/// <summary>
/// Registrations for both points: API and Scheduler
/// </summary>
public static class DependencyContainer
{
    /// <summary>
    /// Register 
    /// </summary>
    /// <param name="services"></param>
    public static void Common(IServiceCollection services)
    {
        services.AddTransient<ApplicationUserStore>();
        services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<ApplicationClaimsPrincipalFactory>();

        // services
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IProfileService, IdentityProfileService>();
        services.AddTransient<ICacheService, CacheService>();
        services.AddTransient<ICorsPolicyService, IdentityServerCorsPolicy>();
    }
}