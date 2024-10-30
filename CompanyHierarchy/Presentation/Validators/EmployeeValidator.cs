using CompanyHierarchy.Presentation.Models;
using FluentValidation;

namespace CompanyHierarchy.Presentation.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(employee => employee.FullName).NotEmpty().MaximumLength(100);
        RuleFor(employee => employee.Title).NotEmpty().MaximumLength(100);
        RuleFor(employee => employee.ManagerEmployeeId).NotEqual(employee => employee.EmployeeId).WithMessage("An employee cannot be their own manager.");
    }
}
