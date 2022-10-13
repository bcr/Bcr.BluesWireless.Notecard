using PrettyPrompt.Consoles;
using PrettyPrompt.Highlighting;
using System.Text;
using System.Text.Json;

namespace Bcr.BluesWireless.Notecard.Console;

public class JsonHelper
{
    static Dictionary<JsonTokenType, AnsiColor> _styleDictionary = new()  {
        { JsonTokenType.String, AnsiColor.BrightGreen },
        { JsonTokenType.Number, AnsiColor.Yellow },
    };

    public static void OutputFormattedJson(IConsole console, string json)
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
}
