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
        private bool _isReferentFileLoaded;
        private bool _isComparisonFileLoaded;
        private const string _loaded = "Loaded";
        private const string _notLoaded = "Not Loaded";
        private bool _shouldPerformIntersect;
        private ComparisonMode _mode;
        public MainWindow()
        {
            InitializeComponent();
            this._engine = new Engine();
            this._loader = new Loader();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._loader.LoadLabels(this.refFileText, this.comparisonText);
            this._loader.LoadComparisonExplanationLaber(this.coparisonLabel);
            this._textHelper = new TextTypeHelper();
            //Default the file type is txt
            this._refFileFormat = FileFormat.TextFile;
            this._comparisonFileFormat = FileFormat.TextFile;
            this._isComparisonFileLoaded = false;
            this._isReferentFileLoaded = false;
            this.refTextLoadedStatus.Content = _notLoaded;
            this.comTextLoadedStatus.Content = _notLoaded;
            this._shouldPerformIntersect = false;
            this._mode = Shared.ComparisonMode.EqualWords;
            this.ComparisonMode.SelectedIndex = 1;
        }
        private void refFilesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combboBox = sender as ComboBox;
            var selectedIted = combboBox.SelectedValue as ComboBoxItem;
            var value = selectedIted.Content;
            if(value != null)
            {
                this._refFileFormat = this._textHelper.GetFileFormat(value.ToString());
            }
            
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
                //after loading making the UI changes
                this._isReferentFileLoaded = true;
                //make red Invisible
                this.refTextImageX.Visibility = Visibility.Hidden;
                this.refTextImageV.Visibility = Visibility.Visible;
                this.refTextLoadedStatus.Content = _loaded;
                this.refTextWordsCount.Content = $"Total words: {wordsCount}";
                var filePathComponents = filePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                this.refFileName.Content = filePathComponents[filePathComponents.Length - 1];
            }
            if(this._isComparisonFileLoaded)
            {
                // both files are loaded, unlock the button
                this.compareButton.IsEnabled = true;
            }
        }
        private void refTextUnload_Click(object sender, RoutedEventArgs e)
        {
            this._engine.UnloadReferentText();
            this.refTextLoadedStatus.Content = _notLoaded;
            this._isReferentFileLoaded = false;
            this.refTextImageV.Visibility = Visibility.Hidden;
            this.refTextImageX.Visibility = Visibility.Visible;
            this.refTextWordsCount.Content = "";
            this.compareButton.IsEnabled = false;
            this.timeElapsed.Content = "";
            this.percents.Content = "";
        }

        private void comFilesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combboBox = sender as ComboBox;
            var selectedIted = combboBox.SelectedValue as ComboBoxItem;
            var value = selectedIted.Content;
            if (value != null)
            {
                this._comparisonFileFormat = this._textHelper.GetFileFormat(value.ToString());
            }
        }

        private void openComButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            var filer = this._textHelper.GetFileDialogFilter(this._comparisonFileFormat);
            dialog.Filter = filer;
            dialog.ShowDialog();
            var filePath = dialog.FileName;
            if (filePath != null && filePath != string.Empty)
            {
                var wordsCount = this._engine.LoadComparisonText(filePath, this._comparisonFileFormat);
                //after loading making the UI changes
                this._isComparisonFileLoaded = true;
                //make red Invisible
                this.comTextImageX.Visibility = Visibility.Hidden;
                this.comTextImageV.Visibility = Visibility.Visible;
                this.comTextLoadedStatus.Content = _loaded;
                this.comTextWordsCount.Content = $"Total words: {wordsCount}";
                var filePathComponents = filePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                this.comFileName.Content = filePathComponents[filePathComponents.Length - 1];
            }
            if(this._isReferentFileLoaded)
            {
                // both files are loaded, unlock the button
                this.compareButton.IsEnabled = true;
            }
        }

        private void comTextUnload_Click(object sender, RoutedEventArgs e)
        {
            this._engine.UnloadComparisonText();
            this.comTextLoadedStatus.Content = _notLoaded;
            this._isComparisonFileLoaded = false;
            this.comTextImageV.Visibility = Visibility.Hidden;
            this.comTextImageX.Visibility = Visibility.Visible;
            this.comTextWordsCount.Content = "";
            this.compareButton.IsEnabled = false;
            this.timeElapsed.Content = "";
            this.percents.Content = "";
        }

        private void intersectionOptionCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            this._shouldPerformIntersect = true;
        }

        private void intersectionOptionCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            this._shouldPerformIntersect = false;
        }

        private void compareButton_Click(object sender, RoutedEventArgs e)
        {
            double resultPercents = 0;
            Stopwatch watch = new Stopwatch();
            string elapsedSeconds = string.Empty;
            if (this._shouldPerformIntersect)
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    dialog.ShowDialog();
                    
                    var path = dialog.SelectedPath;
                    if(path != null && path != "")
                    {
                        //watch.Start();
                        //resultPercents = this._engine.CompareAndIntersectTheTwoTexts(Shared.Language.English, path);
                        //watch.Stop();
                        //elapsedSeconds = watch.Elapsed.Milliseconds.ToString();

                        //just test !
                        watch.Start();
                        resultPercents = this._engine.FindTheDifferencesBetweenTheTwoFiles(Shared.Language.English, path);
                        watch.Stop();
                        elapsedSeconds = watch.Elapsed.Milliseconds.ToString();

                    }
                    else
                    {
                        watch.Start();
                        resultPercents = this._engine.CompareFiles(Shared.Language.English);
                        watch.Stop();
                        elapsedSeconds = watch.Elapsed.Milliseconds.ToString();
                        elapsedSeconds = watch.Elapsed.Milliseconds.ToString();
                    }

                }

            }
            else
            {
                watch.Start();
                resultPercents = this._engine.CompareFiles(Shared.Language.English);
                watch.Stop();
                elapsedSeconds = watch.Elapsed.Milliseconds.ToString();
            }
            if (resultPercents <= 20)
            {
                this.percents.Foreground = Brushes.Green;
            }
            else if (resultPercents >= 20 && resultPercents <= 50)
            {
                this.percents.Foreground = Brushes.Orange;
            }
            else
            {
                this.percents.Foreground = Brushes.Red;
            }
            this.percents.Content = string.Format("{0:0.00} %", resultPercents);
            this.timeElapsed.Content = $"Took {elapsedSeconds} milliseconds";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Hyperlink_RequestNavigate_1(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void ComparisonMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var boxSender = sender as ComboBox;
            var selectedValue = boxSender.SelectedValue as ComboBoxItem;
            if(selectedValue.Content.ToString() == "Equal Words")
            {
                this._mode = Shared.ComparisonMode.EqualWords;
                this._loader.LoadComparisonExplanationLaber(this.coparisonLabel);
                this.intersectionOptionCheckbox.IsEnabled = true;
            }
            else
            {
                this._mode = Shared.ComparisonMode.DifferentWords;
                // need to change the label of dicription 
                this._loader.LoadComparisonExplanationForDifferenceModeLabel(this.coparisonLabel);
                this.intersectionOptionCheckbox.IsEnabled = false;

            }
        }
    }
}
