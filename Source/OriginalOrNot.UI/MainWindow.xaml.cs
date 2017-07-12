namespace OriginalOrNot.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using OriginalOrNot.Logic;
    using OriginalOrNot.Shared;
    using System.Diagnostics;
    using OriginalOrNot.UI.UILoader;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private Engine _engine;
        private Loader _loader;
        private FileFormat _refFileFormat;
        private FileFormat _comparisonFileFormat;
        private TextTypeHelper _textHelper;
        public MainWindow()
        {
            InitializeComponent();
            this._engine = new Engine();
            this._loader = new Loader();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._loader.LoadLabels(this.refFileText, this.comparisonText);
            this._textHelper = new TextTypeHelper();
            //Default the file type is txt
            this._refFileFormat = FileFormat.TextFile;
            this._comparisonFileFormat = FileFormat.TextFile; 
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void openRefButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            var filer = this._textHelper.GetFileDialogFilter(this._refFileFormat);
            dialog.Filter = filer;
            
            dialog.ShowDialog();
            var filePath = dialog.FileName;
            if(filePath != null && filePath != string.Empty)
            {
                var wordsCount = this._engine.LoadReferentText(filePath, this._refFileFormat);
            }
            
        }
    }
}
