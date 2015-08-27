using System;
using System.IO;

namespace SmartFTPClient
{
    public interface IFtpClient
    {
        #region events
        event Action<FtpProgress> FtpProgressEvent;
        event Action<FileInfo[]> RetrieveFileListEvent;
        event Action<String> SendFinishedEvent;
        #endregion events

        #region Props
        Boolean ActiveSyncConnectionEnabled { get; }
        #endregion Props


        #region Methods
        void GetLocalFiles();
        void SendFiles();
        #endregion Methods
    }
}
