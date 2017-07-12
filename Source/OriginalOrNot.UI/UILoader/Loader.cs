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
        public void LoadLabels(Label referentLabe, Label comparisonLebel)
        {
            referentLabe.Content = _referentLabelText;
            comparisonLebel.Content = _comparisonLabelText;
        }
    }
}
