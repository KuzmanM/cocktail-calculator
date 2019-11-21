using System;
using System.Collections.Generic;
using System.Linq;

namespace CocktailCalculator
{
    public static class Calculator
    {
        #region --- System of Equations ---

        // The sum of all ingredients quantities is eaqual to the result sum
        // Q1 + Q2 + ... Qn = Qt
        //
        // The sum of product (quantity * concentration) of all ingredients is equal to the result product (quantity * concentration)
        // Q1*C1 + Q2*C2 + ... Qn*Cn = Qt*Ct

        #endregion

        #region Validation

        /// <summary>
        /// Validate calculation data
        /// </summary>
        /// <param name="ingredients">Ingredients</param>
        /// <param name="total">Total</param>
        /// <returns>Error message or null</returns>
        public static string Validate(IEnumerable<IIngredient> ingredients, IIngredient total)
        {
            // Selected unknown values for calculation
            UVCountValidationResult countValidationResult = GetUnknownValuesCountState(ingredients, total);
            switch (countValidationResult)
            {
                case UVCountValidationResult.ExcessiveSelections:
                    return "Too many unknown values selected. Calculation is indeterminate and impossible. Expected selections are two!";
                
                case UVCountValidationResult.InsufficientSelections:
                    return "Only one unknown value selected. Calculation is over-determinate and incorrect. Expected selections are two!";
                
                case UVCountValidationResult.ExcessiveConcentrationSelections:
                    return "Too many unknown concentrations selected. Calculation is indeterminate and impossible. Only one concentration could be unknown!";
            }

            // Ingredients minimum count
            if (ingredients.Count() < 2)
                return "Minimum two ingredients are expected for meaningful calculation!";

            // Ingredients quantity
            bool invalidQuanity = ingredients.Any(i => !IsQuantityValid(i));
            if (invalidQuanity)
                return "Invalid ingredient quantity found!";

            // Ingredients concentration
            bool invalidConcentration = ingredients.Any(i => !IsConcentrationValid(i));
            if (invalidConcentration)
                return "Invalid ingredient concentration found!";

            // Total quantity
            invalidQuanity = !IsQuantityValid(total);
            if (invalidQuanity)
                return "Invalid total quantity!";

            // Total concentration
            invalidConcentration = !IsConcentrationValid(total);
            if (invalidConcentration)
                return "Invalid total concentration!";

            return null;// validation is OK
        }

        /// <summary>
        /// Validates count of unknown values selection
        /// </summary>
        /// <param name="ingredients">Ingredients</param>
        /// <param name="total">Total</param>
        /// <returns>Validation result</returns>
        public static UVCountValidationResult GetUnknownValuesCountState(IEnumerable<IIngredient> ingredients, IIngredient total)
        {
            int selectedQuantities = ingredients.Where(i => i.IsQuantityUnknown).Count();
            selectedQuantities += total.IsQuantityUnknown ? 1 : 0;

            int selectedConcentration = ingredients.Where(i => i.IsConcentrationUnknown).Count();
            selectedConcentration += total.IsConcentrationUnknown ? 1 : 0;

            int totalSelections = selectedQuantities + selectedConcentration;

            if (totalSelections > 2)
                return UVCountValidationResult.ExcessiveSelections;

            else if (totalSelections < 2)
                return UVCountValidationResult.InsufficientSelections;

            else if (selectedConcentration > 1)
                return UVCountValidationResult.ExcessiveConcentrationSelections;

            else
                return UVCountValidationResult.OK;
        }

        /// <summary>
        /// Validate ingredient quantity value
        /// </summary>
        /// <param name="ingredient">Ingredient item</param>
        /// <param name="validateUnknownValue">Set true to force validation of unknown values</param>
        /// <returns>true if ingredient quantity value is valid</returns>
        public static bool IsQuantityValid(IIngredient ingredient, bool validateUnknownValue = false)
        {
            bool isValid = (!validateUnknownValue && ingredient.IsQuantityUnknown) || ingredient.Quantity > 0;
            return isValid;
        }

