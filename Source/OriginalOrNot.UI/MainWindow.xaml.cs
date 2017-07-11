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
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var pat1 = @"C:\Users\DB\Desktop\test.txt";
            var pat2 = @"C:\Users\DB\Desktop\test.docx";
            var pat3 = @"C:\Users\DB\Desktop";
            Stopwatch wathc = new Stopwatch();
            wathc.Start();
            var result1 = this._engine.LoadReferentText(pat1, FileFormat.TextFile);
            var result2 = this._engine.LoadComparisonText(pat2, FileFormat.DocXFormat);
            var result3 = this._engine.CompareAndIntersectTheTwoTexts(Shared.Language.English, pat3);
            wathc.Stop();
            MessageBox.Show($"Time : {wathc.Elapsed}");
            

        }
    }
}
