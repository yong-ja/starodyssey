#region Disclaimer

/* 
 * PhysicalConstants
 *
 * Created on 01 settembre 2007
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

using System;

namespace AvengersUtd.MultiversalRuleSystem.Space
{
    public static class PhysicalConstants
    {
        public const double RadiansPerRotation = (2.0*Math.PI);
        public const double DegreeToRadian = (Math.PI/180.0);
        public const double RadianToDegree = (180.0/Math.PI);
        public const double Angstrom = 1.0E-8;

        /// <summary>
        /// Gravitational constant.
        /// </summary>
        public const double G = 6.67E-08;

        /// <summary>
        /// Mass of Earth in kilograms.
        /// </summary>
        public const double EarthMassInKilograms = 5.9736E+24;

        public const double BoltzmannConstant = 1.38E-16; // Boltzmann constant 

        /// <summary>
        /// The Absolute Bolumetric Magnitude of the Sun
        /// </summary>
        public const double SolarAbsoluteBolumetricMagnitude = 4.75;


        public const double SolarMainSequenceLifespan = 1E10;
        /// <summary>
        /// Stefan-Boltzmann Constant (in N/m^2/K)
        /// </summary>
        public const double StefanBoltzmannConstant = 5.67E-8; 
        public const double HydrogenMass = 1.673E-24; // mass of hydrogen atom 
        /// <summary>
        /// Bolumetric Luminosity of the Sun in Watts.
        /// </summary>
        public const double SolarBolumetricLuminosity = 3.827E26;

        public const double SolarRadiusInKilometers = 696000;

        /// <summary>
        /// Effective temperature of the Sun in degrees Kelvin.
        /// </summary>
        public const double SolarEffectiveTemperature = 5778;
        public const double H2 = 2.016;
        public const double H2O = 18.016;
        public const double N2 = 28.016;
        public const double O2 = 32.0;
        public const double CO2 = 44.011;

        public const double ChangeInEarthAngularVelocity = (-1.3E-15); // Units of radians/sec/year
        public const double SolarMassInGrams = (1.9891E33); // Units of grams
        public const double SolarMassInKilograms = 1.9891E30; // Units of Kilograms
        public const double JupiterMassInEarthMasses = 317.8;
        public const double EarthMassInGrams = (5.9736E27); // Units of grams 
        public const double EarthAverageTemperatureInCelsius = 14.0;
        public const double EarthAverageTempeartureInKelvin = EarthAverageTemperatureInCelsius + FreezingPointOfWater;  
        public const double EarthRadius = (6.378137E8); // Units of cm		    
        public const double EarthDensity = (5.5153); // Units of g/cc	    
        public const double EarthRadiusInKm = (6378.137); // Units of km              
        public const double EarthAcceleration = (981.0); // Units of cm/sec2         
        public const double EarthAxialTilt = (23.439281); // Units of degrees         
        public const double EarthExosphereTemperature = (1273.0); // Units of degrees Kelvin  
        public const double SolarMassInEarthMasses = (332946.0);
        public const double EarthEffectiveTemperature = (255.0); // Units of degrees Kelvin  
        public const double EarthAlbedo = (0.367);
        public const double CloudCoverageFactor = (1.839E-8); // Km2/kg                   
        public const double EarthWaterMassPerArea = (3.83E15); // grams per square km     
        public const double EarthSurfacePressureInMillibars = (1013.25);
        public const double EarthConvectionFactor = (0.43); // from Hart, eq.20         
        public const double FreezingPointOfWater = (273.0); // Units of degrees Kelvin  
        public const double DaysPerYear = (365.256366); // Earth days per Earth year
        public const double GasRetentionThreshold = (5.0); // ratio of esc vel to RMS vel (was 6.0) 
        public const double GasGiantAlbedo = (0.5); // albedo of a gas giant    
        public const double CloudAlbedo = (0.52);
        public const double RockyAirlessAlbedo = (0.07);
        public const double RockyAlbedo = (0.15);
        public const double WaterAlbedo = (0.04);
        public const double AirlessIceAlbedo = (0.5);
        public const double IceAlbedo = (0.7);
        public const double SecondsPerHour = (3600.0);
        public const double CmPerAU = (1.495978707E13); // number of cm in an AU    
        public const double CmPerKm = (1.0E5); // number of cm in a km     
        public const double KmPerAU = (CmPerAU/CmPerKm);
        public const double CmPerMeter = (100.0);
        public const double MetersPerKilometer = 1000;
        public const double MillibarsPerBar = (1000.0);
        public const double KelvinCelsiusDifference = (273.0);
        /// <summary>
        /// Universal gravitational constant in Nm^2/kg^2
        /// </summary>
        public const double UniversalGravitationalConstant = 6.67259E-11;
        public const double GravitationalConstant = (6.672E-8); // units of dyne cm2/gram2  
        /// <summary>
        /// Affects inner radius.
        /// </summary>
        public const double GreenhouseEffect = (0.93);    
        public const double MolarGasConstant = (8314.41); // units: g*m2/(sec2*K*mol) 
        public const double K = (50.0); // K = gas/dust ratio       
        public const double B = (1.2E-5); // Used in Crit_mass calc   
        public const double DustDensityCoefficient = (2.0E-3); // A in Dole's paper        
        public const double Alpha = (5.0); // Used in density calcs    
        public const double N = (3.0); // Used in density calcs    
        public const double J = (1.46E-19); // Used in day-length calcs (cm2/sec2 g) 
        public const double EccentricityCoefficient = (0.077);

        //  Now for a few molecular weights (used for RMS velocity calcs):
        //  This table is from Dole's book "Habitable Planets for Man", p. 38

        public const double AtomicHydrogen = (1.0); // H   
        public const double MolecularHydrogen = (2.0); // H2  
        public const double Helium = (4.0); // He  
        public const double AtomicNitrogen = (14.0); // N   
        public const double AtomicOxygen = (16.0); // O   
        public const double Methane = (16.0); // CH4 
        public const double Ammonia = (17.0); // NH3 
        public const double WaterVapor = (18.0); // H2O 
        public const double Neon = (20.2); // Ne  
        public const double MolecularNitrogen = (28.0); // N2  
        public const double CarbonMonoxide = (28.0); // CO  
        public const double NitricOxide = (30.0); // NO  
        public const double MolecularOxygen = (32.0); // O2  
        public const double HydrogenSulphide = (34.1); // H2S 
        public const double Argon = (39.9); // Ar  
        public const double CarbonDioxide = (44.0); // CO2 
        public const double NitrousOxide = (44.0); // N2O 
        public const double NitrogenDioxide = (46.0); // NO2 
        public const double Ozone = (48.0); // O3  
        public const double SulphuricDioxide = (64.1); // SO2 
        public const double SulphuricTrioxide = (80.1); // SO3 
        public const double Krypton = (83.8); // Kr  
        public const double Xenon = (131.3); // Xe  

        //  The following constants are used in the kothari_radius function

        public const double A1_20 = (6.485E12); // All units are in cgs system.  
        public const double A2_20 = (4.0032E-8); //   ie: cm, g, dynes, etc.      
        public const double BETA_20 = (5.71E12);

        //   The following values are used in determining the fraction of a planet
        //  covered with clouds in function cloud_fraction

        public const double Q1_36 = (1.258E19); // grams    
        public const double Q2_36 = (0.0698); // 1/Kelvin 
    }
}