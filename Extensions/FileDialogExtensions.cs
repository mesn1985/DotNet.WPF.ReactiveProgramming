using System;
using System.Windows.Forms;

namespace LAB12.Extensions
{
    public static class FileDialogExtensions
    {
        public static string Open(this OpenFileDialog fileDialog)
        {
            var dialogResult = fileDialog.ShowDialog();
            return (dialogResult == DialogResult.OK) ? fileDialog.FileName : "";
        }
        public static Uri AsUri(this string filePath)
            => string.IsNullOrEmpty(filePath) ? null :new Uri(filePath);

    }
}
