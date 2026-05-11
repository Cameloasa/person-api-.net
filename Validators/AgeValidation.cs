
using System.ComponentModel.DataAnnotations;
namespace PersonApi.Validators;

public class AgeValidation: ValidationAttribute
{
    private readonly int _age;
    public AgeValidation(int age)
    {
        _age = age;
        ErrorMessage = $"Age must be at least {_age} years.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is int age && age >= _age)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}