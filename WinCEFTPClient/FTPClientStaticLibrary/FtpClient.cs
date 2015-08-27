
using System.Net.Sockets;
using System.IO;
using System;
using System.Net;
namespace SmartFTPClient
{
    public class FtpClient : FTPHelper, IFtpClient
    {
        #region declares
        private String ftpLocalFilePath = @"/";
        private String searchPattern = "*.*";
        #endregion declares

        #region IFtpClient Events
        public event Action<FtpProgress> FtpProgressEvent;
        public event Action<FileInfo[]> RetrieveFileListEvent;
        public event Action<String> SendFinishedEvent;
        #endregion IFtpClient Events

        #region Props
        public Boolean ActiveSyncConnectionEnabled
        {
            get { return ActiveSyncConnected; }
        }
        #endregion Props

        #region Methods

        public FtpClient()
        {
            FtpCurrentProgressEvent += new Action<FtpProgress>(FtpClient_FtpCurrentProgressEvent);
        }

        void FtpClient_FtpCurrentProgressEvent(FtpProgress ftpProgress)
        {
            if (FtpProgressEvent != null)
                FtpProgressEvent(ftpProgress);
        }

        public void GetLocalFiles()
        {
            filesToSend = IOHelpers.GetFileList(ftpLocalFilePath, searchPattern);
            if (RetrieveFileListEvent != null)
                RetrieveFileListEvent(filesToSend);
        }

        public void SendFiles()
        {
            try
            {
                ftpProg = new FtpProgress();
                ftpProg.CurrentFileNr = 0;
                ftpProg.LastFileNr = filesToSend.Length;
                StartCurrentProgressEvent();
                using (TcpClient tcpCl = new TcpClient(ftpServerAddress, ftpServerPort)) // Create connection
                {
                    using (nStream = tcpCl.GetStream()) // connect to FTP stream
                    {
                        rdStrm = new StreamReader(nStream);
                        wrStrm = new StreamWriter(nStream) { AutoFlush = true };

                        // after connecting and attaching writers and readers to the stream 
                        // read the first response from the FTP Server -> response 200 .....
                        ReadLineFromStream();  

                        // Send Username
                        SendFTPCommand("USER", "your username here", "");
                        // Send Password
                        SendFTPCommand("PASS", "your password here", "");
                        //Transfer Type 'Local format'
                        SendFTPCommand("TYPE", "L", "");
                        // structure command F = files
                        SendFTPCommand("STRU", "F", "");
                        // Mode S = Stream
                        SendFTPCommand("MODE", "S", "");
                        //CWD Change Working Directory/folder on FTP server
                        SendFTPCommand("CWD", ftpCWDFolder, ""); 

                        //Iterate the available files
                        foreach (FileInfo file in filesToSend)
                        {
                            ftpProg.SendFileBytes = 0;
                            // announce file upload , ask for passive port -> response 227......
                            SendFTPCommand("PASV", "", "");
                            CreatePassiveFtpSocketInformation();

                            // tell ftp to create a file and open a socket on passive address
                            SendFTPCommand("STOR", String.Format(@"{0}", file.Name), "");

                            // create an IPEndPoint object based on the received Passive IP and Passive Port nr
                            // these values where given via PASV command and setup in DeterminePassiveFTPPort() method
                            var endPoint = new IPEndPoint(storSendTo.PassiveIP, storSendTo.PasssivePortNr);
                            using (Socket sendFileSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                            {
                                try
                                {
                                    // Read all bytes in the file. These bytes are needed to send to the FTP Server
                                    ReadAllBytesFromTheFile(file);

                                    // Connect to the Passive Socket, send the file and close the port
                                    sendFileSocket.Connect(endPoint);
                                    sendFileSocket.Send(fileBytes, 0, fileBytes.Length, SocketFlags.DontRoute);
                                    sendFileSocket.Close();
                                }
                                catch (Exception err)
                                {
                                    ExceptionHandling.ShowMessage(err);
                                }
                            }
                            // Read the STOR response line -> 150 ..... binary....
                            // This can be done now because the file is physically send to the FTP server
                            ReadLineFromStream();
                            // Go to the next file in the filelist
                            ftpProg.CurrentFileNr++;
                            // Read socket response 
                            ReadLineFromStream();
                        }
                        // Say Goodbye to the FTP server
                        SendFTPCommand("QUIT", "", "");

                        // als er geen excepties zijn geweest, dan:
                        foreach (FileInfo file in filesToSend)
                        {
                            file.Delete();
                        }
                        // Tell th client all files are send
                        if (SendFinishedEvent != null)
                            SendFinishedEvent(String.Format("Finished sending {0} files ", filesToSend.Length));

                    }
                }
            }
            catch (InvalidCastException err)
            {
                ExceptionHandling.ShowMessage(err);
            }
            catch (IOException err)
            {
                ExceptionHandling.ShowMessage(err);
            }
            catch (NotSupportedException err)
            {
                ExceptionHandling.ShowMessage(err);
            }
            catch (ArgumentException err)
            {
                ExceptionHandling.ShowMessage(err);
            }
            catch (ProtocolViolationException err)
            {
                ExceptionHandling.ShowMessage(err);
            }
            catch (WebException err)
            {
                ExceptionHandling.ShowMessage(err);
            }
            finally
            {
                rdStrm.Close();
                rdStrm = null;
                wrStrm = null;
                
            }
        }

        #endregion Methods
    }
}
