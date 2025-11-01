using FluentValidation;
using MinimalAPI.Models.Requests;
using System.Text.RegularExpressions;

namespace MinimalAPI.Validators;

public abstract class ProductRequestValidator<T> : AbstractValidator<T> where T : ProductRequestBase
{
    protected ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters")
            .Must(BeValidProductName).WithMessage("Product name contains invalid characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .LessThan(1000000).WithMessage("Price must be less than 1,000,000")
            .PrecisionScale(18, 2, true).WithMessage("Price must have at most 2 decimal places");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");
    }

    protected bool BeValidProductName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // Allow alphanumeric, spaces, hyphens, and common punctuation
        return Regex.IsMatch(name, @"^[a-zA-Z0-9\s\-\.,&']+$");
    }
}
