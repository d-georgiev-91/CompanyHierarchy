using CompanyHierarchy.Domain.Common;
using MediatR;

namespace CompanyHierarchy.Domain.Commands;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
