using System;

namespace Geography.Point
{
    public class GlobePoint : IGlobePoint
    {
        private const double epsilon = 10E-5;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public GlobePoint() { }
        public GlobePoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public bool IsValid()
        {
            return Math.Abs(Latitude) >= 0 && Math.Abs(Latitude) <= 90 &&
                Math.Abs(Longitude) >= 0 && Math.Abs(Longitude) <= 180;
        }

        public override bool Equals(Object other)
        {
            if (other is not IGlobePoint)
            {
                return false;
            }

            return Equals((IGlobePoint)other);
        }

        public bool Equals(IGlobePoint other)
        {
            if (this == other)
            {
                return true;
            }

            return (Math.Abs(Latitude - other.Latitude) < epsilon) && (Math.Abs(Longitude - other.Longitude) < epsilon);
        }
    }
}
