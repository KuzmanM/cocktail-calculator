using Common.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace CocktailCalculator
{
    public class MainWindowModel : ModelBase
    {
        #region Global fields

        /// <summary>
        /// Model view
        /// </summary>
        private MainWindow _view;

        #endregion

        #region Commands

        public ICommand CalculateCommand { get; set; }

        public ICommand LoadDataCommand { get; set; }

        public ICommand SaveDataCommand { get; set; }

        public ICommand AddIngredientCommand { get; set; }

        public ICommand DeleteIngredientCommand { get; set; }

        #endregion

        #region Binding Properties

        /// <summary>
        /// All ingredients
        /// </summary>
        public ObservableCollection<IngredientModel> Ingredients { get; private set; }

        /// <summary>
        /// Total quantity and concentration
        /// </summary>
        public IngredientModel Total { get; private set; }

        private bool _isSystemOfEquationsValid;
        /// <summary>
        /// Gets true if the system of equations is valid.
        /// Only cooperative rules are validated, the individual values validation are not related to that property.
        /// </summary>
        public bool IsSystemOfEquationsValid
        {
            get { return _isSystemOfEquationsValid; }
            set
            {
                if (_isSystemOfEquationsValid != value)
                {
                    _isSystemOfEquationsValid = value;
                    RisePropertyChanged();
                }
            }
        }

        private string _systemOfEquationsValidationMessage;
        /// <summary>
        /// Gets info about system of equations validity.
        /// Related to cooperative validation rules only. The individual values validation rules are not included.
        /// </summary>
        public string SystemOfEquationsValidationMessage
        {
            get { return _systemOfEquationsValidationMessage; }
            set
            {
                if (_systemOfEquationsValidationMessage != value)
                {
                    _systemOfEquationsValidationMessage = value;
                    RisePropertyChanged();
                }
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view">Model view</param>
        public MainWindowModel(MainWindow view)
        {
            _view = view;

            // Try to load initial data configuration from a file
            List<Ingredient> fileItems = Deserialize(@".\load.xml");
            if (fileItems != null && fileItems.Count > 2)
            {// Try to initialize from file

                Ingredient total = fileItems[0];
                IEnumerable<IngredientModel> ingredients = fileItems
                    .GetRange(1, fileItems.Count - 1)
                    .Select(i => new IngredientModel(view, i, IngredientChangedEventHandler));

                Total = new IngredientModel(view, total, IngredientChangedEventHandler);
                Ingredients = new ObservableCollection<IngredientModel>(ingredients);
            }
            else
            {// default initialization

                string totalStr =(string)_view.Resources["Total"];
                // Total
                Total = new IngredientModel(view, totalStr, 0, 45, IngredientChangedEventHandler, true, false);

                // Ingredients
                string waterStr = (string)_view.Resources["Water"];
                IngredientModel ingredient1 = new IngredientModel(view, waterStr, 0, 0, IngredientChangedEventHandler, true, false);
                string strongAlcoholStr = (string)_view.Resources["StrongAlcohol"];
                IngredientModel ingredient2 = new IngredientModel(view, strongAlcoholStr, 30, 60, IngredientChangedEventHandler, false, false);
                Ingredients = new ObservableCollection<IngredientModel>(new[] { ingredient1, ingredient2 });
            }

            // Commands
            CalculateCommand = new Command(CalculateCommandHandler, CalculateCommandCanExecute);
            LoadDataCommand = new Command(LoadDataCommandHandler, LoadDataCommandCanExecute);
            SaveDataCommand = new Command(SaveDataCommandHandler, SaveDataCommandCanExecute);
            AddIngredientCommand = new Command(AddIngredientCommandHandler, AddIngredientCommandCanExecute);
            DeleteIngredientCommand = new Command(DeleteIngredientCommandHandler, DeleteMappingItemCommandCanExecute);

            // Validate
            ValidateSystemOfEquations();

            // Calculate
            CalculateIfValid();
        }

        #endregion

        #region Command Handlers

        /// <summary>
        /// Execute calculation
        /// </summary>
        /// <param name="parameter">Parameter of the command handler</param>
        private void CalculateCommandHandler(object parameter)
        {
            string error = Calculator.Validate(Ingredients, Total);
            if (string.IsNullOrEmpty(error))
            {
                // Execution is in external thread just because some thread blocking (sleep) is required
                // to slow down the calculation and make it noticeable
                _view.Dispatcher.Invoke(() =>
                    Task.Run(() =>
                    {
                        // Set NaN values and wait some time - makes calculation process noticeable
                        ClearDataForCalculation(Ingredients, Total);
                        Thread.Sleep(200);

                        // Perform calculation and set values
                        Calculator.Calculate(Ingredients, Total);
                    })
                );
            }
            else
            {
                _view.ShowError(error);
            }
        }

        /// <summary>
        /// Enable or disable CalculateCommand execution
        /// </summary>
        /// <param name="parameter">Parameter of the command handler</param>
        /// <returns>True if execution is enabled</returns>
        private bool CalculateCommandCanExecute(object parameter)
        {
            bool areAllDataValid = IsValid();
            return areAllDataValid;
        }


        /// <summary>
        /// Load calcualtion data
        /// </summary>
        /// <param name="parameter">Parameter of the command handler</param>
        private void LoadDataCommandHandler(object parameter)
        {
            // Select file
            string filePath = _view.ShowOpenFileDialog();
            if (string.IsNullOrEmpty(filePath))
                return;

            // Deserialize
            List<Ingredient> fileItems = Deserialize(filePath);
            if (fileItems == null || fileItems.Count < 3)
            {
                _view.ShowError($"Invalid file: {filePath}");
                return;
            }

            // Validate loaded data
            Ingredient total = fileItems[0];
            List<Ingredient> ingredients = fileItems.GetRange(1, fileItems.Count - 1);
            string validationError = Calculator.Validate(ingredients, total);
            if (!string.IsNullOrEmpty(validationError))
            {
                _view.ShowError(validationError);
                return;
            }

            // Set data
            Total.SetSimpleIngredient(total);
            Ingredients.Clear();
            foreach (Ingredient i in ingredients)
                Ingredients.Add(new IngredientModel(_view, i, IngredientChangedEventHandler));

            // Calculate
            CalculateIfValid();
        }

        /// <summary>
        /// Enable or disable LoadDataCommand execution
        /// </summary>
        /// <param name="parameter">Parameter of the command handler</param>
        /// <returns>True if execution is enabled</returns>
        private bool LoadDataCommandCanExecute(object parameter)
        {
            return true;
        }


        /// <summary>
        /// Save calcualtion data
        /// </summary>
        /// <param name="parameter">Parameter of the command handler</param>
        private void SaveDataCommandHandler(object parameter)
        {   
            // Convert ingredients to list of simpel objects for serialization. The total is first
            Ingredient total = Total.CreateSimpleIngredient();
            List<Ingredient> ingredients = Ingredients.Select(i => i.CreateSimpleIngredient()).ToList();
            ingredients.Insert(0, total);
            
            // Serialize
            string filePath = _view.ShowSaveFileDialog();
            if(!string.IsNullOrEmpty(filePath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<Ingredient>));
                using (TextWriter writer = new StreamWriter(filePath))
                {
                    ser.Serialize(writer, ingredients);
                }
            }
        }

        /// <summary>
        /// Enable or disable SaveDataCommand execution
        /// </summary>
        /// <param name="parameter">Parameter of the command handler</param>
        /// <returns>True if execution is enabled</returns>
        private bool SaveDataCommandCanExecute(object parameter)
        {
            bool areAllDataValid = IsValid();
            return areAllDataValid;
        }


        /// <summary>
        /// Add new ingredient
        /// </summary>
        /// <param name="parameter">Parameter of the command handler</param>
        private void AddIngredientCommandHandler(object parameter)
        {
            IngredientModel ingredient = new IngredientModel(_view, string.Empty, 0, 0, IngredientChangedEventHandler);
            Ingredients.Add(ingredient);
            ValidateSystemOfEquations();
            CalculateIfValid();
        }

        /// <summary>
        /// Enable or disable AddIngredientCommandHandler execution
        /// </summary>
        /// <param name="parameter">Parameter of the command handler</param>
        /// <returns>True if execution is enabled</returns>
        private bool AddIngredientCommandCanExecute(object parameter)
        {
            return true;
        }


        /// <summary>
        /// Delete the ingredient
        /// </summary>
        /// <param name="parameter">Parameter of the command handler</param>
        private void DeleteIngredientCommandHandler(object parameter)
        {
            IngredientModel itemToRemove = parameter as IngredientModel;
            if (itemToRemove != null)
            {
                int itemIndex = Ingredients.IndexOf(itemToRemove);

                // Rmove the item from the list
                Ingredients.RemoveAt(itemIndex);
                ValidateSystemOfEquations();
                CalculateIfValid();
            }
        }

        /// <summary>
        /// Enable or disable DeleteIngredientCommand execution
        /// </summary>
        /// <param name="parameter">Parameter of the command handler</param>
        /// <returns>True if execution is enabled</returns>
        private bool DeleteMappingItemCommandCanExecute(object parameter)
        {
            return Ingredients.Count() > 2;
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate system of equations
        /// Related to cooperative validation rules only. The individual values validation rules are not included.
        /// </summary>
        private void ValidateSystemOfEquations()
        {
            // Error messages
            string ingredientsCountMsg = (string)_view.Resources["IngredientsCountError"];
            string excessiveSelectionsMsg = (string)_view.Resources["ExcessiveSelectionsError"];
            string insufficientSelectionsMsg = (string)_view.Resources["InsufficientSelectionsError"];
            string excessiveConcentrationSelectionsMsg = (string)_view.Resources["ExcessiveConcentrationSelectionsError"];

            // Current validation error message
            string validationMsg = null;

            // Validate unknown values count
            UVCountValidationResult countValidationResult = Calculator.GetUnknownValuesCountState(Ingredients, Total);
            switch (countValidationResult)
            {
                case UVCountValidationResult.ExcessiveSelections:
                    validationMsg = excessiveSelectionsMsg;
                    break;

                case UVCountValidationResult.InsufficientSelections:
                    validationMsg = insufficientSelectionsMsg;
                    break;

                case UVCountValidationResult.ExcessiveConcentrationSelections:
                    validationMsg = excessiveConcentrationSelectionsMsg;
                    break;
            }

            // Validation ingredients count
            if (Ingredients.Count() < 2)
                validationMsg = ingredientsCountMsg;

            // Set the validation error if exist or clean all errors of the model
            if(validationMsg == null)
            {
                SystemOfEquationsValidationMessage = null;
                IsSystemOfEquationsValid = true;
                CleanErrors(true);
            }
            else
            {
                SystemOfEquationsValidationMessage = validationMsg;
                IsSystemOfEquationsValid = false;
                AddError(validationMsg, string.Empty);
            }
        }

        /// <summary>
        /// Check if everything is in valid state (all models)
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            //bool hasIngredientError = Ingredients.Any(i => i.HasErrors);
            //bool canExecute = !(hasIngredientError || Total.HasErrors || HasErrors);

            bool invalidIngredientItem = Ingredients.Any(i => !i.IsQuantityValid || !i.IsConcentrationValid);
            bool invalidTotalItem = !Total.IsQuantityValid || !Total.IsConcentrationValid;
            bool isValid = !(invalidIngredientItem || invalidTotalItem || !IsSystemOfEquationsValid);

            return isValid;
        }

        #endregion

        #region Event Handlers

        private void IngredientChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // IngrеdientModel property names important for calculation and validation  
            string isConcentrationUnknownName = nameof(IngredientModel.IsConcentrationUnknown);
            string isQuantityUnknownName = nameof(IngredientModel.IsQuantityUnknown);
            string quantityName = nameof(IngredientModel.Quantity);
            string concentrationName = nameof(IngredientModel.Concentration);

            // ValidateSystemOfEquations
            if (e.PropertyName == isConcentrationUnknownName || e.PropertyName == isQuantityUnknownName)
                ValidateSystemOfEquations();

            // Calculate if data are changed
            IngredientModel senderIngredientModel = sender as IngredientModel;
            if (senderIngredientModel != null
                && (
                        e.PropertyName == isConcentrationUnknownName
                     || e.PropertyName == isQuantityUnknownName
                     || (e.PropertyName == quantityName && !senderIngredientModel.IsQuantityUnknown)
                     || (e.PropertyName == concentrationName && !senderIngredientModel.IsConcentrationUnknown)
                   )
               )
                CalculateIfValid();
        }

        /// <summary>
        /// Execute calculate if data are valid
        /// </summary>
        private void CalculateIfValid()
        {
            bool isValid = IsValid();// TO DO -> Consider using of Calculator.Validate(Ingredients, Total); as more independent solution
            if (isValid)
                Calculator.Calculate(Ingredients, Total);
        }

        #endregion

        #region Help functions

        /// <summary>
        /// Set NaN to all unknown values selected for calculation
        /// </summary>
        /// <param name="ingredients">Ingredients</param>
        /// <param name="total">Total</param>
        public static void ClearDataForCalculation(IEnumerable<IngredientModel> ingredients, IngredientModel total)
        {
            foreach (IngredientModel ingredient in ingredients)
            {
                if (ingredient.IsQuantityUnknown)
                    ingredient.Quantity = double.NaN;

                if (ingredient.IsConcentrationUnknown)
                    ingredient.Concentration = double.NaN;
            }

            if (total.IsQuantityUnknown)
                total.Quantity = double.NaN;

            if (total.IsConcentrationUnknown)
                total.Concentration = double.NaN;
        }

        /// <summary>
        /// Deserialize ingredients configuration
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>List of ingredients or null</returns>
        public static List<Ingredient> Deserialize(string filePath)
        {
            List<Ingredient> fileItems = null;

            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<Ingredient>));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    fileItems = (List<Ingredient>)ser.Deserialize(fs);
                }
            }
            catch (Exception)
            { }

            return fileItems;
        }

        #endregion
    }
}
