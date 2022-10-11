using System.IO.Ports;

namespace Bcr.BluesWireless.Notecard.Core;

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

    public string ReceiveLine()
    {
        return _serialPort.ReadLine();
    }
}
