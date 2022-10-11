using Bcr.BluesWireless.Notecard.Core;

using System.IO.Ports;
using System.Text.RegularExpressions;

IEnumerable<string> GetPotentialSerialPortNames()
{
    return SerialPort.GetPortNames().Where((name) => Regex.IsMatch(name, "cu.*NOTE.*"));
}

using (var serialPort = new SerialPort(GetPotentialSerialPortNames().First()))
{
    serialPort.Open();
    var communicationChannel = new SerialPortCommunicationChannel(serialPort);

    while (true)
    {
        Console.Write("> ");
        var command = Console.ReadLine();
        if (command == null)
        {
            break;
        }
        communicationChannel.SendLine(command);
        Console.WriteLine(communicationChannel.ReceiveLine());
    }
}
