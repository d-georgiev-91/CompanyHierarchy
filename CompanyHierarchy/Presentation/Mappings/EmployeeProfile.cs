using AutoMapper;
using CompanyHierarchy.Domain.Commands.Employee;

namespace CompanyHierarchy.Presentation.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Models.Employee, UpsertEmployeeCommand>();
        CreateMap<Models.EmployeeWithManagedEmployees, UpsertEmployeeCommand>();
        CreateMap<UpsertEmployeeCommand, Domain.Entities.Employee>();
        CreateMap<Domain.Entities.Employee, Infrastructure.Handlers.QueryHandlers.Employee.EmployeeResponse>();
        CreateMap<Infrastructure.Handlers.QueryHandlers.Employee.EmployeeResponse, Models.EmployeeWithManagedEmployees>();
    }
}
