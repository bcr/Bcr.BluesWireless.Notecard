using Bcr.BluesWireless.Notecard.Core;
using PrettyPrompt;
using System.IO.Ports;
// using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

IEnumerable<string> GetPotentialSerialPortNames()
{
    return SerialPort.GetPortNames().Where((name) => Regex.IsMatch(name, "cu.*NOTE.*"));
}

using (var serialPort = new SerialPort(GetPotentialSerialPortNames().First()))
{
    serialPort.Open();
    var communicationChannel = new SerialPortCommunicationChannel(serialPort);
    var prompt = new Prompt(configuration: new PromptConfiguration(prompt: "> "));

    while (true)
    {
        var response = await prompt.ReadLineAsync();
        if (response.IsSuccess)
        {
            if (response.Text == "exit")
            {
                break;
            }
            communicationChannel.SendLine(response.Text);
            var receivedLine = communicationChannel.ReceiveLine();
            Console.WriteLine(receivedLine);
            // var json = JsonNode.Parse(receivedLine).AsObject();
        }
    }
}
