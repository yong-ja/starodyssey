#region Disclaimer

/* 
 * StellarFeatures
 *
 * Created on 30 agosto 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Multiversal Rule System Library
 *
 * This source code is Intellectual Property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

#region Using Directives

    using System;
    using System.Text;

#endregion

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    

    public class StellarFeatures
    {
        #region Private fields

        readonly double mass;
        readonly SpectralClass spectralClass;
        readonly double luminosity;
        readonly double magnitude;
        readonly double temperature;
        readonly double density;
        readonly double age;
        readonly double radius;

        readonly double semiMajorAxis;
        readonly double semiMinorAxis;
        readonly double eccentricity;
        readonly double orbitalPeriod;
        StarType type;

        #endregion

        #region Properties

        public double Radius
        {
            get { return radius; }
        }

        public double Age
        {
            get { return age; }
        }

        public double Mass
        {
            get { return mass; }
        }

        public double Luminosity
        {
            get { return luminosity; }
        }

        public double Temperature
        {
            get { return temperature; }
        }

        public SpectralClass SpectralClass
        {
            get { return spectralClass; }
        }

        public StarType Type
        {
            get { return type; }
        }

        internal double InnerLimit
        {
            get { return 0.3*Math.Pow(mass, (1.0/3.0)); }
        }

        internal double OuterLimit
        {
            get { return 200.0*Math.Pow(mass, (1.0/3.0)); }
        }

        internal double StellarDustLimit
        {
            get { return 200.0*Math.Pow(mass, (1.0/3.0)); }
        }

        /// <summary>
        /// Computes the radius of the Ecosphere (habitable zone) in AU.
        /// </summary>
        /// <value>The Ecosphere radius in AU.</value>
        public double EcosphereRadius
        {
            get { return Math.Sqrt(luminosity); }
        }

        /// <summary>
        /// Computes the radius of the Greenhouse zone in AU.
        /// </summary>
        /// <value>The Greenhouse radius in AU.</value>
        public double GreenhouseRadius
        {
            get { return EcosphereRadius*PhysicalConstants.GreenhouseEffect; }
        }

        #endregion

        #region Constructors

        public StellarFeatures(double magnitude, double mass, double luminosity, double temperature, double radius, double semiMajorAxis, double eccentricity, double orbitalPeriod, double density, double age, SpectralClass spectralClass)
        {
            this.mass = mass;
            this.magnitude = magnitude;
            this.luminosity = luminosity;
            this.spectralClass = spectralClass;
            this.density = density;
            this.temperature = temperature;
            this.radius = radius;
            this.age = age;
            this.semiMajorAxis = semiMajorAxis;
            this.eccentricity = eccentricity;
            this.semiMinorAxis = semiMajorAxis * (Math.Sqrt(1 - (eccentricity * eccentricity)));
            this.orbitalPeriod = orbitalPeriod;
        }

        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0}",spectralClass.CommonType));
            sb.AppendLine(string.Format("{0}: Age:{1:F2} GY", spectralClass.ToString(), age));
            sb.AppendLine(string.Format("A: {0:F2} AU - B: {1:F2} AU - E: {2:F3}", semiMajorAxis, semiMinorAxis, eccentricity));
            sb.AppendLine(string.Format("M: {0:F2} D:{1:F2} Kg/Km3 T:{2:F3} Yrs", mass, density, orbitalPeriod));
            sb.AppendLine(string.Format("L: {0:F2} MBol: {1:F2}", luminosity, magnitude));
            sb.AppendLine(string.Format("Teff: {0:F0} °K", temperature));
            sb.AppendLine(string.Format("R: {0:F2}", radius));
            sb.AppendLine();
            return sb.ToString();
        }
    }
}