using System.IO.Ports;

namespace Bcr.BluesWireless.Notecard.Core;

public class SerialPortCommunicationChannel : ICommunicationChannel
{
    private SerialPort _serialPort;
    private bool disposedValue;

    public SerialPortCommunicationChannel(SerialPort serialPort)
    {
        _serialPort = serialPort;
    }

    public void SendLine(string data)
    {
        _serialPort.WriteLine(data);
    }

    public string ReceiveLine()
    {
        return _serialPort.ReadLine();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                _serialPort.Dispose();
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
