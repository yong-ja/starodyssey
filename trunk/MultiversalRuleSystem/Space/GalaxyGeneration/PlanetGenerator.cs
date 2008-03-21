#region Disclaimer

/* 
 * PlanetGenerator2
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
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;

namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{
    #region Using Directives

    

    #endregion

    public partial class PlanetGenerator
    {
        #region Private fields

        static EnhancedRandom rnd = Dice.Rnd;
        StellarFeatures stellarFeatures;
        ChemicalElement[] chemicalTable = ChemicalElement.CreateGasTable();

        #region Current planet data

        bool greenhouseEffect;
        bool isGasGiant;

        int resonantPeriod;
        int orbitalZone;

        double albedo;
        double axialTilt;
        double dayLength;
        double cloudCoverage;
        double cloudFactor;
        double density;
        double dustMass;
        double eccentricity;
        double escapeVelocity;
        double exosphericTemperature;
        double gasMass;
        double greenhouseRiseInTemperature;
        double iceFactor;
        double iceCoverage;
        double molecularWeightRetained;
        double radius;
        double rmsVelocity;
        double nightTemp;
        double dayTemp;
        double mass;
        double minTemp;
        double maxTemp;
        double rockFactor;
        double surfaceAcceleration;
        double surfaceGravity;
        double surfacePressure;
        double surfaceTemperature;
        double waterBoilingPoint;
        double hydrographicCoverage;
        double orbitalPeriod;
        double orbitalRadius;
        double resonance;
        double stellarMass;
        double volatileGasInventory;
        double waterFactor;

        Gas[] atmosphere;

        #endregion

        #endregion

        #region Properties

        public StellarFeatures StellarFeatures
        {
            get { return stellarFeatures; }
            set
            {
                stellarFeatures = value;
            }
        }

        #endregion

        #region Constructors

        public PlanetGenerator()
        {
        }

        #endregion

        void Reset()
        {
            greenhouseEffect = isGasGiant = false;

            resonantPeriod = orbitalZone = 0;

            albedo =
                axialTilt =
                dayLength =
                cloudCoverage =
                cloudFactor =
                density =
                dustMass =
                eccentricity =
                escapeVelocity =
                exosphericTemperature =
                gasMass =
                greenhouseRiseInTemperature =
                iceFactor =
                iceCoverage =
                molecularWeightRetained =
                radius =
                rmsVelocity =
                nightTemp =
                dayTemp =
                mass =
                minTemp =
                maxTemp =
                rockFactor =
                surfaceAcceleration =
                surfaceGravity =
                surfacePressure =
                surfaceTemperature =
                waterBoilingPoint =
                hydrographicCoverage =
                orbitalPeriod =
                orbitalRadius =
                resonance =
                stellarMass =
                volatileGasInventory =
                waterFactor = 0.0;
        }

        double GenerateDensity(PlanetClass planetClass)
        {
            switch (planetClass)
            {
                case PlanetClass.Planet:
                    return rnd.LogNormalDeviate(0.2);

                case PlanetClass.GasGiant:
                    return rnd.LogNormalDeviate(0.3);

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Empirically determine density based on distance from primary.
        /// </summary>
        /// <param name="mass">Mass in Earth masses.</param>
        /// <param name="orbitalRadius">Semi-major axis of orbit in AU.</param>
        /// <param name="ecosphereRadius">The ecosphere radius.</param>
        /// <param name="isGasGiant">if set to <c>true</c>, specifies that it is a gas giant.</param>
        /// <returns>Density in grams per cubic centimeter.</returns>
        public static double GenerateEmpiricalDensity(double mass, double orbitalRadius, double ecosphereRadius,
                                                      bool isGasGiant)
        {
            if (mass == 0.0) return 0.0; // for grit belts (see constructor, mass_by_integration)

            double temp = Math.Pow(mass, (1.0 / 8.0)) * Math.Pow(ecosphereRadius / orbitalRadius, 0.25);

            if (isGasGiant)
                return temp * 1.2;
            else
                return temp * PhysicalConstants.EarthDensity;
        }


        /// <summary>
        /// Determines if the planet suffers from runaway greenhouse effect.
        /// </summary>
        /// <param name="ecosphereRadius">Radius of ecosphere in AU.</param>
        /// <param name="orbitalRadius">Semi-major axis of orbit, in AU.</param>
        /// <returns><c>true</c> if planet is a greenhouse; <c>false</c> otherwise.</returns>
        public static bool DetermineRunawayGreenhouseEffect(double ecosphereRadius, double orbitalRadius)
        {
            double temp = ComputeBlackbodyTemperature(orbitalRadius, ecosphereRadius, 0.20);


            if (temp > PhysicalConstants.FreezingPointOfWater)
                return true;
            else
                return false;
        }


        public CelestialFeatures GeneratePlanet(Protoplanet p)
        {
            isGasGiant = p.IsGasGiant;
            mass = p.Mass * PhysicalConstants.SolarMassInEarthMasses;
            dustMass = p.DustMass * PhysicalConstants.SolarMassInEarthMasses;
            gasMass = p.GasMass * PhysicalConstants.SolarMassInEarthMasses;
            orbitalRadius = p.A;
            eccentricity = p.E;

            if (mass < 1.0E-6)
            {
                mass = 0.0;
            }

            return ComputeValues(stellarFeatures.EcosphereRadius);
        }

        CelestialFeatures ComputeValues(double ecosphereRadius)
        {
            stellarMass = stellarFeatures.Mass;

            orbitalPeriod = ComputeOrbitalPeriod(orbitalRadius, stellarMass,
                                                 mass / PhysicalConstants.SolarMassInEarthMasses);
            orbitalZone = ComputeOrbitalZone(orbitalRadius, stellarFeatures.Luminosity);


            if (isGasGiant)
            {
                density = GenerateEmpiricalDensity(mass, orbitalRadius, ecosphereRadius, isGasGiant);
                radius = ComputeRadius(mass, density);
            }
            else
            {
                radius = ComputeKothariRadius(mass, isGasGiant, orbitalZone);
                density = ComputeDensity(mass, radius);
            }

            surfaceAcceleration = ComputeSurfaceAcceleration(mass, radius);
            dayLength = ComputeDayLength(mass, radius, eccentricity, density, orbitalRadius, orbitalPeriod,
                                         stellarMass, stellarFeatures.Age, isGasGiant, out resonance);

            resonantPeriod = (int) resonance;
            escapeVelocity = ComputeEscapeVelocityUsingMass(mass, radius);
            axialTilt = GenerateInclination(orbitalRadius);
            rmsVelocity = ComputeRmsVelocity(PhysicalConstants.MolecularNitrogen, orbitalRadius);
            molecularWeightRetained = ComputeMolecularWeightRetained(escapeVelocity);
            exosphericTemperature = PhysicalConstants.EarthExosphereTemperature /
                                    Math.Pow(orbitalRadius / ecosphereRadius, 2);

            if (isGasGiant)
            {
                surfaceGravity = ComputeSurfaceGravityUsingMass(density, radius);
                volatileGasInventory = double.NaN;
                surfacePressure = double.NaN;
                surfaceTemperature = double.NaN;
                waterBoilingPoint = double.NaN;
                hydrographicCoverage = double.NaN;

                greenhouseEffect = false;

                albedo = rnd.About(PhysicalConstants.GasGiantAlbedo, 0.1);
            }
            else
            {
                surfaceGravity = ComputeSurfaceGravityUsingAcceleration(surfaceAcceleration);
                greenhouseEffect = DetermineRunawayGreenhouseEffect(ecosphereRadius, orbitalRadius);
                volatileGasInventory = GenerateVolatileGasInventory(mass, escapeVelocity, rmsVelocity, stellarMass,
                                                                    orbitalZone, greenhouseEffect, gasMass / mass > 1E-6);
                surfacePressure = ComputeSurfacePressure(radius, surfaceGravity, volatileGasInventory);

                // sometimes airless rocks are showing a greenhouse effect;
                // remove that effect if the rock has very little air.
                if (surfacePressure < 0.01)
                    greenhouseEffect = false;

                waterBoilingPoint = (surfacePressure == 0.0) ? 0.0 : ComputeWaterBoilingPoint(surfacePressure);
                EstimateSurfaceTemperature(ecosphereRadius);
            }

            CalculateGases();

            Reset();
            return null;
        }

        void SetInitialConditions(double ecosphereRadius)
        {
            cloudFactor = rnd.About(PhysicalConstants.CloudAlbedo, 0.2);
            waterFactor = rnd.About(PhysicalConstants.WaterAlbedo, 0.2);

            if (surfacePressure == 0.0)
            {
                rockFactor = rnd.About(PhysicalConstants.RockyAirlessAlbedo, 0.3);
                iceFactor = rnd.About(PhysicalConstants.AirlessIceAlbedo, 0.4);
            }
            else
            {
                rockFactor = rnd.About(PhysicalConstants.RockyAirlessAlbedo, 0.1);
                iceFactor = rnd.About(PhysicalConstants.AirlessIceAlbedo, 0.1);
            }

            albedo = PhysicalConstants.EarthAlbedo;
            surfaceTemperature = ComputeSurfaceTemperature(orbitalRadius, ecosphereRadius,
                                                           albedo, surfacePressure, molecularWeightRetained);
            SetTemperatureRange();

            //hydrographicCoverage = ComputeHydrographicCoverage(radius, volatileGasInventory);
            //iceCoverage = ComputeIceCoverage(surfaceTemperature, hydrographicCoverage);
            //cloudCoverage = ComputeCloudCoverage(radius, surfacePressure, surfaceTemperature, molecularWeightRetained,
            //                                     hydrographicCoverage);
        }

        void EstimateSurfaceTemperature(double ecosphereRadius)
        {
            const int Iterations = 50;
            bool accretedGas = (gasMass / mass) > 1E-6;
            SetInitialConditions(ecosphereRadius);
            double initialTemperature = surfaceTemperature;

            for (int i = 0; i < Iterations; i++)
            {
                double previousWaterCoverage = hydrographicCoverage;
                double previousIceCoverage = iceCoverage;
                double previousCloudCoverage = cloudCoverage;
                double previousSurfaceTemperature = surfaceTemperature;
                double previousAlbedo = albedo;

                bool atmosphereBoiledOff = false;
                if (greenhouseEffect && maxTemp < waterBoilingPoint)
                {
                    greenhouseEffect = false;
                    volatileGasInventory = GenerateVolatileGasInventory(mass,
                                                                        escapeVelocity,
                                                                        rmsVelocity,
                                                                        stellarMass,
                                                                        orbitalZone,
                                                                        greenhouseEffect,
                                                                        accretedGas);
                    surfacePressure = ComputeSurfacePressure(radius, surfaceGravity, volatileGasInventory);
                    waterBoilingPoint = ComputeWaterBoilingPoint(surfacePressure);
                }

                hydrographicCoverage = ComputeHydrographicCoverage(radius, volatileGasInventory);
                cloudCoverage = ComputeCloudCoverage(radius, surfacePressure, surfaceTemperature,
                                                     molecularWeightRetained, hydrographicCoverage);

                iceCoverage = ComputeIceCoverage(surfaceTemperature, hydrographicCoverage);

                if (greenhouseEffect && surfacePressure > 0.0)
                    cloudCoverage = 1;
                if ((dayTemp >= waterBoilingPoint) && ((int) dayLength == (int) (orbitalPeriod * 24.0)) ||
                    resonantPeriod == 1)
                {
                    hydrographicCoverage = 0;
                    atmosphereBoiledOff = true;

                    if (molecularWeightRetained > PhysicalConstants.WaterVapor)
                        cloudCoverage = 0.0;
                    else
                        cloudCoverage = 1.0;
                }
                if (surfaceTemperature < (PhysicalConstants.FreezingPointOfWater - 3.0))
                    hydrographicCoverage = 0;


                albedo = ComputeAlbedo(hydrographicCoverage, cloudCoverage, iceCoverage,
                                       rockFactor, cloudFactor, iceFactor, waterFactor, surfacePressure);

                surfaceTemperature = ComputeSurfaceTemperature(
                    orbitalRadius, ecosphereRadius, albedo, surfacePressure, molecularWeightRetained);

                if (i > 0)
                {
                    if (!atmosphereBoiledOff)
                        hydrographicCoverage = (hydrographicCoverage + (previousWaterCoverage * 2)) / 3;
                    cloudCoverage = (cloudCoverage + (previousCloudCoverage * 2)) / 3;
                    iceCoverage = (iceCoverage + (previousIceCoverage * 2)) / 3;
                    albedo = (albedo + (previousAlbedo * 2)) / 3;
                    surfaceTemperature = (surfaceTemperature + (previousSurfaceTemperature * 2)) / 3;

                    SetTemperatureRange();
                }

                if (Math.Abs(surfaceTemperature - previousSurfaceTemperature) < 0.25)
                    break;
            }

            greenhouseRiseInTemperature = surfaceTemperature - initialTemperature;
        }

        void SetTemperatureRange()
        {
            double pressmod = 1 / Math.Sqrt(1 + 20 * surfacePressure / 1000.0);
            double ppmod = 1 / Math.Sqrt(10 + 5 * surfacePressure / 1000.0);
            double tiltmod = Math.Abs(Math.Cos(axialTilt) * Math.PI / 180) * Math.Pow(1 + eccentricity, 2);
            double daymod = 1 / (200 / dayLength + 1);
            double mh = Math.Pow(1 + daymod, pressmod);
            double ml = Math.Pow(1 - daymod, pressmod);
            double hi = mh * surfaceTemperature;
            double lo = ml * surfaceTemperature;
            double sh = hi + Math.Pow((100 + hi) * tiltmod, Math.Sqrt(ppmod));
            double wl = lo - Math.Pow((150 + lo) * tiltmod, Math.Sqrt(ppmod));
            double max = surfaceTemperature + Math.Sqrt(surfaceTemperature) * 10;
            double min = surfaceTemperature / Math.Sqrt(dayLength + 24);

            if (lo < min) lo = min;
            if (wl < 0) wl = 0;

            dayTemp = Soft(hi, max, min);
            nightTemp = Soft(lo, max, min);
            maxTemp = Soft(sh, max, min);
            minTemp = Soft(wl, max, min);
        }

        void CalculateGases()
        {
            double stellarAge = stellarFeatures.Age;
            double[] amount = new double[ChemicalElement.GasCount];
            double totalAmount = 0;
            int n = 0;

            if (surfacePressure > 0)
            {
                double pressure = surfacePressure / PhysicalConstants.MillibarsPerBar;
                for (int i = 0; i < ChemicalElement.GasCount; i++)
                {
                    double boilingPoint = chemicalTable[i].BoilingPointInCelsius /
                                          (373 * ((Math.Log(pressure) + 0.001) / -5050.5) +
                                           (1.0 / 373));
                    if ((boilingPoint >= 0 && boilingPoint < nightTemp)
                        && (chemicalTable[i].AtomicWeight >= molecularWeightRetained))
                    {
                        double vrms = ComputeRmsVelocity(chemicalTable[i].AtomicWeight, orbitalRadius);
                        double pvrms = Math.Pow(1 / (1 + vrms / escapeVelocity), stellarAge / 1e9);
                        double abundance = chemicalTable[i].AbundanceEarth;
                        double reactivity = 1.0;
                        double fraction = 1.0;
                        double pressure2 = 1.0;

                        if (chemicalTable[i].Symbol == ChemicalSymbol.Ar)
                        {
                            reactivity = .19 * stellarAge / 4E9;
                        }
                        else if (chemicalTable[i].Symbol == ChemicalSymbol.He)
                        {
                            abundance *= (.001 + (gasMass / mass));
                            pressure2 += 0.75;
                            reactivity =
                                Math.Pow(1 / (1 + chemicalTable[i].Reactivity), stellarAge / 2E9 * pressure2);
                        }
                        else if (chemicalTable[i].Symbol == ChemicalSymbol.O
                                 // || gasTable[i].Symbol == ChemicalSymbol.O2)
                                 && stellarAge > 2E9 && (surfaceTemperature > 270 && surfaceTemperature < 400))
                        {
                            reactivity = Math.Pow(1 / (1 + chemicalTable[i].Reactivity),
                                                  Math.Pow(stellarAge / 2E9, 0.25) * pressure2);
                        }
                        else if (chemicalTable[i].Symbol == ChemicalSymbol.CO2 &&
                                 stellarAge > 2E9 && (surfaceTemperature > 270 && surfaceTemperature < 400))
                        {
                            pressure2 += pressure;
                            reactivity = 1.5 * Math.Pow(1 / (1 + chemicalTable[i].Reactivity),
                                                        Math.Pow(stellarAge / 2E9, 0.5) * pressure2);
                        }
                        else
                        {
                            pressure2 += 0.75;
                            reactivity = Math.Pow(1 / (1 + chemicalTable[i].Reactivity),
                                                  stellarAge / 2E9 * pressure2);
                        }

                        fraction = 1 - (molecularWeightRetained / chemicalTable[i].AtomicWeight);
                        amount[i] = abundance * pvrms * reactivity * fraction;
                        totalAmount += amount[i];
                        if (amount[i] > 0.0)
                            n++;
                    }
                    else
                        amount[i] = 0.0;
                }

                if (n > 0)
                {
                    atmosphere = new Gas[n];
                    n = 0;
                    for (int i = 0; i < ChemicalElement.GasCount; i++)
                    {
                        if (amount[i] > 0.0)
                        {
                            atmosphere[n] = new Gas(chemicalTable[i].AtomicNumber,
                                                    chemicalTable[i].Symbol,
                                                    surfacePressure * amount[i] / totalAmount);
                            n++;
                        }
                    }
                }
            }
        }
    }
}