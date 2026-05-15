using FluentValidation;
using PersonApi.Models.DTOs;

namespace PersonApi.Validators;
public class AdressDTOValidator : AbstractValidator<AdressDTO>
{
    public AdressDTOValidator()
    {
        RuleFor(a => a.Street)
            .NotEmpty().WithMessage("Street is required.");

        RuleFor(a => a.City)
            .NotEmpty().WithMessage("City is required.");

        RuleFor(a => a.Zip)
            .NotEmpty().WithMessage("Zip is required.");
    }
}
