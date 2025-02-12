using DrugsMicroservice.Application.DTOs.Diseases;
using FluentValidation;

namespace DrugsMicroservice.BusinessLogic.Validators;

public class DiseaseUpdateDTOValidator : AbstractValidator<DiseaseUpdateDTO>
{
    public DiseaseUpdateDTOValidator()
    {
        RuleFor(disease => disease.Name)
            .NotEmpty().WithMessage("Name is required.") // Walidacja, że nazwa nie jest pusta
            .Length(3, 100).WithMessage("Name must be between 3 and 100 characters."); // Walidacja długości
    }
}