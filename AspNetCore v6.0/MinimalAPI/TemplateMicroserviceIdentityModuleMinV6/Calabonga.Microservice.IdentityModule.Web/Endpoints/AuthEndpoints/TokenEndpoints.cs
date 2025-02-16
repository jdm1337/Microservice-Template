﻿using $ext_projectname$.Infrastructure;
using $safeprojectname$.Application.Services;
using $safeprojectname$.Definitions.Base;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace $safeprojectname$.Endpoints.AuthEndpoints;

/// <summary>
/// Token Endpoint for OpenIddict
/// </summary>
public class TokenEndpoints : AppDefinition
{
    public override void ConfigureApplication(WebApplication app, IWebHostEnvironment environment) =>
        app.MapPost("~/connect/token", TokenAsync).ExcludeFromDescription();

    private async Task<IResult> TokenAsync(
        HttpContext httpContext,
        IOpenIddictScopeManager manager,
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        IAccountService accountService)
    {
        var request = httpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
        
        if (request.IsClientCredentialsGrantType())
        {
            var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // Subject or sub is a required field, we use the client id as the subject identifier here.
            identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId!);
            identity.AddClaim(OpenIddictConstants.Claims.ClientId, request.ClientId!);
        

            // Don't forget to add destination otherwise it won't be added to the access token.
            identity.AddClaim(OpenIddictConstants.Claims.Scope, request.Scope!, OpenIddictConstants.Destinations.AccessToken);
            identity.AddClaim("nimble", "framework", OpenIddictConstants.Destinations.AccessToken);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            claimsPrincipal.SetScopes(request.GetScopes());
            return Results.SignIn(claimsPrincipal, new AuthenticationProperties(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (request.IsPasswordGrantType())
        {
            var user = await userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return Results.Problem("Invalid operation");
            }

            // Ensure the user is allowed to sign in
            if (!await signInManager.CanSignInAsync(user))
            {
                return Results.Problem("Invalid operation");
            }


            // Ensure the user is not already locked out
            if (userManager.SupportsUserLockout && await userManager.IsLockedOutAsync(user))
            {
                return Results.Problem("Invalid operation");
            }

            // Ensure the password is valid
            if (!await userManager.CheckPasswordAsync(user, request.Password))
            {
                if (userManager.SupportsUserLockout)
                {
                    await userManager.AccessFailedAsync(user);
                }

                return Results.Problem("Invalid operation");
            }

            // Reset the lockout count
            if (userManager.SupportsUserLockout)
            {
                await userManager.ResetAccessFailedCountAsync(user);
            }

            var principal = await accountService.GetPrincipalByEmailAsync(user.Email);
            return Results.SignIn(principal, new AuthenticationProperties(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (request.IsAuthorizationCodeGrantType())
        {
            var authenticateResult = await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            var properties = authenticateResult.Properties;
            var claimsPrincipal = authenticateResult.Principal;
            return Results.SignIn(claimsPrincipal!, properties ?? new AuthenticationProperties(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return Results.Problem("The specified grant type is not supported.");
    }
}