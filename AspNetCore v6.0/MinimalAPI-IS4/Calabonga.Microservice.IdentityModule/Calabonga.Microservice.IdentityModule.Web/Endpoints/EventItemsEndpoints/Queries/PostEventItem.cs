﻿using AutoMapper;
using Calabonga.Microservice.IdentityModule.Domain;
using Calabonga.Microservice.IdentityModule.Web.Endpoints.EventItemsEndpoints.ViewModels;
using Calabonga.Microservices.Core;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;

namespace Calabonga.Microservice.IdentityModule.Web.Endpoints.EventItemsEndpoints.Queries
{
    /// <summary>
    /// Request: EventItem creation
    /// </summary>
    public record PostEventItemRequest(EventItemCreateViewModel Model) : IRequest<OperationResult<EventItemViewModel>>;

    /// <summary>
    /// Request: EventItem creation
    /// </summary>
    public class PostEventItemRequestHandler : IRequestHandler<PostEventItemRequest, OperationResult<EventItemViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PostEventItemRequestHandler> _logger;


        public PostEventItemRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostEventItemRequestHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<EventItemViewModel>> Handle(PostEventItemRequest eventItemRequest, CancellationToken cancellationToken)
        {
            var operation = OperationResult.CreateResult<EventItemViewModel>();

            var entity = _mapper.Map<EventItemCreateViewModel, EventItem>(eventItemRequest.Model);
            if (entity == null)
            {
                operation.AddError(new MicroserviceUnauthorizedException(AppContracts.Exceptions.MappingException));
                return operation;
            }

            await _unitOfWork.GetRepository<EventItem>().InsertAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            var lastResult = _unitOfWork.LastSaveChangesResult;
            if (lastResult.IsOk)
            {

                var mapped = _mapper.Map<EventItem, EventItemViewModel>(entity);

                operation.Result = mapped;
                operation.AddSuccess("Successfully created");
                return operation;
            }

            operation.AddError(lastResult.Exception);
            operation.AppendLog("Something went wrong");

            return operation;
        }
    }
}