using System.IO.Ports;
using System.Text.RegularExpressions;

IEnumerable<string> GetPotentialSerialPortNames()
{
    return SerialPort.GetPortNames().Where((name) => Regex.IsMatch(name, "cu.*NOTE.*"));
}

using (var serialPort = new SerialPort(GetPotentialSerialPortNames().First()))
{
    serialPort.Open();
    serialPort.WriteLine("{\"req\":\"card.time\"}");
    Console.WriteLine(serialPort.ReadLine());
}
