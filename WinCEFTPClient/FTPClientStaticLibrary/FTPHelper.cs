using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SmartFTPClient
{
    public class FTPHelper
    {
        protected NetworkStream nStream = null;
        protected StreamReader rdStrm = null;
        protected StreamWriter wrStrm = null;
        protected FileInfo[] filesToSend = null;
        protected FtpProgress ftpProg = null;
        protected FtpPassiveConnection storSendTo = null;
        protected Byte[] fileBytes = null;
        protected String ftpServerAddress = "ftp.yourserver.com";
        protected Int32 ftpServerPort = 21;
        protected String ftpCWDFolder = @"/"; // @"/home/";
        protected event Action<FtpProgress> FtpCurrentProgressEvent;

        /// <summary>
        /// Writes an FTO command to wrStrm object
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Verb"></param>
        /// <param name="Verb2"></param>
        protected void SendFTPCommand(String Command, String Verb, String Verb2)
        {
            var Data = String.Format("{0} {1} {2}", Command, Verb, Verb2).Trim();
            wrStrm.WriteLine(Data);
            wrStrm.Flush();
            // if the Command = STOR, the FTP server starts listening to a passive port
            // in that case you read the lin from the stream after the files is send to the
            // passive port.
            if (!Command.Equals("STOR", StringComparison.OrdinalIgnoreCase))
            {
                ReadLineFromStream();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CreatePassiveFtpSocketInformation()
        {
            // Most answers are in a pattern like
            // 227 blah blah blah (ip0,ip1,ip2,ip3,prt0,prt1)
            // So first find ( and than ) and in between you find the 
            // IP addres and the port which has to be calculated.
            // prt0 must be multiplied by 256 and prt1 must be added to the calculation to get the 'listeners port'

            /*
             Official documentation:
                Tells the server to enter "passive mode". 
                In passive mode, the server will wait for the client to establish a connection with it 
                rather than attempting to connect to a client-specified port. 
             
                The server will respond with the address of the port it is listening on, with a message like:
                227 Entering Passive Mode (a1,a2,a3,a4,p1,p2)
                where a1.a2.a3.a4 is the IP address and p1*256+p2 is the port number.
             */
            var startPart = ftpProg.FtpResponse.LastIndexOf("(") + 1;
            var endPart = ftpProg.FtpResponse.LastIndexOf(")");
            var passsiveResult = ftpProg.FtpResponse.Substring(startPart, endPart - startPart);
            var results = passsiveResult.Split(',');
            storSendTo = new FtpPassiveConnection();
            storSendTo.PassiveIP = IPAddress.Parse(String.Format("{0}.{1}.{2}.{3}", results[0], results[1], results[2], results[3]));
            storSendTo.PasssivePortNr = (Convert.ToInt32(results[4]) * 256) + Convert.ToInt32(results[5]);
        }

        protected void ReadLineFromStream()
        {
            bool MultipleLines = false;
            while (true)
            {
                string line = rdStrm.ReadLine();
                MultipleLines = line.Substring(3, 1).Equals("-");

                ftpProg.FtpResponse = line;
                StartCurrentProgressEvent();
                if (!MultipleLines) break;
            }
        }

        protected void ReadAllBytesFromTheFile(FileInfo file)
        {
            fileBytes = new Byte[file.Length];
            using (var fStream = new FileStream(file.FullName, FileMode.Open))
            {
                fStream.Read(fileBytes, 0, fileBytes.Length);
            }
        }

        protected void StartCurrentProgressEvent()
        {
            if (FtpCurrentProgressEvent != null)
                FtpCurrentProgressEvent(ftpProg);
        }

        protected bool ActiveSyncConnected
        {
            get
            {
                try
                {
                    var entry = Dns.GetHostEntry("PPP_PEER");
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
