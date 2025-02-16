﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Calabonga.Microservice.Module.Entities;
using Calabonga.Microservice.Module.Web.Features.Logs;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Factories;
using Microsoft.Extensions.Logging;

namespace Calabonga.Microservice.Module.Web.Infrastructure.Engine.ViewModelFactories
{
    /// <summary>
    /// ViewModel factory for <see cref="Log"/>
    /// </summary>
    public class LogViewModelFactory : ViewModelFactory<LogCreateViewModel, LogUpdateViewModel>
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public LogViewModelFactory(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override Task<OperationResult<LogCreateViewModel>> GenerateForCreateAsync()
        {
            var operation = OperationResult.CreateResult<LogCreateViewModel>();
            operation.Result = new LogCreateViewModel()
            {
                CreatedAt = DateTime.Now,
                Level = LogLevel.Information.ToString(),
                Logger = "Demo purposes only",
                Message = $"This is demo message {DateTime.Now}"
            };
            return Task.FromResult(operation);
        }

        public override async Task<OperationResult<LogUpdateViewModel>> GenerateForUpdateAsync(Guid id)
        {
            var operation = OperationResult.CreateResult<LogUpdateViewModel>();
            var entity = await _context.GetRepository<Log>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            var mapped = _mapper.Map<LogUpdateViewModel>(entity);
            operation.Result = mapped;
            operation.AddSuccess("ViewModel generated for Log entity. Please see Additional information in DataObject").AddData(new { Identifier = id });

            return operation;
        }
    }
}
