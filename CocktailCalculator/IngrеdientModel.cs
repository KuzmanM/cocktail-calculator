using Common.WPF;
using System.ComponentModel;

namespace CocktailCalculator
{
    /// <summary>
    /// Represents cocktail ingredient
    /// </summary>
    public class IngredientModel : ModelBase, IIngredient
    {
        private MainWindow _view;

        #region Binding Properties

        private string _description;
        /// <summary>
        /// Ingredient description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    RisePropertyChanged();
                }
            }
        }

        private bool _isQuantityUnknown;
        /// <summary>
        /// Is Quantity selected as unknown
        /// </summary>
        public bool IsQuantityUnknown
        {
            get { return _isQuantityUnknown; }
            set
            {
                if (_isQuantityUnknown != value)
                {
                    _isQuantityUnknown = value;
                    ValidateQuantity();
                    RisePropertyChanged();
                }
            }
        }

        private double _quantity;
        /// <summary>
        /// Ingredient quantity
        /// </summary>
        public double Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    ValidateQuantity();
                    RisePropertyChanged();
                }
            }
        }

        private bool _isQuantityValid;
        /// <summary>
        /// Gets true if the quantity value is valid
        /// </summary>
        public bool IsQuantityValid
        {
            get { return _isQuantityValid; }
            set
            {
                if (_isQuantityValid != value)
                {
                    _isQuantityValid = value;
                    RisePropertyChanged();
                }
            }
        }

        private string _quantityValidationMessage;
        /// <summary>
        /// Gets some info if the quantity is not valid
        /// </summary>
        public string QuantityValidationMessage
        {
            get { return _quantityValidationMessage; }
            set
            {
                if (_quantityValidationMessage != value)
                {
                    _quantityValidationMessage = value;
                    RisePropertyChanged();
                }
            }
        }

        private bool _isConcentrationUnknown;
        /// <summary>
        /// Is Concentration selected as unknown
        /// </summary>
        public bool IsConcentrationUnknown
        {
            get { return _isConcentrationUnknown; }
            set
            {
                if (_isConcentrationUnknown != value)
                {
                    _isConcentrationUnknown = value;
                    ValidateConcentration();
                    RisePropertyChanged();
                }
            }
        }

        private double _concentration;
        /// <summary>
        /// Concentration
        /// </summary>
        public double Concentration
        {
            get { return _concentration; }
            set
            {
                if (_concentration != value)
                {
                    _concentration = value;
                    ValidateConcentration();
                    RisePropertyChanged();
                }
            }
        }

        private bool _isConcentrationValid;
        /// <summary>
        /// Gets true if the concentration value is valid
        /// </summary>
        public bool IsConcentrationValid
        {
            get { return _isConcentrationValid; }
            set
            {
                if (_isConcentrationValid != value)
                {
                    _isConcentrationValid = value;
                    RisePropertyChanged();
                }
            }
        }

        private string _concentrationValidationMessage;
        /// <summary>
        /// Gets some info if the concentration is not valid
        /// </summary>
        public string ConcentrationValidationMessage
        {
            get { return _concentrationValidationMessage; }
            set
            {
                if (_concentrationValidationMessage != value)
                {
                    _concentrationValidationMessage = value;
                    RisePropertyChanged();
                }
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="view">Model view</param>
        /// <param name="description">Ingredient description</param>
        /// <param name="quantity">Ingredient quantity</param>
        /// <param name="concentration">Concentration</param>
        /// <param name="changedEventHandler">Property changed event handler</param>
        /// <param name="isQuantityUnknown">Is Quantity selected as unknown</param>
        /// <param name="isConcentrationUnknown">Is Concentration selected as unknown</param>
        public IngredientModel(MainWindow view, string description, double quantity, double concentration, PropertyChangedEventHandler changedEventHandler, bool isQuantityUnknown = false, bool isConcentrationUnknown = false)
        {
            _view = view;

            _description = description;
            _quantity = quantity;
            _concentration = concentration;

            _isQuantityUnknown = isQuantityUnknown;
            _isConcentrationUnknown = isConcentrationUnknown;

            if (changedEventHandler != null)
                PropertyChanged += changedEventHandler;

            ValidateQuantity();
            ValidateConcentration();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="view">Model view</param>
        /// <param name="ingredient">Simple Ingredient object</param>
        /// <param name="changedEventHandler">Property changed event handler</param>
        public IngredientModel(MainWindow view, Ingredient ingredient, PropertyChangedEventHandler changedEventHandler) : 
            this(view, ingredient.Description, ingredient.Quantity, ingredient.Concentration, changedEventHandler, ingredient.IsQuantityUnknown, ingredient.IsConcentrationUnknown)
        { }

        /// <summary>
        /// Set values of simple ingredient object
        /// </summary>
        /// <param name="ingredient">simple ingredient object</param>
        public void SetSimpleIngredient(Ingredient ingredient)
        {
            _description = ingredient.Description;
            _quantity = ingredient.Quantity;
            _concentration = ingredient.Concentration;

            _isQuantityUnknown = ingredient.IsQuantityUnknown;
            _isConcentrationUnknown = ingredient.IsConcentrationUnknown;

            ValidateQuantity();
            ValidateConcentration();
        }

        #endregion

        #region Validation

        private void ValidateQuantity()
        {
            string errorMessage = (string)_view.Resources["QuantityError"];
            string propertyName = nameof(Quantity);

            if (Calculator.IsQuantityValid(this))
            {
                QuantityValidationMessage = null;
                IsQuantityValid = true;
                RemoveError(errorMessage, propertyName);
            }
            else
            {
                QuantityValidationMessage = errorMessage;
                IsQuantityValid = false;
                AddError(errorMessage, propertyName);
            }
        }

        private void ValidateConcentration()
        {
            string errorMessage = (string)_view.Resources["ConcentrationError"]; ;
            string propertyName = nameof(Concentration);

            if (Calculator.IsConcentrationValid(this))
            {
                ConcentrationValidationMessage = null;
                IsConcentrationValid = true;
                RemoveError(errorMessage, propertyName);
            }
            else
            {
                ConcentrationValidationMessage = errorMessage;
                IsConcentrationValid = false;
                AddError(errorMessage, propertyName);
            }
        }

        #endregion

        /// <summary>
        /// Creates cloning of simple ingredient object
        /// </summary>
        /// <returns>simple Ingredient object</returns>
        public Ingredient CreateSimpleIngredient()
        {
            Ingredient i = new Ingredient(Description, Quantity, Concentration, IsQuantityUnknown, IsConcentrationUnknown);
            return i;
        }

        public override string ToString()
        {
            string qu = IsQuantityUnknown ? "*" : string.Empty;
            string cu = IsConcentrationUnknown ? "*" : string.Empty;
            return $"{Description} -> Quantity{qu}:{Quantity}   Concentration{cu}:{Concentration}";
        }
    }
}
