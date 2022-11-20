namespace LocationUpdates.Models;

public record LocationModel(double Latitude, double Longitude, double Bearing)
{
    public double Latitude { get; } = Latitude;
    public double Longitude { get; } = Longitude;
    public double Bearing { get; } = Bearing;
}