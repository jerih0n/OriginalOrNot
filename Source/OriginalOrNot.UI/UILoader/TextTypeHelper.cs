

namespace OriginalOrNot.UI.UILoader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OriginalOrNot.Shared;
    public class TextTypeHelper
    {
        private Dictionary<string, FileFormat> _supportedFileFormts = new Dictionary<string, FileFormat>()
        {
            { ".txt File", FileFormat.TextFile },
            { ".docx File", FileFormat.DocXFormat },
            { ".pdf File", FileFormat.PdfFormat }
        };
        public TextTypeHelper()
        {

        }
        public FileFormat GetFileFormat(string formatAsString)
        {
            return this._supportedFileFormts[formatAsString];
        }
        public string GetFileDialogFilter(FileFormat format)
        {
            switch(format)
            {
                case FileFormat.TextFile:
                    return "Text files (*.txt)|*.txt";

                case FileFormat.DocXFormat:
                    return "Word files (*.docx)|*.docx|(*.doc)|*.doc";
                case FileFormat.PdfFormat:
                    return "Pdf files(*.pdf)| *.pdf";
                default:
                    return "Error";
            }
        }
    }
    
}
