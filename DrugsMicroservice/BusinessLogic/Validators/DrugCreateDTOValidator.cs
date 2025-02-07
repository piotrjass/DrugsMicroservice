using DrugsMicroservice.Application.DTOs;
using FluentValidation;
namespace DrugsMicroservice.BusinessLogic.Validators;

public class DrugCreateDTOValidator : AbstractValidator<DrugCreateDTO>
{
    public DrugCreateDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot be longer than 200 characters.");

        RuleFor(x => x.Manufacturer)
            .NotEmpty().WithMessage("Manufacturer is required.")
            .MaximumLength(150).WithMessage("Manufacturer cannot be longer than 150 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

    }
}