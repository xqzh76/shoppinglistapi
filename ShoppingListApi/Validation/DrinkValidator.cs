namespace ShoppingListApi.Validation
{
    using FluentValidation;
    using ShoppingListApi.Model;

    public sealed class DrinkValidator : AbstractValidator<Drink>
    {
        public DrinkValidator()
        {
            this.RuleFor(drink => drink.Name)
                .NotEmpty()
                .WithMessage("The name of the drink must be provided")
                .MaximumLength(200) // 200 is an random number for illustration purpose 
                .WithMessage("The name of drink can't be more than 200 characters");

            this.RuleFor(drink => drink.Quantity)
                .NotNull()
                .WithMessage("The quantity of the drink must be provided")
                .GreaterThan(0)
                .WithMessage("The quantity of the drink must be more than 0");
        }
    }
}