using System.IO.Ports;

namespace Bcr.BluesWireless.Notecard.Console;

public class SerialPortCommunicationChannel : ICommunicationChannel
{
    private SerialPort _serialPort;

    public SerialPortCommunicationChannel(SerialPort serialPort)
    {
        _serialPort = serialPort;
    }

    public void SendLine(string data)
    {
        _serialPort.WriteLine(data);
    }

    public string ReceiveLine(string data)
    {
        return _serialPort.ReadLine();
    }
}
