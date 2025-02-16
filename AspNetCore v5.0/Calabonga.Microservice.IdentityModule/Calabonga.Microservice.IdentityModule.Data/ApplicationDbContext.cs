﻿using Calabonga.Microservice.IdentityModule.Data.Base;
using Calabonga.Microservice.IdentityModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace Calabonga.Microservice.IdentityModule.Data
{
    /// <summary>
    /// Database context for current application
    /// </summary>
    public class ApplicationDbContext : DbContextBase, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        #region System

        public DbSet<Log> Logs { get; set; } = null!;

        public DbSet<ApplicationUserProfile> Profiles { get; set; } = null!;

        /// <inheritdoc />
        public DbSet<MicroservicePermission> Permissions { get; set; } = null!;

        #endregion
    }
}