        /// <summary>
        /// Validate ingredient concentration value
        /// </summary>
        /// <param name="ingredient">Ingredient item</param>
        /// <param name="validateUnknownValue">Set true to force validation of unknown values</param>
        /// <returns>true if ingredient concentration value is valid</returns>
        public static bool IsConcentrationValid(IIngredient ingredient, bool validateUnknownValue = false)
        {
            bool isValid = (!validateUnknownValue && ingredient.IsConcentrationUnknown) || (ingredient.Concentration >= 0 && ingredient.Concentration <= 100);
            return isValid;
        }

        /// <summary>
        /// Validate calculation. The validation covers the mathematic side of the calculation.
        /// So negative values will be accepted although they is not physically meaningful
        /// </summary>
        /// <param name="ingredients">Ingredients</param>
        /// <param name="total">Total</param>
        /// <param name="acceptNaNAsValid">Incorrect input data can cause division by zero and NaN result. Set true to accept these results as valid.</param>
        /// <param name="acceptAutOfRangeAsValid">Incorrect input data can cause mathematically correct but physically incorrect result (negative values for example). Set true to accept these results as valid.</param>
        /// <param name="precision">Floating point calculation precision</param>
        /// <returns>true if math validation passes</returns>
        public static bool ValidateCalculationResult(IEnumerable<IIngredient> ingredients, IIngredient total, bool acceptNaNAsValid = false, bool acceptAutOfRangeAsValid = false, double precision = 0)
        {
            // Validate result for NaN values (devision by zero)
            if (!acceptNaNAsValid)
            {
                if (double.IsNaN(total.Quantity) || double.IsNaN(total.Concentration))
                    return false;

                foreach(IIngredient ingredient in ingredients)
                if (double.IsNaN(ingredient.Quantity) || double.IsNaN(ingredient.Concentration))
                    return false;
            }

            // Validate result for out of range values
            if (!acceptAutOfRangeAsValid)
            {
                if (!IsQuantityValid(total, true) || !IsConcentrationValid(total, true))
                    return false;

                foreach (IIngredient ingredient in ingredients)
                    if (!IsQuantityValid(ingredient, true) || !IsConcentrationValid(ingredient, true))
                        return false;
            }

            // Validate result mathematically
            double ingredientsQuantitySum = ingredients.Sum(i => i.Quantity);
            double deltaQuantity = Math.Abs(ingredientsQuantitySum - total.Quantity);
            if (deltaQuantity > precision || (double.IsNaN(deltaQuantity) && acceptNaNAsValid))
                return false;

            double ingredientsProductUnits = ingredients.Sum(i => i.Quantity * i.Concentration);
            double totalProductUnits = total.Quantity * total.Concentration;
            double deltaProductUnits = Math.Abs(ingredientsProductUnits - totalProductUnits);
            if (deltaProductUnits > precision || (double.IsNaN(deltaProductUnits) && acceptNaNAsValid))
                return false;

            return true;// Calculation is valid
        }

        #endregion

        #region Calculation

        /// <summary>
        /// Calculate and set unknown values
        /// </summary>
        /// <param name="ingredients">Ingredients</param>
        /// <param name="total">Total</param>
        public static void Calculate(IEnumerable<IIngredient> ingredients, IIngredient total)
        {
            CalculationCase calculationCase = GetCalculationCase(ingredients, total);
            switch (calculationCase)
            {
                case CalculationCase.TwoQuantities:
                    CalcTwoQuantities(ingredients, total);
                    break;

                case CalculationCase.OneConcentrationOneQuantity:
                    CalcQuantityAndConcentration(ingredients, total);
                    break;
            }
        }

        /// <summary>
        /// Calculate and set unknown values according the calculation case - one unknown concentration, one unknown quantity
        /// </summary>
        /// <param name="ingredients">Ingredients</param>
        /// <param name="total">Total</param>
        private static void CalcQuantityAndConcentration(IEnumerable<IIngredient> ingredients, IIngredient total)
        {
            // Calculate uknown quantity
            double quantityCalc = ingredients
                .Where(i => !i.IsQuantityUnknown)
                .Sum(i => i.Quantity);

            if (!total.IsQuantityUnknown)
                quantityCalc = total.Quantity - quantityCalc;

            // Set calculated quantity
            IIngredient targetQuantityIngredient = ingredients
                .Where(i => i.IsQuantityUnknown)
                .SingleOrDefault();

            if (targetQuantityIngredient == null)
                targetQuantityIngredient = total;

            targetQuantityIngredient.Quantity = quantityCalc;

            //------------------------------------------------

            // Calculate concentration (expected quantity calculation and set to have been done)
            double productUnits = ingredients
                .Where(i => !i.IsConcentrationUnknown)
                .Sum(i => i.Quantity * i.Concentration);

            if (!total.IsConcentrationUnknown)
                productUnits = (total.Quantity * total.Concentration) - productUnits;

            // Set calculated concentration
            IIngredient targetConcentrationIngredient = ingredients
                .Where(i => i.IsConcentrationUnknown)
                .SingleOrDefault();

            if (targetConcentrationIngredient == null)
                targetConcentrationIngredient = total;

            targetConcentrationIngredient.Concentration = productUnits / targetConcentrationIngredient.Quantity;
        }

