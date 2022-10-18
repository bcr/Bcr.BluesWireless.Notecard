namespace Bcr.BluesWireless.Notecard.Core;

public abstract class CommunicationChannel : IDisposable
{
    /// <summary>
    /// Send a line.
    /// </summary>
    /// <param name='data'>The line to send.</param>
    /// <remarks>Do NOT include the trailing LF.</remarks>
    protected abstract void SendLine(string data);

    /// <summary>
    /// Receive a line.
    /// </summary>
    /// <returns>
    /// The next LF-delimited line.
    /// </returns>
    /// <remarks>The delimiting LF is NOT included.</remarks>
    protected abstract string ReceiveLine();

    /// <summary>
    /// Perform a transaction.
    /// </summary>
    /// <param name='data'>The line to send.</param>
    /// <returns>
    /// The next LF-delimited line.
    /// </returns>
    /// <remarks>Do NOT include the trailing LF.</remarks>
    public string Transaction(string data)
    {
        SendLine(data);
        return ReceiveLine();
    }

    public abstract void Dispose();
}
