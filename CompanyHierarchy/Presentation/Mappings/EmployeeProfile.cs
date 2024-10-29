using AutoMapper;

namespace CompanyHierarchy.Presentation.Mappings;

public class EmployeeProfile: Profile
{
    public EmployeeProfile()
    {
        CreateMap<Models.Employee, Domain.Commands.UpsertEmployeeCommand>();
        CreateMap<Models.EmployeeWithManagedEmployees, Domain.Commands.UpsertEmployeeCommand>();
        CreateMap<Domain.Commands.UpsertEmployeeCommand, Domain.Entities.Employee>();
        CreateMap<Domain.Entities.Employee, Models.EmployeeWithManagedEmployees>();
    }
}
