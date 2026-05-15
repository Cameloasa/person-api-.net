using FluentValidation;
using PersonApi.Models.Requests;

namespace PersonApi.Validators;
public class CreatePersonRequestValidator : AbstractValidator<CreatePersonRequest>
{
    public CreatePersonRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(p => p.Age)
            .GreaterThan(0).WithMessage("Age must be greater than 0.");

        RuleFor(p => p.Adresses)
            .NotEmpty().WithMessage("At least one address is required.");

        RuleForEach(p => p.Adresses)
            .SetValidator(new AdressDTOValidator());
    }
}
