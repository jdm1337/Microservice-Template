﻿using $ext_projectname$.Entities;
using $safeprojectname$.Features.Logs;
using $safeprojectname$.Infrastructure.Mappers.Base;
using Calabonga.UnitOfWork;

namespace $safeprojectname$.Infrastructure.Mappers
{
    /// <summary>
    /// Mapper Configuration for entity Log
    /// </summary>
    public class LogMapperConfiguration: MapperConfigurationBase
    {
        /// <inheritdoc />
        public LogMapperConfiguration()
        {
            CreateMap<LogCreateViewModel, Log>()
                .ForMember(x=>x.Id, o=>o.Ignore());

            CreateMap<Log, LogViewModel>();

            CreateMap<IPagedList<Log>, IPagedList<LogViewModel>>()
                .ConvertUsing<PagedListConverter<Log, LogViewModel>>();
        }
    }
}
