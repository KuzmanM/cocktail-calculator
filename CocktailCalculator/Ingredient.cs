namespace CocktailCalculator
{
    public class Ingredient : IIngredient
    {
        public string Description { get; set; }

        public bool IsQuantityUnknown { get; set; }

        public double Quantity { get; set; }

        public bool IsConcentrationUnknown { get; set; }

        public double Concentration { get; set; }

        public Ingredient()
        { }

        public Ingredient(string description, double quantity, double concentration, bool isQuantityUnknown, bool isConcentrationUnknown)
        {
            Description = description;
            Quantity = quantity;
            Concentration = concentration;

            IsQuantityUnknown = isQuantityUnknown;
            IsConcentrationUnknown = isConcentrationUnknown;
        }

        public Ingredient(double quantity, double concentration, bool isQuantityUnknown = false, bool isConcentrationUnknown = false)
            : this(string.Empty, quantity, concentration, isQuantityUnknown, isConcentrationUnknown)
        { }

        public override string ToString()
        {
            string qu = IsQuantityUnknown ? "*" : string.Empty;
            string cu = IsConcentrationUnknown ? "*" : string.Empty;
            return $"{Description} -> Quantity{qu}:{Quantity}   Concentration{cu}:{Concentration}";
        }
    }
}
