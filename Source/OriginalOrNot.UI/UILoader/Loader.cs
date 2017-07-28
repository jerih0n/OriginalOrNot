using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OriginalOrNot.UI.UILoader
{
    public class Loader
    {
        private const string _referentLabelText = "Please, select the file type and then load\nthe file, that will be used as\nreferent in the comparison process";
        private const string _comparisonLabelText = "Please, select the file type and then load\nthe file that will be compared\nto the referent file";
        private const string _comparisonExplanationText = "In order to compare, both files should be loaded. After comparison you will see the percents of\nequal words between the two files.If you perform intersection, a new file with all the equal words\nwill be created in selected folder. You can check for equal sentances, paragraphs\nor even entire section that way.";
        private const string _comparisonByDifferenceExplanation = "In order to compare, both files should be loaded. After comparison you will see the number of different words\n.This mode will automaticaly performe intersect and the new file, that will be created, will contains all the words,\nand the different words will be highlighted in red. It may take time, if you compare very different files";
        public void LoadLabels(Label referentLabe, Label comparisonLebel)
        {
            referentLabe.Content = _referentLabelText;
            comparisonLebel.Content = _comparisonLabelText;
        }
        public void LoadComparisonExplanationLaber(Label label)
        {
            label.Content = _comparisonExplanationText;
        }
        public void LoadComparisonExplanationForDifferenceModeLabel(Label label)
        {
            label.Content = _comparisonByDifferenceExplanation;
        }
    }
}
