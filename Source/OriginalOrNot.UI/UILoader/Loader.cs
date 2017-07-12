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
        private const string _referentLabelText = "Please, select the file type and then load the file,\nthat will be used as referent in the\ncomparison process";
        private const string _comparisonLabelText = "Please, select the file type and then load the file\nthat will be compared to the\nreferent file"; 
        public void LoadLabels(Label referentLabe, Label comparisonLebel)
        {
            referentLabe.Content = _referentLabelText;
            comparisonLebel.Content = _referentLabelText;
        }
    }
}
