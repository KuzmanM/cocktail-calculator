namespace CocktailCalculator
{
    public interface IIngredient
    {
        bool IsQuantityUnknown { get; }

        double Quantity { get; set; }

        bool IsConcentrationUnknown { get; }

        double Concentration { get; set; }
    }
}
