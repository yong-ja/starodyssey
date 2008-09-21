using System;
using System.Collections.Generic;
using System.Text;

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    public struct OrbitalParameters
    {
        readonly double semiMajorAxis;
        readonly double semiMinorAxis;
        readonly double eccentricity;
        readonly double rotationPeriod;
        readonly double orbitalPeriod;
        readonly double axialTilt;

        #region Properties
        public double SemiMajorAxis
        {
            get { return semiMajorAxis; }
        }

        public double SemiMinorAxis
        {
            get { return semiMinorAxis; }
        }

        public double Eccentricity
        {
            get { return eccentricity; }
        }

        /// <summary>
        /// Rotation period in Hours
        /// </summary>
        public double RotationPeriod
        {
            get { return rotationPeriod; }
        }

        /// <summary>
        /// Orbital Period in Days
        /// </summary>
        public double OrbitalPeriod
        {
            get { return orbitalPeriod; }
        }

        /// <summary>
        /// Returns the fraction of arc that this celestial body travels each day.
        /// </summary>
        public double OrbitalDelta
        {
            get { return 30/365.0*(360/orbitalPeriod); }
        }

        public double AxialTilt
        {
            get { return axialTilt; }
        } 
        #endregion

        public OrbitalParameters(double semiMajorAxis, double semiMinorAxis, double eccentricity, double rotationPeriod, double orbitalPeriod, double axialTilt)
        {
            this.semiMajorAxis = semiMajorAxis;
            this.semiMinorAxis = semiMinorAxis;
            this.eccentricity = eccentricity;
            this.rotationPeriod = rotationPeriod;
            this.orbitalPeriod = orbitalPeriod;
            this.axialTilt = axialTilt;
        }

       
    }
}
