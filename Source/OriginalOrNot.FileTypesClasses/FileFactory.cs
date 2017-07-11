

namespace OriginalOrNot.FileTypesClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OriginalOrNot.Shared;
    public static class FileFactory
    {
        private static Dictionary<FileFormat, ITextTypeFile> supportedFileTypes = new Dictionary<FileFormat, ITextTypeFile>()
        {
            { FileFormat.DocXFormat, new DocXFormatClass() },
            { FileFormat.TextFile, new TxtFormatClass()}
        };
        public static ITextTypeFile GetFileClass(FileFormat format)
        {
            return supportedFileTypes[format];
        }
    }
}
