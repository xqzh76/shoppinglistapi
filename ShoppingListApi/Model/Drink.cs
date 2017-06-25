namespace ShoppingListApi.Model
{
    using FluentValidation.Attributes;
    using ShoppingListApi.Validation;

    [Validator(typeof(DrinkValidator))]
    public sealed class Drink
    {
        public string Name { get; set; }

        public int? Quantity { get; set; }
    }
}