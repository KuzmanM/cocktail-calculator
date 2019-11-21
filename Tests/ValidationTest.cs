using CocktailCalculator;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class ValidationTest
    {
        #region IsQuantityValid

        [Fact]
        public void NegativeQuantity()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(-1, 100);

            // Act
            bool isValid = Calculator.IsQuantityValid(ingredient);

            // Assert
            Assert.False(isValid, "Validation error!");
        }

        [Fact]
        public void ZeroQuantity()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(0, 100);

            // Act
            bool isValid = Calculator.IsQuantityValid(ingredient);

            // Assert
            Assert.False(isValid, "Validation error!");
        }

        [Fact]
        private void NaNQuantity()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(double.NaN, 100);

            // Act
            bool isValid = Calculator.IsQuantityValid(ingredient);

            // Assert
            Assert.False(isValid, "Validation error!");
        }

        [Fact]
        public void NegativeUnknownQuantity()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(-1, 100, true);

            // Act
            bool isValid = Calculator.IsQuantityValid(ingredient);

            // Assert
            Assert.True(isValid, "Validation error!");
        }

        [Fact]
        public void ZeroUnknownQuantity()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(0, 100, true);

            // Act
            bool isValid = Calculator.IsQuantityValid(ingredient);

            // Assert
            Assert.True(isValid, "Validation error!");
        }

        [Fact]
        public void NaNUnknownQuantity()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(double.NaN, 100, true);

            // Act
            bool isValid = Calculator.IsQuantityValid(ingredient);

            // Assert
            Assert.True(isValid, "Validation error!");
        }

        #endregion

        #region IsConcentrationValid

        [Fact]
        public void NegativeConcentration()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(1, -1);

            // Act
            bool isValid = Calculator.IsConcentrationValid(ingredient);

            // Assert
            Assert.False(isValid, "Validation error!");
        }

        [Fact]
        public void ZeroConcentration()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(1, 0);

            // Act
            bool isValid = Calculator.IsConcentrationValid(ingredient);

            // Assert
            Assert.True(isValid, "Validation error!");
        }

        [Fact]
        public void MaxConcentration()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(1, 100);

            // Act
            bool isValid = Calculator.IsConcentrationValid(ingredient);

            // Assert
            Assert.True(isValid, "Validation error!");
        }

        [Fact]
        public void OverMaxConcentration()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(1, 100.1);

            // Act
            bool isValid = Calculator.IsConcentrationValid(ingredient);

            // Assert
            Assert.False(isValid, "Validation error!");
        }

        [Fact]
        public void NaNConcentration()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(1, double.NaN);

            // Act
            bool isValid = Calculator.IsConcentrationValid(ingredient);

            // Assert
            Assert.False(isValid, "Validation error!");
        }

        [Fact]
        public void NegativeUnknownConcentration()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(1, -1, false, true);

            // Act
            bool isValid = Calculator.IsConcentrationValid(ingredient);

            // Assert
            Assert.True(isValid, "Validation error!");
        }

        [Fact]
        public void ZeroUnknownConcentration()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(1, 100, false, true);

            // Act
            bool isValid = Calculator.IsConcentrationValid(ingredient);

            // Assert
            Assert.True(isValid, "Validation error!");
        }

        [Fact]
        public void MaxeUnknownConcentration()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(1, 100, false, true);

            // Act
            bool isValid = Calculator.IsConcentrationValid(ingredient);

            // Assert
            Assert.True(isValid, "Validation error!");
        }

        [Fact]
        public void OverMaxeUnknownConcentration()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(1, 100, false, true);

            // Act
            bool isValid = Calculator.IsConcentrationValid(ingredient);

            // Assert
            Assert.True(isValid, "Validation error!");
        }

        [Fact]
        public void NaNUnknownConcentration()
        {
            // Arrange
            Ingredient ingredient = new Ingredient(1, 100, false, true);

            // Act
            bool isValid = Calculator.IsConcentrationValid(ingredient);

            // Assert
            Assert.True(isValid, "Validation error!");
        }

        #endregion

        #region GetUnknownValuesCountState

        [Fact]
        public void InsufficientSelections1()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.InsufficientSelections, result);
        }

        [Fact]
        public void InsufficientSelections2()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.InsufficientSelections, result);
        }

        [Fact]
        public void InsufficientSelections3()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, false, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.InsufficientSelections, result);
        }

        [Fact]
        public void InsufficientSelections4()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.InsufficientSelections, result);
        }

        [Fact]
        public void InsufficientSelections5()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.InsufficientSelections, result);
        }

        [Fact]
        public void ExcessiveSelections1()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0, true, false);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveSelections, result);
        }

        [Fact]
        public void ExcessiveSelections2()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, false);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveSelections, result);
        }

        [Fact]
        public void ExcessiveSelections3()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveSelections, result);
        }

        [Fact]
        public void ExcessiveSelections4()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, true, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveSelections, result);
        }

        [Fact]
        public void ExcessiveSelections5()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, true, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveSelections, result);
        }

        [Fact]
        public void ExcessiveSelections6()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveSelections, result);
        }

        [Fact]
        public void ExcessiveSelections7()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, false, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveSelections, result);
        }

        [Fact]
        public void ExcessiveSelections8()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, false, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveSelections, result);
        }

        [Fact]
        public void ExcessiveSelections9()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0, false, true);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveSelections, result);
        }

        [Fact]
        public void ExcessiveConcentrationSelections1()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveConcentrationSelections, result);
        }

        [Fact]
        public void ExcessiveConcentrationSelections2()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, false, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.ExcessiveConcentrationSelections, result);
        }

        [Fact]
        public void OK1()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.OK, result);
        }

        [Fact]
        public void OK2()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, true, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.OK, result);
        }

        [Fact]
        public void OK3()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, false);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.OK, result);
        }

        [Fact]
        public void OK4()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.OK, result);
        }

        [Fact]
        public void OK5()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.OK, result);
        }

        [Fact]
        public void OK6()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.OK, result);
        }

        [Fact]
        public void OK7()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, false, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.OK, result);
        }

        [Fact]
        public void OK8()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, false);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, false, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            UVCountValidationResult result = Calculator.GetUnknownValuesCountState(ingredients, total);

            // Assert
            Assert.Equal(UVCountValidationResult.OK, result);
        }

        #endregion

        #region Validate

        [Fact]
        public void Validate_OK()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, false);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, false);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            string result = Calculator.Validate(ingredients, total);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Validate_UnknownValuesCount()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, false);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            string result = Calculator.Validate(ingredients, total);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Validate_IngredientsCount()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, false);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(30, 100);
            ingredients.Add(ingredient);

            // Act
            string result = Calculator.Validate(ingredients, total);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Validate_IngredientsQuantity()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(0, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            string result = Calculator.Validate(ingredients, total);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Validate_IngredientsConcentration()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 40, true, true);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(1, 101);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            string result = Calculator.Validate(ingredients, total);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Validate_TotalQuantity()
        {
            // Arrange
            Ingredient total = new Ingredient(0, 40);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(0, 100);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            string result = Calculator.Validate(ingredients, total);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Validate_TotalConcentration()
        {
            // Arrange
            Ingredient total = new Ingredient(160, 101);
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient;
            ingredient = new Ingredient(1, 101);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(40, 20, true, true);
            ingredients.Add(ingredient);
            ingredient = new Ingredient(60, 0);
            ingredients.Add(ingredient);

            // Act
            string result = Calculator.Validate(ingredients, total);

            // Assert
            Assert.NotNull(result);
        }

        #endregion
    }
}
