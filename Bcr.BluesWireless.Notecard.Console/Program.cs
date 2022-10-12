using Bcr.BluesWireless.Notecard.Console;
using Bcr.BluesWireless.Notecard.Core;
using PrettyPrompt;
using System.IO.Ports;
using System.Text.Json;
using System.Text.RegularExpressions;

IEnumerable<string> GetPotentialSerialPortNames()
{
    return SerialPort.GetPortNames().Where((name) => Regex.IsMatch(name, "cu.*NOTE.*"));
}

DateTimeOffset MakeDateTimeOffset(long unixTime, int offsetMinutes)
{
    var offset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
    offset = offset.AddMinutes(offsetMinutes);
    offset = new DateTimeOffset(offset.DateTime, TimeSpan.FromMinutes(offsetMinutes));

    return offset;
}

using (var serialPort = new SerialPort(GetPotentialSerialPortNames().First()))
{
    serialPort.Open();
    var communicationChannel = new SerialPortCommunicationChannel(serialPort);
    var prompt = new Prompt(configuration: new PromptConfiguration(prompt: "> "));

    while (true)
    {
        var response = await prompt.ReadLineAsync();
        if ((!response.IsSuccess) || (response.Text == "exit"))
        {
            break;
        }

        communicationChannel.SendLine(response.Text);
        var receivedLine = communicationChannel.ReceiveLine();
        Console.WriteLine(receivedLine);
        if (response.Text.Contains("card.time"))
        {
            var json = JsonSerializer.Deserialize<CardTimeResponse>(
                receivedLine,
                new JsonSerializerOptions {
                    PropertyNamingPolicy = new InitialCapNamingPolicy()
                    }
                );
            Console.WriteLine(json);
            var offset = MakeDateTimeOffset(json!.Time, json.Minutes);
            Console.WriteLine(offset);
        }
    }
}
