#region Disclaimer

/* 
 * CelestialFeatures
 *
 * Created on 30 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Multiversal Rule System Library
 *
 * This source code is Intellectual property of the Author
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

using System;
using System.Text;
namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    public class CelestialFeatures
    {
        #region Private Fields

        bool isGasGiant;
        double albedo;
        double axialTilt;
        double dayLength;
        double cloudCoverage;
        double density;
        double eccentricity;
        double escapeVelocity;
        double exosphericTemperature;
        double iceCoverage;
        double molecularWeightRetained;
        double radius;
        double nightTemp;
        double dayTemp;
        double mass;
        double minTemp;
        double maxTemp;
        double surfaceGravity;
        double surfacePressure;
        double surfaceTemperature;
        double waterBoilingPoint;
        double hydrographicCoverage;
        double orbitalPeriod;
        double semiMajorAxis;
        double semiMinorAxis;
        double dustMass;
        double gasMass;
        OrbitalZone orbitalZone;
        Gas[] atmosphere;
        #endregion

        #region Properties
        
        public OrbitalParameters OrbitalParameters
        {
            get { return new OrbitalParameters(semiMajorAxis, semiMinorAxis, eccentricity, dayLength, orbitalPeriod, axialTilt);}
        }


        public bool IsGasGiant
        {
            get { return isGasGiant; }
        }

        public double DustMass
        {
            get { return dustMass; }
        }

        public double MolecularWeightRetained
        {
            get { return molecularWeightRetained; }
        }

        public double GasMass
        {
            get { return gasMass; }
        }

        public double Albedo
        {
            get { return albedo; }
        }

        public double AxialTilt
        {
            get { return axialTilt; }
        }

        public double DayLength
        {
            get { return dayLength; }
        }

        public double CloudCoverage
        {
            get { return cloudCoverage; }
        }

        public double Density
        {
            get { return density; }
        }

        public double Eccentricity
        {
            get { return eccentricity; }
        }

        public double EscapeVelocity
        {
            get { return escapeVelocity; }
        }

        public double ExosphericTemperature
        {
            get { return exosphericTemperature; }
        }

        public double IceCoverage
        {
            get { return iceCoverage; }
        }

        public double MinMolecularWeightRetained
        {
            get { return molecularWeightRetained; }
        }

        public double Radius
        {
            get { return radius; }
        }

        public double NightTemp
        {
            get { return nightTemp; }
        }

        public double DayTemp
        {
            get { return dayTemp; }
        }

        public double MinTemp
        {
            get { return minTemp; }
        }

        public double Mass
        {
            get { return mass; }
        }

        public double MaxTemp
        {
            get { return maxTemp; }
        }

        public double SurfaceGravity
        {
            get { return surfaceGravity; }
        }

        public double SurfacePressure
        {
            get { return surfacePressure; }
        }

        public double SurfaceTemperature
        {
            get { return surfaceTemperature; }
        }

        public double WaterBoilingPoint
        {
            get { return waterBoilingPoint; }
        }

        public double HydrographicCoverage
        {
            get { return hydrographicCoverage; }
        }

        public double OrbitalPeriod
        {
            get { return orbitalPeriod; }
        }

        public double SemiMajorAxis
        {
            get { return semiMajorAxis; }
        }

        public double SemiMinorAxis
        {
            get { return semiMinorAxis; }
        }

        public Gas[] Atmosphere
        {
            get { return atmosphere; }
        }

        public OrbitalZone OrbitalZone
        {
            get { return orbitalZone; }
        }

        #endregion

        #region Constructor
        public CelestialFeatures(
            bool isGasGiant,
            double albedo,
            double axialTilt,
            double dayLength,
            double cloudCoverage,
            double density,
            double eccentricity,
            double escapeVelocity,
            double exosphericTemperature,
            double iceCoverage,
            double molecularWeightRetained,
            double radius,
            double nightTemp,
            double dayTemp,
            double mass,
            double minTemp,
            double maxTemp,
            double surfaceGravity,
            double surfacePressure,
            double surfaceTemperature,
            double waterBoilingPoint,
            double hydrographicCoverage,
            double orbitalPeriod,
            double orbitalRadius,
            double dustMass,
            double gasMass,
            OrbitalZone orbitalZone,
            Gas[] atmosphere)
        {
            this.isGasGiant = isGasGiant;
            this.albedo = albedo;
            this.axialTilt = axialTilt;
            this.dayLength = dayLength;
            this.cloudCoverage = cloudCoverage;
            this.density = density;
            this.eccentricity = eccentricity;
            this.escapeVelocity = escapeVelocity;
            this.exosphericTemperature = exosphericTemperature;
            this.iceCoverage = iceCoverage;
            this.molecularWeightRetained = molecularWeightRetained;
            this.radius = radius;
            this.nightTemp = nightTemp;
            this.dayTemp = dayTemp;
            this.mass = mass;
            this.minTemp = minTemp;
            this.maxTemp = maxTemp;
            this.surfaceGravity = surfaceGravity;
            this.surfacePressure = surfacePressure;
            this.surfaceTemperature = surfaceTemperature;
            this.waterBoilingPoint = waterBoilingPoint;
            this.hydrographicCoverage = hydrographicCoverage;
            this.orbitalPeriod = orbitalPeriod;
            this.semiMajorAxis = orbitalRadius;
            semiMinorAxis = semiMajorAxis * (Math.Sqrt(1 - (eccentricity * eccentricity)));
            this.orbitalZone = orbitalZone;
            this.atmosphere = atmosphere;
            this.dustMass = dustMass;
            this.gasMass = gasMass;
        } 
        #endregion

        internal double NearestMoon
        {
            get { return (0.3/50*Math.Pow(mass/PhysicalConstants.SolarMassInEarthMasses, (1.0/3.0))); }
        }

        internal double FarthestMoon
        {
            get { return (50.0 / 40.0 * Math.Pow(mass / PhysicalConstants.SolarMassInEarthMasses, (1.0 / 3.0))); }
        }
    

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("A:{0:F2} AU E:{1:F2} To:{2:F2} Yrs Td: {3:F2} Hours", semiMajorAxis, eccentricity, orbitalPeriod/365.26,dayLength));
            sb.AppendLine(string.Format("M:{0:F3} EM G:{1:F2} EG D: {2:F2} g/cc AT:{3:F2}°", mass, surfaceGravity, density,axialTilt));
            sb.AppendLine(string.Format("R:{0:F2} Km EV:{1:F2} Km/sec Al:{2:F2} OZ:{3}", radius, escapeVelocity, albedo, orbitalZone.ToString()));
            sb.AppendLine(string.Format("P:{0:F2} mB EA MWR:{1:F2}", surfacePressure, molecularWeightRetained));
            sb.AppendLine(string.Format("T:{0:F2} °K ExoT:{1:F2} °K WBP: {2:F2}", surfaceTemperature, exosphericTemperature, waterBoilingPoint));
            sb.AppendLine(string.Format("mT:{0:F2} °K MT:{1:F2} °K NT:{2:F2} °K DT:{3:F2} °K",minTemp,maxTemp, nightTemp, dayTemp));
            sb.AppendLine(string.Format("H:{0:F2}% K:{1:F2}% I:{2:F2}%",hydrographicCoverage,cloudCoverage,iceCoverage));
            sb.AppendLine();
            return sb.ToString();

        }
    }
}