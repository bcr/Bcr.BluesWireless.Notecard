using Bcr.BluesWireless.Notecard.Console;

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

    communicationChannel.SendLine("{\"req\":\"card.time\"}");
    Console.WriteLine(communicationChannel.ReceiveLine());
}
