using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Geography;
using Geography.Point;

namespace UnitTests
{
    [TestClass]
    public class GeographyCalculatorUnitTests
    {
        private const double Epsilon = 10E-7;
        private const double DistanceEpsilon = 1000;
        private const double AzimuthEpsilon = 1;

        //RadianToDegree Tests

        [TestMethod]
        public void RadianToDegree_PositiveRadian_ReturnsPositiveDegree()
        {
            double radians = Math.PI;
            double expectedDegrees = 180;

            double actualDegrees = GeographyCalculator.RadianToDegree(radians);

            Assert.IsTrue(Math.Abs(expectedDegrees - actualDegrees) < Epsilon);
        }

        [TestMethod]
        public void RadianToDegree_ZeroRadian_ReturnsZeroDegree()
        {
            double radians = 0;
            double expectedDegrees = 0;

            double actualDegrees = GeographyCalculator.RadianToDegree(radians);

            Assert.AreEqual(expectedDegrees, actualDegrees);
        }

        [TestMethod]
        public void RadianToDegree_NegativeRadian_ReturnsNegativeDegree()
        {
            double radians = -Math.PI;
            double expectedDegrees = -180;

            double actualDegrees = GeographyCalculator.RadianToDegree(radians);

            Assert.IsTrue(Math.Abs(expectedDegrees - actualDegrees) < Epsilon);
        }

        //DegreeToRadian Tests

        [TestMethod]
        public void DegreeToRadian_PositiveDegree_ReturnsPositiveRadian()
        {
            double degrees = 180;
            double expectedRadians = Math.PI;

            double actualRadians = GeographyCalculator.DegreeToRadian(degrees);

            Assert.IsTrue(Math.Abs(expectedRadians - actualRadians) < Epsilon);
        }

        [TestMethod]
        public void DegreeToRadian_ZeroDegree_ReturnsZeroRadian()
        {
            double degrees = 0;
            double expectedRadians = 0;

            double actualRadians = GeographyCalculator.DegreeToRadian(degrees);

            Assert.AreEqual(expectedRadians, actualRadians);
        }

        [TestMethod]
        public void DegreeToRadian_NegativeDegree_ReturnsNegativeRadian()
        {
            double degrees = -180;
            double expectedRadians = -Math.PI;

            double actualRadians = GeographyCalculator.DegreeToRadian(degrees);

            Assert.IsTrue(Math.Abs(expectedRadians - actualRadians) < Epsilon);
        }

        //CalcDistance Tests


        [TestMethod]
        public void CalcDistance_ValidPoints_ReturnsValidDistance()
        {
            IGlobePoint point1 = new GlobePoint(0, -60);
            IGlobePoint point2 = new GlobePoint(-60, 10);
            double expectedDistance = 8912230;

            double actualDistance = GeographyCalculator.CalcDistance(point1, point2);

            Assert.IsTrue(Math.Abs(expectedDistance - actualDistance) < DistanceEpsilon);
        }

        [TestMethod]
        public void CalcDistance_EqualPoints_ReturnsZero()
        {
            IGlobePoint point1 = new GlobePoint(30, -60);
            IGlobePoint point2 = new GlobePoint(30, -60);
            double expectedDistance = 0;

            double actualDistance = GeographyCalculator.CalcDistance(point1, point2);

            Assert.AreEqual(actualDistance, expectedDistance);
        }

        [TestMethod]
        public void CalcDistance_InvalidPoints_ThrowsArgumentException()
        {
            IGlobePoint point1 = new GlobePoint(100, -10);
            IGlobePoint point2 = new GlobePoint(30, -60);

            Assert.ThrowsException<ArgumentException>(() => GeographyCalculator.CalcDistance(point1, point2));
        }

        //CalcAzimuth Tests

        [TestMethod]
        public void CalcAzimuth_ValidPoints_ReturnsValidAzimuth()
        {
            IGlobePoint point1 = new GlobePoint(0, -60);
            IGlobePoint point2 = new GlobePoint(-60, 10);
            double expectedAzimuth = 151.5;

            double actualAzimuth = GeographyCalculator.CalcAzimuthDegree(point1, point2);

            Assert.IsTrue(Math.Abs(actualAzimuth - expectedAzimuth) < AzimuthEpsilon);
        }

