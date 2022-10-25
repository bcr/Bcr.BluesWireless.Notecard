using System.IO.Ports;
using System.Text.RegularExpressions;

namespace Bcr.BluesWireless.Notecard.Core;

public class SerialPortCommunicationChannel : CommunicationChannel
{
    private SerialPort _serialPort;
    private bool disposedValue;

    public SerialPortCommunicationChannel(SerialPort serialPort)
    {
        _serialPort = serialPort;
    }

    protected override void SendLine(string data) => _serialPort.WriteLine(data);

    protected override string ReceiveLine() =>  _serialPort.ReadLine();

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

    public override void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public override string ToString() => _serialPort.PortName;
}

public class DefaultSerialPortCommunicationChannel : SerialPortCommunicationChannel
{
    static SerialPort GetDefaultOpenedSerialPort()
    {
        string portName;
        try
        {
            portName = GetPotentialSerialPortNames().First();
        }
        catch (InvalidOperationException e)
        {
            throw new Exception("Unable to find matching serial port name", e);
        }
        var serialPort = new SerialPort(portName);
        serialPort.Open();
        return serialPort;
    }

    static IEnumerable<string> GetPotentialSerialPortNames() => SerialPort.GetPortNames().Where((name) => Regex.IsMatch(name, "cu.*NOTE.*"));

    public DefaultSerialPortCommunicationChannel() : base(GetDefaultOpenedSerialPort()) {}
}
