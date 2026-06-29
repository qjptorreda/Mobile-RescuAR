using System;

public static class NavigationUtility
{
    private const double EarthRadius = 6371000.0;

    public static double CalculateDistanceMeters(
        double lat1,
        double lon1,
        double lat2,
        double lon2)
    {
        double latRad1 = DegreesToRadians(lat1);
        double latRad2 = DegreesToRadians(lat2);

        double dLat = DegreesToRadians(lat2 - lat1);
        double dLon = DegreesToRadians(lon2 - lon1);

        double a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(latRad1) * Math.Cos(latRad2) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c =
            2 * Math.Atan2(
                Math.Sqrt(a),
                Math.Sqrt(1 - a));

        return EarthRadius * c;
    }

    public static double CalculateBearing(
        double lat1,
        double lon1,
        double lat2,
        double lon2)
    {
        double latRad1 = DegreesToRadians(lat1);
        double latRad2 = DegreesToRadians(lat2);

        double dLon =
            DegreesToRadians(
                lon2 - lon1);

        double y =
            Math.Sin(dLon) *
            Math.Cos(latRad2);

        double x =
            Math.Cos(latRad1) *
            Math.Sin(latRad2) -
            Math.Sin(latRad1) *
            Math.Cos(latRad2) *
            Math.Cos(dLon);

        double bearing =
            RadiansToDegrees(
                Math.Atan2(y, x));

        return (bearing + 360) % 360;
    }

    private static double DegreesToRadians(
        double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static double RadiansToDegrees(
        double radians)
    {
        return radians * 180.0 / Math.PI;
    }
}
