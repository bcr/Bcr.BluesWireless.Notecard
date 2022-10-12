using System.Text.Json;

namespace Bcr.BluesWireless.Notecard.Console;

public class InitialCapNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name[0].ToString().ToLower() + name.Substring(1);
    }
}
