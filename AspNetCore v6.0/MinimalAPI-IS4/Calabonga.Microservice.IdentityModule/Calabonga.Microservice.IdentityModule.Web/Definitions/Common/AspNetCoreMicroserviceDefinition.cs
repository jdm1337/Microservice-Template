﻿using Calabonga.Microservice.IdentityModule.Web.Definitions.Base;

namespace Calabonga.Microservice.IdentityModule.Web.Definitions.Common
{
    /// <summary>
    /// AspNetCore common configuration
    /// </summary>
    public class CommonMicroserviceDefinition : MicroserviceDefinition
    {
        /// <summary>
        /// Configure application for current microservice
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public override void ConfigureApplication(IApplicationBuilder app, IWebHostEnvironment env)
            => app.UseHttpsRedirection();

        /// <summary>
        /// Configure services for current microservice
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization();
            services.AddHttpContextAccessor();
            services.AddResponseCaching();
            services.AddMemoryCache();
        }
    }
}