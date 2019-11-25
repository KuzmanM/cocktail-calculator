using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Common.WPF.Controls
{
    /// <summary>
    /// Interaction logic for DoubleBox.xaml
    /// </summary>
    public partial class DoubleBox : UserControl
    {
        #region Initialization

        private char _numberDecimalSeparator;

        public DoubleBox()
        {
            InitializeComponent();
            _numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        }

        #endregion

        /// <summary>
        /// Text box value
        /// </summary>
        private string TextBoxValue
        {
            get { return numericBox.Text; }
            set
            {
                if (numericBox.Text != value)
                {
                    numericBox.Text = value;
                    // the line below is inportant only for numericBox_TextChanged
                    numericBox.CaretIndex = numericBox.Text.Length;
                }
            }
        }

        /// <summary>
        /// Set double value to the text box
        /// </summary>
        /// <param name="value">double value</param>
        private void SetTextBoxValue(double value)
        {
            string valueBindingStringFormat = GetValueBindingStringFormat();
            TextBoxValue = value.ToString(valueBindingStringFormat);
        }

        #region IsReadOnly

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(DoubleBox), new PropertyMetadata(false, new PropertyChangedCallback(OnIsReadOnlyPropertyChanged)));

        /// <summary>
        /// Is read only
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }
            set
            {
                if (IsReadOnly != value)
                {
                    SetValue(IsReadOnlyProperty, value);
                }
            }
        }

        private static void OnIsReadOnlyPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            DoubleBox targetControl = target as DoubleBox;
            targetControl.numericBox.IsReadOnly = (bool)e.NewValue;
        }

        #endregion

        #region Value

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(DoubleBox), new PropertyMetadata(0.0, new PropertyChangedCallback(OnValuePropertyChanged)));

        /// <summary>
        /// Double value
        /// </summary>
        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                if (Value != value)
                {
                    SetValue(ValueProperty, value);
                    RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
                }
            }
        }

        /// <summary>
        /// "Value" dependency property callback
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="e">arguments</param>
        private static void OnValuePropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            DoubleBox targetControl = target as DoubleBox;

            double.TryParse(targetControl.TextBoxValue, out double oldValue);
            double newValue = (double)e.NewValue;

            if (newValue != oldValue)// avoid infinite recursion 
                targetControl.SetTextBoxValue(newValue);
        }

        /// <summary>
        /// Gets StringFormat parameter of "Value" dependency property binding
        /// </summary>
        /// <returns>StringFormat</returns>
        private string GetValueBindingStringFormat()
        {
            BindingExpression be = GetBindingExpression(ValueProperty);
            Binding pb = be?.ParentBinding;
            string stringFormat = pb?.StringFormat;
            return stringFormat;
        }

        #endregion

        #region ValueChanged event

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(ValueChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DoubleBox));

        /// <summary>
        /// Value changed event
        /// </summary>
        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Set the default value of "Value" dependency property to the text box
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void numericBox_Loaded(object sender, RoutedEventArgs e)
        {
            SetTextBoxValue(Value);
        }

        /// <summary>
        /// As result of text property change, set actual value to "Value" dependency property
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void numericBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxValue))
            {
                // Set only the dependancy property and leave the text box empty
                Value = 0;
            }
            else if ((TextBoxValue.StartsWith("-") || TextBoxValue.StartsWith("+")) && TextBoxValue.Length == 1)
            {
                // Set only the dependancy property and accept the '-' or '+' character as correct
                Value = 0;
            }
            else if (!double.TryParse(TextBoxValue, out double numericBoxValue))
            {
                // Rollback the value
                SetTextBoxValue(Value);
            }
            else
            {
                // Set the actual value
                Value = numericBoxValue;
            }
        }

        /// <summary>
        /// Prevent incorrect user input
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void numericBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            char inputCharacter = e.Text[0];
            // Set e.Handled to true if you want to ignore the inputCharacter
            bool ignoreInputCharacter = !(char.IsDigit(inputCharacter) || inputCharacter == _numberDecimalSeparator || inputCharacter == '-' || inputCharacter == '+');
            e.Handled = ignoreInputCharacter;
        }

        /// <summary>
        /// Restore the text box value in case that it has been deleted
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void numericBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetTextBoxValue(Value);
        }

        #endregion
    }
}
