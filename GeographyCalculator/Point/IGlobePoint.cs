using System;

namespace Geography.Point
{
    public interface IGlobePoint : IEquatable<IGlobePoint>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsValid();
    }
}
