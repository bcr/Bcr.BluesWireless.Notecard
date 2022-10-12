namespace Bcr.BluesWireless.Notecard.Console;

public record CardTimeResponse(int Minutes, Decimal Lat, Decimal Lon, string Area, string Country, string Zone, long Time);
