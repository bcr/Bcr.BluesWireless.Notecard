namespace Bcr.BluesWireless.Notecard.Core;

public interface ICommunicationChannel
{
    /// <summary>
    /// Send a line. Do NOT include the trailing LF in your data.
    /// </summary>
    void SendLine(string data);

    /// <summary>
    /// Receive a line. The trailing LF is NOT included.
    /// </summary>
    string ReceiveLine();
}
