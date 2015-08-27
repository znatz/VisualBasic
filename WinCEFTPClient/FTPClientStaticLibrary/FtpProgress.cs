using System;

namespace SmartFTPClient
{
    public class FtpProgress
    {
        public Int32 CurrentFileNr; // after sending each file Iterate +1. The selectedIndex of the ListBox will reflect the current file in sending
        public Int32 LastFileNr;   // needed for the Maximum property for the ProgressBar
        public String FileName;    // name of the current file name
        public Int64 FileBytes;    // the number of bytes to be send. When streaming in parts a file progress can be shown (2nd progressbar needed)
        public Int64 SendFileBytes; // number of bytes send
        public String FtpResponse;  // current FtpReponse
    }
}
