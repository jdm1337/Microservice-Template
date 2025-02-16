﻿using AutoMapper;
using $ext_projectname$.Infrastructure;
using $safeprojectname$.Endpoints.ProfileEndpoints.ViewModels;
using Calabonga.Microservices.Core;
using IdentityModel;
using System.Security.Claims;

namespace $safeprojectname$.Endpoints.ProfileEndpoints
{
    /// <summary>
    /// Mapper Configuration for entity ApplicationUser
    /// </summary>
    public class ProfilesMapperConfiguration : Profile
    {
        /// <inheritdoc />
        public ProfilesMapperConfiguration()
        {
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(x => x.UserName, o => o.MapFrom(p => p.Email))
                .ForMember(x => x.Email, o => o.MapFrom(p => p.Email))
                .ForMember(x => x.EmailConfirmed, o => o.MapFrom(src => true))
                .ForMember(x => x.FirstName, o => o.MapFrom(p => p.FirstName))
                .ForMember(x => x.LastName, o => o.MapFrom(p => p.LastName))
                .ForMember(x => x.PhoneNumberConfirmed, o => o.MapFrom(src => true))
                .ForMember(x => x.ApplicationUserProfileId, o => o.Ignore())
                .ForMember(x => x.ApplicationUserProfile, o => o.Ignore())
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.NormalizedUserName, o => o.Ignore())
                .ForMember(x => x.NormalizedEmail, o => o.Ignore())
                .ForMember(x => x.PasswordHash, o => o.Ignore())
                .ForMember(x => x.SecurityStamp, o => o.Ignore())
                .ForMember(x => x.ConcurrencyStamp, o => o.Ignore())
                .ForMember(x => x.PhoneNumber, o => o.Ignore())
                .ForMember(x => x.TwoFactorEnabled, o => o.Ignore())
                .ForMember(x => x.LockoutEnd, o => o.Ignore())
                .ForMember(x => x.LockoutEnabled, o => o.Ignore())
                .ForMember(x => x.AccessFailedCount, o => o.Ignore());

            CreateMap<ClaimsIdentity, UserProfileViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(claims => ClaimsHelper.GetValue<Guid>(claims, JwtClaimTypes.Subject)))
                .ForMember(x => x.PositionName, o => o.MapFrom(claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.Actor)))
                .ForMember(x => x.FirstName, o => o.MapFrom(claims => ClaimsHelper.GetValue<string>(claims, JwtClaimTypes.GivenName)))
                .ForMember(x => x.LastName, o => o.MapFrom(claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.Surname)))
                .ForMember(x => x.Roles, o => o.MapFrom(claims => ClaimsHelper.GetValues<string>(claims, JwtClaimTypes.Role)))
                .ForMember(x => x.Email, o => o.MapFrom(claims => ClaimsHelper.GetValue<string>(claims, JwtClaimTypes.Name)))
                .ForMember(x => x.PhoneNumber, o => o.MapFrom(claims => ClaimsHelper.GetValue<string>(claims, JwtClaimTypes.PhoneNumber)));
        }
    }

    /// <summary>
    /// Mapper Configuration for entity Person
    /// </summary>
    public class ApplicationUserProfileMapperConfiguration : Profile
    {
        /// <inheritdoc />
        public ApplicationUserProfileMapperConfiguration() =>
            CreateMap<RegisterViewModel, ApplicationUserProfile>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.Permissions, o => o.Ignore())
                .ForMember(x => x.ApplicationUser, o => o.Ignore())
                .ForMember(x => x.CreatedAt, o => o.Ignore())
                .ForMember(x => x.CreatedBy, o => o.Ignore())
                .ForMember(x => x.UpdatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedBy, o => o.Ignore());
    }
}