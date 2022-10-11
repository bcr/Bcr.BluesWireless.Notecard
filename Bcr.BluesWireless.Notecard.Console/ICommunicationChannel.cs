namespace Bcr.BluesWireless.Notecard.Console;

interface ICommunicationChannel
{
    /// <summary>
    /// Send a line. Do NOT include the trailing LF in your data.
    /// </summary>
    public void SendLine(string data);

    /// <summary>
    /// Receive a line. The trailing LF is NOT included.
    /// </summary>
    public string ReceiveLine();
}
