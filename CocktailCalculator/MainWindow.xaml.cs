using Common.WPF.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace CocktailCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            // Change resource dictionary according current culture
            GlobalizationDictionaryManager gdm = new GlobalizationDictionaryManager();
            gdm.SetCulture(this);

            // Set DataContext
            _viewModel = new MainWindowModel(this);
            DataContext = _viewModel;

            // Set infinity delay of the tooltips
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));
        }

        /// <summary>
        /// Show error message
        /// </summary>
        /// <param name="message">Message</param>
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        const string fileFilter = "XML files (*.xml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*";

        /// <summary>
        /// Show Save File Dialog
        /// </summary>
        /// <returns>File path</returns>
        public string ShowSaveFileDialog()
        {
            string filePath = null;

            Microsoft.Win32.SaveFileDialog fd = new Microsoft.Win32.SaveFileDialog();
            fd.Filter = fileFilter;
            if (fd.ShowDialog() == true)
            {
                filePath = fd.FileName;
            }

            return filePath;
        }

        /// <summary>
        /// Show Open File Dialog
        /// </summary>
        /// <returns>File path</returns>
        public string ShowOpenFileDialog()
        {
            string filePath = null;

            Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
            fd.Filter = fileFilter;
            if (fd.ShowDialog() == true)
            {
                filePath = fd.FileName;
            }

            return filePath;
        }
    }
}
