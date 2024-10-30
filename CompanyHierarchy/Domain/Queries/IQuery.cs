using CompanyHierarchy.Domain.Common;
using MediatR;

namespace CompanyHierarchy.Domain.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}