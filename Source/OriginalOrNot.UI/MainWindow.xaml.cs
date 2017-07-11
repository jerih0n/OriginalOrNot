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
namespace OriginalOrNot.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Engine _engine;
        public MainWindow()
        {
            InitializeComponent();
            this._engine = new Engine();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string refPath = @"C:\Users\DB\Desktop\test.txt";
            string comparPath = @"C:\Users\DB\Desktop\test.docx";
            string path = "C:\\Users\\DB\\Desktop";
            int result = this._engine.LoadReferentText(refPath, FileFormat.TextFile);
            int result2 = this._engine.LoadComparisonText(comparPath, FileFormat.DocXFormat);
            double result3 = this._engine.CompareAndIntersectTheTwoTexts(Shared.Language.English, path);

        }
    }
}
