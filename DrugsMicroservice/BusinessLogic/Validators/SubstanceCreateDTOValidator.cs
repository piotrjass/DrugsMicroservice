using DrugsMicroservice.Application.DTOs.Substances;
using FluentValidation;
namespace DrugsMicroservice.BusinessLogic.Validators;


public class SubstanceCreateDTOValidator : AbstractValidator<SubstanceCreateDTO>
{
    public SubstanceCreateDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters.");

        RuleFor(x => x.Dosage)
            .NotEmpty().WithMessage("Dosage is required.")
            .MaximumLength(50).WithMessage("Dosage cannot be longer than 50 characters.");
        
    }
}
