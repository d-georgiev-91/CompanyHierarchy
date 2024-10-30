using CompanyHierarchy.Domain.Commands;
using CompanyHierarchy.Domain.Common;
using MediatR;

namespace CompanyHierarchy.Infrastructure.Handlers.CommandHandlers;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}