        [TestMethod]
        public void CalcAzimuth_EqualPoints_ReturnsZero()
        {
            IGlobePoint point1 = new GlobePoint(-60, 30);
            IGlobePoint point2 = new GlobePoint(-60, 30);
            double expectedAzimuth = 0;

            double actualAzimuth = GeographyCalculator.CalcAzimuthDegree(point1, point2);

            Assert.AreEqual(expectedAzimuth, actualAzimuth);
        }

        [TestMethod]
        public void CalcAzimuth_InvalidPoints_ThrowsArgumentException()
        {
            IGlobePoint point1 = new GlobePoint(-100, 30);
            IGlobePoint point2 = new GlobePoint(-60, 240);

            Assert.ThrowsException<ArgumentException>(() => GeographyCalculator.CalcAzimuthDegree(point1, point2));
        }

        //CalcMidpoint Tests

        [TestMethod]
        public void CalcMidpoint_ValidPoints_ReturnsValidPoint()
        {
            IGlobePoint point1 = new GlobePoint(0, 90);
            IGlobePoint point2 = new GlobePoint(0, 0);
            IGlobePoint expectedPoint = new GlobePoint(0, 45);

            IGlobePoint actualPoint = GeographyCalculator.CalcMidpoint(point1, point2);

            Assert.AreEqual<IGlobePoint>(expectedPoint, actualPoint);
        }

        [TestMethod]
        public void CalcMidpoint_EqualPoints_ReturnsSamePoint()
        {
            IGlobePoint point1 = new GlobePoint(0, 0);
            IGlobePoint point2 = new GlobePoint(0, 0);
            IGlobePoint expectedPoint = new GlobePoint(0, 0);

            IGlobePoint actualPoint = GeographyCalculator.CalcMidpoint(point1, point2);

            Assert.AreEqual<IGlobePoint>(expectedPoint, actualPoint);
        }

        [TestMethod]
        public void CalcMidpoint_InvalidPoints_ThrowsArgumentException()
        {
            IGlobePoint point1 = new GlobePoint(-100, 90);
            IGlobePoint point2 = new GlobePoint(0, 200);

            Assert.ThrowsException<ArgumentException>(() => GeographyCalculator.CalcMidpoint(point1, point2));
        }

        //CalcDestination Tests

        [TestMethod]
        public void CalcDestination_ValidArgs_ReturnsValidPoint()
        {
            IGlobePoint start = new GlobePoint(0, 0);
            double azimuthD = 90;
            double distanceM = 10007550;
            IGlobePoint expectedPoint = new GlobePoint(0, 90);

            IGlobePoint actualPoint = GeographyCalculator.CalcDestination(start, azimuthD, distanceM);

            Assert.AreEqual<IGlobePoint>(expectedPoint, actualPoint);
        }

        [TestMethod]
        public void CalcDestination_ValidPointZeroDistance_ReturnsSamePoint()
        {
            IGlobePoint start = new GlobePoint(30, 60);
            double azimuthD = 0;
            double distanceM = 0;
            IGlobePoint expectedPoint = new GlobePoint(30, 60);

            IGlobePoint actualPoint = GeographyCalculator.CalcDestination(start, azimuthD, distanceM);

            Assert.AreEqual<IGlobePoint>(expectedPoint, actualPoint);
        }

        [TestMethod]
        public void CalcDestination_InvalidStartPoint_ThrowsArgumentException()
        {
            IGlobePoint start = new GlobePoint(100, 60);
            double azimuthD = 30;
            double distanceM = 1500;

            Assert.ThrowsException<ArgumentException>(() => GeographyCalculator.CalcDestination(start, azimuthD, distanceM));
        }

        [TestMethod]
        public void CalcDestination_InvalidAzimuth_ThrowsArgumentException()
        {
            IGlobePoint start = new GlobePoint(90, 60);
            double azimuthD = -150;
            double distanceM = 1500;

            Assert.ThrowsException<ArgumentException>(() => GeographyCalculator.CalcDestination(start, azimuthD, distanceM));
        }

        [TestMethod]
        public void CalcDestination_InvalidDistance_ThrowsArgumentException()
        {
            IGlobePoint start = new GlobePoint(90, 60);
            double azimuthD = 30;
            double distanceM = -1500;

            Assert.ThrowsException<ArgumentException>(() => GeographyCalculator.CalcDestination(start, azimuthD, distanceM));
        }
    }
}
