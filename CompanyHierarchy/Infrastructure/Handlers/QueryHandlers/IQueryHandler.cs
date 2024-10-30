using CompanyHierarchy.Domain.Common;
using CompanyHierarchy.Domain.Queries;
using MediatR;

namespace CompanyHierarchy.Infrastructure.Handlers.QueryHandlers;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>?
{
}