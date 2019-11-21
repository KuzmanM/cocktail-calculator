using CocktailCalculator;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class CalculationsTest
    {
        #region One Quantity One Concentration

        /// <summary>
        /// Unknown total quantity and total concentration
        /// </summary>
        [Fact]
        public void TotalQuantityTotalConcentrationTest()
        {
            // Arrange
            Ingredient total = new Ingredient(double.NaN, double.NaN, true, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        /// <summary>
        /// Unknown ingredient1 quantity and ingredient1 concentration
        /// </summary>
        [Fact]
        public void Ingredient1QuantityIngredient1ConcentrationTest()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(double.NaN, double.NaN, true, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        /// <summary>
        /// Unknown ingredient2 quantity and ingredient2 concentration
        /// </summary>
        [Fact]
        public void Ingredient2QuantityIngredient2ConcentrationTest()
        {
            // Arrange
            Ingredient total = new Ingredient(150, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(double.NaN, double.NaN, true, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        /// <summary>
        /// Unknown ingredient3 quantity and ingredient3 concentration
        /// </summary>
        [Fact]
        public void Ingredient3QuantityIngredient3ConcentrationTest()
        {
            // Arrange
            Ingredient total = new Ingredient(150, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(double.NaN, double.NaN, true, true);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        /// <summary>
        /// Unknown total quantity and Ingredient1 concentration
        /// </summary>
        [Fact]
        public void TotalQuantityIngredient1ConcentrationTest()
        {
            // Arrange
            Ingredient total = new Ingredient(double.NaN, 10, true, false);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, double.NaN, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        /// <summary>
        /// Unknown Ingredient1 quantity and total concentration
        /// </summary>
        [Fact]
        public void Ingredient1QuantityTotalConcentrationTest()
        {
            // Arrange
            Ingredient total = new Ingredient(150, double.NaN, false, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(double.NaN, 100, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        /// <summary>
        /// Unknown Ingredient1 quantity and Ingredient2 concentration
        /// </summary>
        [Fact]
        public void Ingredient1QuantityIngredient2ConcentrationTest()
        {
            // Arrange
            Ingredient total = new Ingredient(150, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(double.NaN, 100, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, double.NaN, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        /// <summary>
        /// Unknown Ingredient1 quantity and Ingredient2 concentration
        /// </summary>
        [Fact]
        public void Ingredient3QuantityIngredient2ConcentrationTest()
        {
            // Arrange
            Ingredient total = new Ingredient(150, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, double.NaN, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(double.NaN, 0, true, false);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        #endregion

        #region Two Quantities

        /// <summary>
        /// Unknown total and Ingredient1 quantity
        /// </summary>
        [Fact]
        public void TotalIngredient1QuantityTest()
        {
            // Arrange
            Ingredient total = new Ingredient(double.NaN, 40, true, false);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(double.NaN, 100, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        /// <summary>
        /// Unknown Ingredient1 and Ingredient2 quantity
        /// </summary>
        [Fact]
        public void Ingredient1Ingredient2QuantityTest()
        {
            // Arrange
            Ingredient total = new Ingredient(150, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(double.NaN, 100, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(double.NaN, 20, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        /// <summary>
        /// Unknown Ingredient2 and Ingredient3 quantity
        /// </summary>
        [Fact]
        public void Ingredient2Ingredient3QuantityTest()
        {
            // Arrange
            Ingredient total = new Ingredient(90, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(double.NaN, 20, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(double.NaN, 0, true, false);
            ingredients.Add(ingredient);

            // Act
            Calculator.Calculate(ingredients, total);

            // Assert
            bool isOK = Calculator.ValidateCalculationResult(ingredients, total);
            Assert.True(isOK, "Invalid calcualtion!");
        }

        #endregion
    }
}
