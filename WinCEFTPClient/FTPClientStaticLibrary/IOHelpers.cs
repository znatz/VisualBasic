using System;
using System.IO;
using System.Windows.Forms;

namespace SmartFTPClient
{
    public class IOHelpers
    {
        public static FileInfo[] GetFileList(String Path, String SearchPattern)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Path);
                return dirInfo.GetFiles(SearchPattern);
            }
            catch (IOException err)
            {
                ExceptionHandling.ShowMessage(err);
            }
            return null;
        }
    }
}
