using DrugsMicroservice.Application.DTOs.Diseases;
using FluentValidation;

namespace DrugsMicroservice.BusinessLogic.Validators;

public class DiseaseCreateDTOValidator : AbstractValidator<DiseaseCreateDTO>
{
    public DiseaseCreateDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters.");
    }
}