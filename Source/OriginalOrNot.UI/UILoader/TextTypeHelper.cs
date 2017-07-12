

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
        private Dictionary<string, FileFormat> supportedFileFormats = new Dictionary<string, FileFormat>()
        {
            { "TxtFile", FileFormat.TextFile },
            { "TxtFile", FileFormat.TextFile }
        };
        public FileFormat GetFileFormat(string selected)
        {
            return supportedFileFormats[selected];
        }
    }
    
}