        /// <summary>
        /// Calculate and set unknown values according the calculation case - two unknown quantities
        /// </summary>
        /// <param name="ingredients">Ingredients</param>
        /// <param name="total">Total</param>
        private static void CalcTwoQuantities(IEnumerable<IIngredient> ingredients, IIngredient total)
        {
            // Sum of all known quantities in the left part of the euqation
            double Qsum = ingredients
                .Where(i => !i.IsQuantityUnknown)
                .Sum(i => i.Quantity);

            // Sum of all known products (quantity * concentration) in the left part of the euqation
            double Psum = ingredients
                .Where(i => !i.IsQuantityUnknown)
                .Sum(i => i.Quantity * i.Concentration);

            if (total.IsQuantityUnknown)
            {// one of both unknown quantities is in the left part of the equation the other one is in the right - "total"

                IIngredient x = ingredients
                    .Where(i => i.IsQuantityUnknown)
                    .Single();

                // Note that NaN is possible result if devision by zero is performed
                double Qt = (Psum - Qsum * x.Concentration) / (total.Concentration - x.Concentration);
                double Qx = Qt - Qsum;

                // Set calcualations
                total.Quantity = Qt;
                x.Quantity = Qx;
            }
            else
            {// both unknown quantities are in the left part of the equation

                IIngredient x = ingredients
                    .Where(i => i.IsQuantityUnknown)
                    .First();

                IIngredient y = ingredients
                    .Where(i => i.IsQuantityUnknown && i != x)
                    .First();

                // Note that NaN is possible result if devision by zero is performed
                double Qy = (Psum + x.Concentration * total.Quantity - x.Concentration * Qsum - total.Concentration * total.Quantity) / (x.Concentration - y.Concentration);
                double Qx = total.Quantity - Qsum - Qy;

                // Set calcualations
                y.Quantity = Qy;
                x.Quantity = Qx;
            }
        }

        /// <summary>
        /// Gets calculation case which have to be performed
        /// </summary>
        /// <param name="ingredients">Ingredients</param>
        /// <param name="total">Total</param>
        /// <returns>Calculation case that have to be performed</returns>
        private static CalculationCase GetCalculationCase(IEnumerable<IIngredient> ingredients, IIngredient total)
        {
            int countOfUnknowndQuantities = ingredients.Where(i => i.IsQuantityUnknown).Count();
            countOfUnknowndQuantities += total.IsQuantityUnknown ? 1 : 0;

            int countOfUnknownConcentrations = ingredients.Where(i => i.IsConcentrationUnknown).Count();
            countOfUnknownConcentrations += total.IsConcentrationUnknown ? 1 : 0;

            if (countOfUnknowndQuantities == 2 && countOfUnknownConcentrations == 0)
                return CalculationCase.TwoQuantities;
            else if (countOfUnknowndQuantities == 1 && countOfUnknownConcentrations == 1)
                return CalculationCase.OneConcentrationOneQuantity;
            else
                throw new ApplicationException($"Invalid calculation case: Unknown quantities {countOfUnknowndQuantities}; Unknown concentrations {countOfUnknownConcentrations};");
        }

        /// <summary>
        /// Main calculation scenarios
        /// </summary>
        private enum CalculationCase
        {
            TwoQuantities,
            OneConcentrationOneQuantity,
        }

        #endregion
    }

    /// <summary>
    /// Presents validation of unknown values for calculation selection
    /// </summary>
    public enum UVCountValidationResult
    {
        OK,
        ExcessiveSelections,
        ExcessiveConcentrationSelections,
        InsufficientSelections,
    }
}
