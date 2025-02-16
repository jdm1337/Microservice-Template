﻿using AutoMapper;
using Calabonga.Microservice.Module.Web.Endpoints.ProfileEndpoints.ViewModels;
using Calabonga.Microservices.Core;
using IdentityModel;
using System.Security.Claims;

namespace Calabonga.Microservice.Module.Web.Endpoints.ProfileEndpoints
{
    /// <summary>
    /// Mapper Configuration for entity ApplicationUser
    /// </summary>
    public class ProfilesMapperConfiguration : Profile
    {
        /// <inheritdoc />
        public ProfilesMapperConfiguration()
            => CreateMap<ClaimsIdentity, UserProfileViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(claims => ClaimsHelper.GetValue<Guid>(claims, JwtClaimTypes.Subject)))
                .ForMember(x => x.PositionName, o => o.MapFrom(claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.Actor)))
                .ForMember(x => x.FirstName, o => o.MapFrom(claims => ClaimsHelper.GetValue<string>(claims, JwtClaimTypes.GivenName)))
                .ForMember(x => x.LastName, o => o.MapFrom(claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.Surname)))
                .ForMember(x => x.Roles, o => o.MapFrom(claims => ClaimsHelper.GetValues<string>(claims, JwtClaimTypes.Role)))
                .ForMember(x => x.Email, o => o.MapFrom(claims => ClaimsHelper.GetValue<string>(claims, JwtClaimTypes.Name)))
                .ForMember(x => x.PhoneNumber, o => o.MapFrom(claims => ClaimsHelper.GetValue<string>(claims, JwtClaimTypes.PhoneNumber)));
    }
}