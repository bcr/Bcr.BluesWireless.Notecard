namespace Bcr.BluesWireless.Notecard.Console;

public record class CardTimeResponse(int minutes, Decimal lat, Decimal lon, string area, string country, string zone, uint time);
