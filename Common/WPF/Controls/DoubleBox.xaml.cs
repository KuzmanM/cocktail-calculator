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
        char _numberDecimalSeparator;

        public DoubleBox()
        {
            InitializeComponent();

            _numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            numericBox.Text = "0";
            Value = 0;
        }

        #region Value

        private static readonly DependencyProperty ValueProperty =
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

        private static void OnValuePropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            DoubleBox targetControl = target as DoubleBox;
            BindingExpression be = targetControl.GetBindingExpression(ValueProperty);
            Binding pb = be.ParentBinding;
            string sf = pb.StringFormat;
            targetControl.SetTextValue((double)e.NewValue, sf);
        }

        private void SetTextValue(double value, string format)
        {
            double.TryParse(numericBox.Text, out double oldValue);

            if (value != oldValue)
                numericBox.Text = value.ToString(format);
        }

        #endregion

        #region ValueChanged event

        private static readonly RoutedEvent ValueChangedEvent =
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
                    numericBox.CaretIndex = numericBox.Text.Length;
                }
            }
        }

        private void numericBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxValue))
            {
                // Set only the dependancy property and leave the text box empty
                Value = 0;
            }
            else if (TextBoxValue.StartsWith("-") && TextBoxValue.Length == 1)
            {
                // Set only the dependancy property and leave the '-' character
                Value = 0;
            }
            else if (!double.TryParse(TextBoxValue, out double numericBoxValue))
            {
                // Rollback the value
                TextBoxValue = Value.ToString();
            }
            else
            {
                // Set the actual value
                Value = numericBoxValue;
            }
        }

        private void numericBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            char inputCharacter = e.Text[0];
            // Set e.Handled to true if you want to ignore the inputCharacter
            bool ignoreInputCharacter = !(char.IsDigit(inputCharacter) || inputCharacter == _numberDecimalSeparator || inputCharacter == '-');
            e.Handled = ignoreInputCharacter;
        }

        private void numericBox_LostFocus(object sender, RoutedEventArgs e)
        {
            numericBox.Text = Value.ToString();
        }

        #endregion
    }
}
