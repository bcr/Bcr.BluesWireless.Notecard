using Bcr.BluesWireless.Notecard.Console;
using Bcr.BluesWireless.Notecard.Core;
using PrettyPrompt;
using PrettyPrompt.Consoles;
using PrettyPrompt.Highlighting;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

internal class Program
{
    static Dictionary<JsonTokenType, AnsiColor> _styleDictionary = new()  {
        { JsonTokenType.String, AnsiColor.BrightGreen },
        { JsonTokenType.Number, AnsiColor.Yellow },
    };

    static IEnumerable<string> GetPotentialSerialPortNames()
    {
        return SerialPort.GetPortNames().Where((name) => Regex.IsMatch(name, "cu.*NOTE.*"));
    }

    static DateTimeOffset MakeDateTimeOffset(long unixTime, int offsetMinutes)
    {
        var offset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        offset = offset.AddMinutes(offsetMinutes);
        offset = new DateTimeOffset(offset.DateTime, TimeSpan.FromMinutes(offsetMinutes));

        return offset;
    }

    static string GetHistoryPath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".bluehistory");
    }

    static void OutputFormattedJson(IConsole console, string json)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        var formatting = new List<FormatSpan>();

        while (reader.Read())
        {
            int start = (int) reader.TokenStartIndex;
            int length = reader.HasValueSequence ? (int) reader.ValueSequence.Length : reader.ValueSpan.Length;

            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                case JsonTokenType.String:
                    length += 2;
                    break;
            }

            if (_styleDictionary.ContainsKey(reader.TokenType))
            {
                formatting.Add(new FormatSpan(start, length, _styleDictionary[reader.TokenType]));
            }
        }

        console.WriteLine(new FormattedString(json, formatting));
    }

    private static async Task Main(string[] args)
    {
        using (var serialPort = new SerialPort(GetPotentialSerialPortNames().First()))
        {
            serialPort.Open();
            Console.WriteLine($"Connected to {serialPort.PortName}");
            var communicationChannel = new SerialPortCommunicationChannel(serialPort);
            var historyPath = GetHistoryPath();
            var console = new SystemConsole();
            var prompt = new Prompt(persistentHistoryFilepath: historyPath, configuration: new PromptConfiguration(prompt: "> "), console: console);
            Console.WriteLine($"History saved at {historyPath}");

            while (true)
            {
                var response = await prompt.ReadLineAsync();
                if ((!response.IsSuccess) || (response.Text == "exit"))
                {
                    break;
                }

                communicationChannel.SendLine(response.Text);
                var receivedLine = communicationChannel.ReceiveLine();
                OutputFormattedJson(console, receivedLine);
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
    }
}