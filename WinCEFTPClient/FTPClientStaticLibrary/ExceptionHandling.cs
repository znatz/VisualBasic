using System;
using System.Windows.Forms;

namespace SmartFTPClient
{
    public class ExceptionHandling
    {
        public static void ShowMessage(Exception err)
        {
            MessageBox.Show(err.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }
    }
}
