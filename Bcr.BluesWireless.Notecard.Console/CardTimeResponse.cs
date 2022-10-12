namespace Bcr.BluesWireless.Notecard.Console;

public record class CardTimeResponse(int Minutes, Decimal Lat, Decimal Lon, string Area, string Country, string Zone, uint Time);
