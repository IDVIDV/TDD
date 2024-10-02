using Geography.Point;
using System;

namespace Geography
{
    public class GeographyCalculator
    {
        public const double EarthRadiusM = 6371E3;
        public const double DegreeToRadianConversion = Math.PI / 180;
        public const double RadianToDegreeConversion = 180 / Math.PI;

        public static double DegreeToRadian(double degree)
        {
            return degree * DegreeToRadianConversion;
        }

        public static double RadianToDegree(double radian)
        {
            return radian * RadianToDegreeConversion;
        }

        public static double CalcDistance(IGlobePoint point1, IGlobePoint point2)
        {
            ValidatePoints(new IGlobePoint[] { point1, point2 });

            double lat1Rad = DegreeToRadian(point1.Latitude);
            double lat2Rad = DegreeToRadian(point2.Latitude);
            double halfDeltaLatRad = DegreeToRadian((point2.Latitude - point1.Latitude)) / 2;
            double halfDeltaLonRad = DegreeToRadian((point2.Longitude - point1.Longitude)) / 2;

            double tmp = Math.Sin(halfDeltaLatRad) * Math.Sin(halfDeltaLatRad) +
                      Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                      Math.Sin(halfDeltaLonRad) * Math.Sin(halfDeltaLonRad);
            return EarthRadiusM * 2 * Math.Atan2(Math.Sqrt(tmp), Math.Sqrt(1 - tmp));
        }

        public static double CalcAzimuthDegree(IGlobePoint point1, IGlobePoint point2)
        {
            ValidatePoints(new IGlobePoint[] { point1, point2 });

            double lat1Rad = DegreeToRadian(point1.Latitude);
            double lat2Rad = DegreeToRadian(point2.Latitude);
            double lon1Rad = DegreeToRadian(point1.Longitude);
            double lon2Rad = DegreeToRadian(point2.Longitude);

            double y = Math.Sin(lon2Rad - lon1Rad) * Math.Cos(lat2Rad);
            double x = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) -
                      Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(lon2Rad - lon1Rad);
            double preResult = Math.Atan2(y, x);
            return (RadianToDegree(preResult) + 360) % 360;
        }

        public static IGlobePoint CalcMidpoint(IGlobePoint point1, IGlobePoint point2)
        {
            ValidatePoints(new IGlobePoint[] { point1, point2 });

            double lat1Rad = DegreeToRadian(point1.Latitude);
            double lat2Rad = DegreeToRadian(point2.Latitude);
            double lon1Rad = DegreeToRadian(point1.Longitude);
            double lon2Rad = DegreeToRadian(point2.Longitude);

            double Bx = Math.Cos(lat2Rad) * Math.Cos(lon2Rad - lon1Rad);
            double By = Math.Cos(lat2Rad) * Math.Sin(lon2Rad - lon1Rad);
            double latitude = Math.Atan2(Math.Sin(lat1Rad) + Math.Sin(lat2Rad),
                                  Math.Sqrt((Math.Cos(lat1Rad) + Bx) * (Math.Cos(lat1Rad) + Bx) + By * By));
            double longitude = lon1Rad + Math.Atan2(By, Math.Cos(lat1Rad) + Bx);
            longitude = (RadianToDegree(longitude) + 540) % 360 - 180;
            latitude = RadianToDegree(latitude);

            return new GlobePoint(latitude, longitude);
        }

        public static IGlobePoint CalcDestination(IGlobePoint start, double azimuthD, double distanceM)
        {

            if (distanceM < 0)
            {
                throw new ArgumentException($"Invalid distance: {distanceM}");
            }

            if (azimuthD < 0 || azimuthD > 360)
            {
                throw new ArgumentException($"Invalid azimuth: {azimuthD}");
            }

            ValidatePoints(new IGlobePoint[] { start });

            double distanceToEarthRadiusRatio = distanceM / EarthRadiusM;
            double latRad = DegreeToRadian(start.Latitude);
            double lonRad = DegreeToRadian(start.Longitude);
            double azimuthRad = DegreeToRadian(azimuthD);


            double destLatitude = Math.Asin(Math.Sin(latRad) * Math.Cos(distanceToEarthRadiusRatio) +
                      Math.Cos(latRad) * Math.Sin(distanceToEarthRadiusRatio) * Math.Cos(azimuthRad));
            double destLongitude = lonRad + Math.Atan2(Math.Sin(azimuthRad) * Math.Sin(distanceToEarthRadiusRatio) * Math.Cos(latRad),
                                       Math.Cos(distanceToEarthRadiusRatio) - Math.Sin(latRad) * Math.Sin(destLatitude));

            destLatitude = RadianToDegree(destLatitude);
            destLongitude = (RadianToDegree(destLongitude) + 540) % 360 - 180;

            return new GlobePoint(destLatitude, destLongitude);
        }

        private static void ValidatePoints(IGlobePoint[] points)
        {
            string errorMsg = "";

            foreach (IGlobePoint point in points)
            {
                if (!point.IsValid())
                {
                    errorMsg += $"Globe point: ({point.Latitude}, {point.Longitude}) is invalid; ";
                }
            }

            if (errorMsg.Length != 0)
            {
                throw new ArgumentException(errorMsg);
            }

        }
    }
}