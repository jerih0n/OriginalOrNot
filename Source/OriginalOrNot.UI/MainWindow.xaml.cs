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

namespace OriginalOrNot.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private Engine _engine;
        private Loader _loader;
        private FileFormat _refFileFormat;
        private FileFormat _comparisonFileFormat;
        public MainWindow()
        {
            InitializeComponent();
            this._engine = new Engine();
            this._loader = new Loader();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._loader.LoadLabels(this.refFileText, this.comparisonText);
            //Default the file type is txt
            this._refFileFormat = FileFormat.TextFile;
            this._comparisonFileFormat = FileFormat.TextFile; 
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
