#region Disclaimer

/* 
 * PlanetGenerator.Computations
 *
 * Created on 02 settembre 2007
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
#endregion

namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{
    public partial class PlanetGenerator
    {
        /// <summary>
        /// Calculates radius based on volume, calibrated to Earth.
        /// </summary>
        /// <param name="mass">Mass in Earth masses.</param>
        /// <param name="density">Average density, in grams per cubic centimeter.</param>
        /// <returns>Planetary radius in Earth radii.</returns>
        public static double ComputeRadius(double mass, double density)
        {
            double volume;

            if (density == 0.0) return 0.0; // sometimes it happens, for grit

            volume = (mass*PhysicalConstants.EarthMassInGrams)/density;
            return Math.Pow((3.0*volume)/(4.0*Math.PI), (1.0/3.0))/PhysicalConstants.CmPerKm;
        }

        /// <summary>
        /// Calculates average density of body, given mass and radius.
        /// </summary>
        /// <param name="mass">Mass in Earth masses.</param>
        /// <param name="equatorialRadius">Equatorial radius in Km.</param>
        /// <returns>Average density in g/cc.</returns>
        public static double ComputeDensity(double mass, double equatorialRadius)
        {
            double volume;

            if (equatorialRadius == 0.0)
                return 0.0;
            mass *= PhysicalConstants.EarthMassInGrams;
            equatorialRadius *= PhysicalConstants.CmPerKm;
            volume = (4.0*Math.PI*equatorialRadius*equatorialRadius*equatorialRadius)/3.0;
            return (mass/volume);
        }


        /// <summary>
        /// Calculates the surface acceleration of a Planet.
        /// </summary>
        /// <param name="mass">Planetary mass in Earth masses.</param>
        /// <param name="radius">Equatorial radius in Km.</param>
        /// <returns>Acceleration in cm/sec^2</returns>
        public static double ComputeSurfaceAcceleration(double mass, double radius)
        {
            if (radius == 0.0) return 0.0;
            return (PhysicalConstants.GravitationalConstant*(mass*PhysicalConstants.EarthMassInGrams)/
                    Math.Pow(radius*PhysicalConstants.CmPerKm, 2.0));
        }

        /// <summary>
        /// Calculates the surface gravity of the planet.
        /// </summary>
        /// <param name="acceleration">Acceleration Surface gravity in cm/sec^2.</param>
        /// <returns>Surface gravity in Earth gravities.</returns>
        public static double ComputeSurfaceGravityUsingAcceleration(double acceleration)
        {
            return (acceleration/PhysicalConstants.EarthAcceleration);
        }

        /// <summary>
        /// Computes the surface gravity using its density.
        /// </summary>
        /// <param name="density">The density of the planet, in g/c^3.</param>
        /// <param name="radius">The radius of the planet, in Km.</param>
        /// <returns>The surface gravity of the planet, in m/sec^2.</returns>
        public static double ComputeSurfaceGravityUsingDensity(double density, double radius)
        {
            return (4 * Math.PI * PhysicalConstants.UniversalGravitationalConstant * 
                radius * density / 3) * 1E6;
        }

        /// <summary>
        /// Calculates the surface gravity of the planet using its mass.
        /// </summary>
        /// <param name="mass">The mass of the planet, in Earth Masses.</param>
        /// <param name="radius">The radius of the planet in Km.</param>
        /// <returns>The surface gravity of the planet, in m/sec^2.</returns>
        public static double ComputeSurfaceGravityUsingMass(double mass, double radius)
        {
            // g = Gm/r^2
            return (PhysicalConstants.UniversalGravitationalConstant * 
                (mass*PhysicalConstants.EarthMassInKilograms)) / Math.Pow(radius*1000,2);
        }

        /// <summary>
        /// This function implements the escape velocity calculation using mass.
        /// </summary>
        /// <param name="mass">Mass in Earth masses.</param>
        /// <param name="radius">Radius in Km.</param>
        /// <returns>Escape velocity in Km/sec.</returns>
        public static double ComputeEscapeVelocityUsingMass(double mass, double radius)
        {
            double massInKilograms, radiusInMeters;

            
            // Mass should be in units of solar masses.
            massInKilograms = (mass/PhysicalConstants.SolarMassInEarthMasses)*PhysicalConstants.SolarMassInKilograms;
            radiusInMeters = radius*PhysicalConstants.MetersPerKilometer;

            // The final result should be in Km/s
            // (2Gm/r)^1/2
            return (Math.Sqrt(2.0 * PhysicalConstants.UniversalGravitationalConstant * massInKilograms / radiusInMeters)) / 
                PhysicalConstants.MetersPerKilometer;
        }

        /// <summary>
        /// Computes the escape velocity using gravity.
        /// </summary>
        /// <param name="gravity">The gravity, in m/sec^2.</param>
        /// <param name="radius">The radius, in Km.</param>
        /// <returns></returns>
        public static double ComputeEscapeVelocityUsingGravity(double gravity, double radius)
        {
            // (2gr)^1/2
            return Math.Sqrt(2.0 * gravity * radius * PhysicalConstants.MetersPerKilometer) /
                PhysicalConstants.MetersPerKilometer;
        }



        /// <summary>
        /// Calculates Root Mean Square (RMS) velocity of a molecule or atom.
        /// This is Fogg's eq.16.  Calibrated to Earth exospheric temperature,
        /// which implies that the orbital radius has been preadjusted so that
        /// temperature comparisons are meaningful.
        /// </summary>
        /// <param name="molecularWeight">The molecular weight (usually assumed to be N2).</param>
        /// <param name="orbitalRadius">Semi-major axis of orbit in AU.</param>
        /// <returns>RMS velocity in Km/sec^2.</returns>
        public static double ComputeRmsVelocity(double exosphericTemperature, double molecularWeight)
        {
            return (Math.Sqrt((3.0 * PhysicalConstants.MolarGasConstant * exosphericTemperature) /
                              molecularWeight) / PhysicalConstants.MetersPerKilometer);
        }

        /// <summary>
        /// Calculates the smallest molecular weight retained by the
        /// body, which is useful for determining the atmosphere composition.
        /// </summary>
        /// <param name="escapeVelocity">The escape velocity in cm/sec.</param>
        /// <returns>Molecular weight</returns>
        public static double ComputeMolecularWeightRetained(double exosphericTemperature, double escapeVelocity)
        {
            if (escapeVelocity == 0.0)
                return 0.0;
            else
            {
                return (3.0 * PhysicalConstants.MolarGasConstant * exosphericTemperature) /
                    (Math.Pow((escapeVelocity / PhysicalConstants.GasRetentionThreshold) * PhysicalConstants.MetersPerKilometer, 2));

            }
        }



        /// <summary>
        /// Calculates surface pressure on this planet. Uses volatile gas inventory,
        /// equatorial radius, and surface gravity, and is calibrated so that Earth
        /// (with a gas inventory of 1000 and gravity of 1 G) has a pressure of 1000
        /// millibars. This implements Fogg's eq.18.
        /// </summary>
        /// <param name="equatorialRadius">Equatorial radius in Km.</param>
        /// <param name="gravity">Surface gravity in units of Earth gravities.</param>
        /// <param name="volatileGasInventory">The volatile gas inventory.</param>
        /// <returns>Surface pressure in millibars.</returns>
        public static double ComputeSurfacePressure(double equatorialRadius, double gravity, double volatileGasInventory)
        {
            if (equatorialRadius == 0.0)
                return 0.0;
            else
            {
                equatorialRadius = PhysicalConstants.EarthRadiusInKm/equatorialRadius;
                return (volatileGasInventory*gravity * 
                    (PhysicalConstants.EarthSurfacePressureInMillibars/1000)/
                    (equatorialRadius*equatorialRadius));
            }
        }

        /// <summary>
        /// Calculates the boiling point of water in this planet's atmosphere,
        /// stored in millibars. This is Fogg's eq.21.
        /// </summary>
        /// <param name="surfacePressure">Surface pressure in millibars.</param>
        /// <returns>Boiling point in degrees Kelvin</returns>
        public static double ComputeWaterBoilingPoint(double surfacePressure)
        {
            double surfacePressureInBars;

            surfacePressureInBars = surfacePressure/PhysicalConstants.MillibarsPerBar;
            if (surfacePressureInBars == 0)
                return 0.0;
            else
                return (1.0/(Math.Log(surfacePressureInBars)/-5050.5 + 1.0/373.0));
        }

        /// <summary>
        /// Calculates effective temperature of the planet, based on semi-major
        /// axis, albedo, and provided ecosphere radius.  The equation is calibrated
        /// to the effective temperature of Earth.
        /// This is Fogg's eq.19.
        /// </summary>
        /// <param name="orbitalRadius">Orbital radius in Au.</param>
        /// <param name="ecosphereRadius">Radius of ecosphere in AU.</param>
        /// <param name="albedo">The albedo.</param>
        /// <returns>Effective temperature in degrees Kelvin</returns>
        public static double ComputeBlackbodyTemperature(double orbitalRadius, double ecosphereRadius, double albedo)
        {
            return (Math.Sqrt(ecosphereRadius / orbitalRadius)
                    *Math.Pow((1.0 - albedo)/0.7, 0.25)
                    *PhysicalConstants.EarthEffectiveTemperature);
        }
        
        /// <summary>
        /// /// calculates the number of years it takes for 1/e of a gas to escape
        /// from a planet's atmosphere. 
        ///	Taken from Dole p. 34. He cites Jeans (1916) & Jones (1923)
        /// </summary>
        /// <param name="exosphericTemperature"></param>
        /// <param name="molecularWeight"></param>
        /// <param name="gravity"></param>
        /// <param name="radius"></param>
        /// <returns>The presumed number of years that a given atmosphere will last, in GigaYears (1E9 Yrs)</returns>
        public static double EstimateGasLife(double exosphericTemperature, double molecularWeight, double gravity, double radius)
        {
            double v = ComputeRmsVelocity(exosphericTemperature, molecularWeight) * PhysicalConstants.CmPerKm;
            double g = gravity * PhysicalConstants.EarthAcceleration;
            double r = (radius * PhysicalConstants.CmPerKm);
            double t = (Math.Pow(v,3) / (2.0 * g*g * r)) * Math.Exp((3.0 * g * r) / (v*v));
            double years = t / (PhysicalConstants.SecondsPerHour * 24.0 * PhysicalConstants.DaysPerYear);

            if (years > 2.0E10)
                years = double.PositiveInfinity;

            return years/1E9;
        }

        public static double MinMolecularWeight(double starSystemAge, double exosphericTemperature, double mass, double gravity, double radius)
        {
            double guess1 = ComputeMolecularWeightRetained(exosphericTemperature, ComputeEscapeVelocityUsingMass(mass, radius));
            double guess2 = guess1;

            double life = EstimateGasLife(exosphericTemperature, guess1, gravity, radius);

            int loops = 0;


            if (life > starSystemAge)
            {
                while ((life > starSystemAge) && (loops++ < 25))
                {
                    guess1 = guess1 / 2.0;
                    life = EstimateGasLife(exosphericTemperature, guess1, gravity, radius);
                }
            }
            else
            {
                while ((life < starSystemAge) && (loops++ < 25))
                {
                    guess2 = guess2 * 2.0;
                    life = EstimateGasLife(exosphericTemperature, guess1, gravity, radius);
                }
            }

            loops = 0;

            while (((guess2 - guess1) > 0.1) && (loops++ < 25))
            {
                double guess3 = (guess1 + guess2) / 2.0;
                life = EstimateGasLife(exosphericTemperature, guess3, gravity, radius);

                if (life < starSystemAge)
                    guess1 = guess3;
                else
                    guess2 = guess3;
            }

            life = EstimateGasLife(exosphericTemperature, guess2, gravity, radius);

            return (guess2);
        }


        /// <summary>
        /// Calculates the rise in temperature due to greenhouse effect.
        /// This is Fogg's eq.20, and is also Hart's eq.20 in his "Evolution of
        /// Earth's Atmosphere" article.
        /// </summary>
        /// <param name="surfacePressure">The surface pressure in millibars.</param>
        /// <param name="atmosphericAbsorption">Dimensionless quantity representing atmospheric absorption.</param>
        /// <param name="blackbodyTemperature">Temperature in Kelvin of a blackbody.</param>
        /// <returns>Temperature rise in degrees Kelvin.</returns>
        public static double ComputeGreenhouseRiseInTemperature(double surfacePressure, double atmosphericAbsorption,
                                                         double blackbodyTemperature)
        {
            double convectionFactor = PhysicalConstants.EarthConvectionFactor*
                                      Math.Pow(surfacePressure/PhysicalConstants.EarthSurfacePressureInMillibars, 0.25);
            return (Math.Pow(1.0 + 0.75*atmosphericAbsorption, 0.25) - 1.0)*blackbodyTemperature*convectionFactor;
        }

        /// <summary>
        /// This function returns the dimensionless quantity of optical depth,
        /// which is useful in determining the amount of greenhouse effect on a
        /// Planet.
        /// </summary>
        /// <param name="surfacePressure">The surface pressure in millibars.</param>
        /// <param name="molecularWeightRetained">The molecular weight retained.</param>
        /// <returns>The atmospheric absorption factor.</returns>
        public static double ComputeAbsorptionFactor(double surfacePressure, double molecularWeightRetained)
        {
            double atmosphericAbsorption = 0.0;
            if ((molecularWeightRetained > 0.0) && (molecularWeightRetained < 10.0)) atmosphericAbsorption += 3.0;
            if ((molecularWeightRetained >= 10.0) && (molecularWeightRetained < 20.0)) atmosphericAbsorption += 2.34;
            if ((molecularWeightRetained >= 20.0) && (molecularWeightRetained < 30.0)) atmosphericAbsorption += 1.0;
            if ((molecularWeightRetained >= 30.0) && (molecularWeightRetained < 45.0)) atmosphericAbsorption += 0.15;
            if ((molecularWeightRetained >= 45.0) && (molecularWeightRetained < 100.0)) atmosphericAbsorption += 0.05;
            if (surfacePressure >= (70.0*PhysicalConstants.EarthSurfacePressureInMillibars))
                atmosphericAbsorption *= 8.333;
            else if (surfacePressure >= (50.0*PhysicalConstants.EarthSurfacePressureInMillibars))
                atmosphericAbsorption *= 6.666;
            else if (surfacePressure >= (30.0*PhysicalConstants.EarthSurfacePressureInMillibars))
                atmosphericAbsorption *= 3.333;
            else if (surfacePressure >= (10.0*PhysicalConstants.EarthSurfacePressureInMillibars))
                atmosphericAbsorption *= 2.0;
            else if (surfacePressure >= (5.0*PhysicalConstants.EarthSurfacePressureInMillibars))
                atmosphericAbsorption *= 1.5;
            return (atmosphericAbsorption);
        }

        /// <summary>
        /// Calculates the fraction of planet surface covered by water.
        /// This function is Fogg's eq.22. Uses the volatile gas inventory
        /// (a dimensionless quantity, calibrated to Earth==1000) and
        /// planetary radius in Km.
        /// I [Matt] have changed the function very slightly:  the fraction of Earth's
        /// surface covered by water is 71%, not 75% as Fogg used.
        /// </summary>
        /// <param name="radius">The equatorial radius in Earth radii.</param>
        /// <param name="volatileGasInventory">The volatile gas inventory.</param>
        /// <returns>The hydrographic coverage fraction.</returns>
        public static double ComputeHydrographicCoverage(double radius, double volatileGasInventory)
        {
            double coverage;

            if (radius == 0.0) return 0.0;
            coverage = (0.71*volatileGasInventory/1000.0)*
                       Math.Pow(PhysicalConstants.EarthRadiusInKm/radius, 2.0);
            if (coverage >= 1.0)
                return (1.0);
            else
                return (coverage);
        }

        /// <summary>
        /// Given the surface temperature of a Planet (in Kelvin), this function
        /// returns the fraction of cloud cover available.  This is Fogg's eq.23.
        /// See Hart in "Icarus" (vol 33, pp23 - 39, 1978) for an explanation.
        /// This equation is Hart's eq.3.
        /// I [Matt] have modified it slightly using constants and relationships from
        /// Glass's book "Introduction to Planetary Geology", p.46.
        /// The 'CLOUD_COVERAGE_FACTOR' is the amount of surface area on Earth
        /// covered by one Kg. of cloud.
        /// </summary>
        /// <returns>The cloud coverage fraction.</returns>
        public static double ComputeCloudCoverage(double radius, double surfacePressure, double surfaceTemperature,
                                           double molecularWeightRetained, double hydrographicCoverage)
        {
            double waterVaporInKg, fraction, surfaceArea, hydroMass;

            if (radius == 0.0)
                return 0.0;
            if (surfacePressure == 0.0)
                return 0.0;
            if (molecularWeightRetained > PhysicalConstants.WaterVapor)
                return (0.0);

            surfaceArea = 4.0*Math.PI*(radius*radius);
            hydroMass = hydrographicCoverage*surfaceArea*PhysicalConstants.EarthWaterMassPerArea;
            if (hydroMass <= 0.0)
                return 0.0;
            // log is used to reduce chance of overflow, which had happened
            // in some previous implementations on some systems.
            waterVaporInKg = (0.00000001*hydroMass)* 
                Math.Exp(PhysicalConstants.Q2_36*(surfaceTemperature - PhysicalConstants.EarthAverageTempeartureInKelvin));
            fraction = PhysicalConstants.CloudCoverageFactor * waterVaporInKg / surfaceArea;
            if (fraction >= 1.0)
                return (1.0);
            else
                return fraction;
        }

        /// <summary>
        /// Given the surface temperature of a Planet (in Kelvin), this function
        /// returns the fraction of the Planet's surface covered by ice.  This is
        /// Fogg's eq.24.  See Hart[24] in Icarus vol.33, p.28 for an explanation.
        /// I [Matt] have changed a constant from 70 to 90 in order to bring it more in
        /// line with the fraction of the Earth's surface covered with ice, which
        /// is approximatly .016 (=1.6%).
        /// </summary>
        /// <returns>The ice coverage fraction.</returns>
        public static double ComputeIceCoverage(double surfaceTemperature, double hydrographicCoverage)
        {
            double temp = surfaceTemperature;
            if (temp > 328.0)
                temp = 328.0;
            temp = Math.Pow(((328.0 - temp)/90.0), 5.0);
            if (temp > (1.5*hydrographicCoverage))
                temp = (1.5*hydrographicCoverage);
            if (temp >= 1.0)
                temp = 1.0;
            return (temp);
        }

        /// <summary>
        /// Calculates an approximation of the surface temperature, using the given
        /// quantities. For best result, this value should be recomputed iteratively.
        /// </summary>
        /// <param name="orbitalRadius">The orbitalRadius radius in Km.</param>
        /// <param name="ecosphereRadius">The ecosphere radius in AU.</param>
        /// <param name="albedo">The albedo.</param>
        /// <param name="surfacePressure">The surface pressure in milliBar.</param>
        /// <param name="molecularWeightRetained">The molecular weight retained.</param>
        /// <returns>The surface temperature, in degrees Kelvin.</returns>
        public static double ComputeSurfaceTemperature(
            double orbitalRadius,
            double ecosphereRadius,
            double albedo,
            double surfacePressure,
            double molecularWeightRetained)
        {
            double blackbodyTemperature = ComputeBlackbodyTemperature(orbitalRadius, ecosphereRadius, albedo);
            double absorptionFactor = ComputeAbsorptionFactor(surfacePressure, molecularWeightRetained);
            double greenhouseRiseInTemperature = ComputeGreenhouseRiseInTemperature(surfacePressure,
                absorptionFactor, blackbodyTemperature);

            return blackbodyTemperature + greenhouseRiseInTemperature;

        }

        /// <summary>
        /// Calculates the albedo of the planet, which is the fraction of light
        /// reflected rather than absorbed. The cloud adjustment is the fraction 
        /// of cloud cover obscuring each of the three major components of albedo
        /// that lie below the clouds. 
        /// </summary>
        /// <param name="hydrographicCoverage">The fraction of the surface covered by water.</param>
        /// <param name="cloudCoverage">The fraction of the planet covered by clouds.</param>
        /// <param name="iceCoverage">The fraction of the planet covered by ice.</param>
        /// <param name="rockAlbedo">The rock albedo.</param>
        /// <param name="cloudAlbedo">The cloud albedo.</param>
        /// <param name="iceAlbedo">The ice albedo.</param>
        /// <param name="waterAlbedo">The water albedo.</param>
        /// <param name="surfacePressure">The surface pressure in millibar.</param>
        /// <returns></returns>
        public static double ComputeAlbedo(double hydrographicCoverage, double cloudCoverage, double iceCoverage,
            double rockAlbedo, double cloudAlbedo, double iceAlbedo, double waterAlbedo,
            double surfacePressure)
        {
            double rockCoverage,
                   cloudAdjustment,
                   components,
                   cloudContribution,
                   rockContribution,
                   waterContribution,
                   iceContribution;

            rockCoverage = 1.0 - hydrographicCoverage - iceCoverage;
            components = 0.0;

            if (hydrographicCoverage > 0.0)
                components += 1.0;
            if (iceCoverage > 0.0)
                components += 1.0;
            if (rockCoverage > 0.0)
                components += 1.0;

            cloudAdjustment = cloudCoverage/components;

            if (rockCoverage >= cloudAdjustment)
                rockCoverage = rockCoverage - cloudAdjustment;
            else
                rockCoverage = 0.0;

            if (hydrographicCoverage > cloudAdjustment)
                hydrographicCoverage = hydrographicCoverage - cloudAdjustment;
            else
                hydrographicCoverage = 0.0;

            if (iceCoverage > cloudAdjustment)
                iceCoverage = iceCoverage - cloudAdjustment;
            else
                iceCoverage = 0.0;

            cloudContribution = cloudCoverage*cloudAlbedo;
            rockContribution = rockCoverage*rockAlbedo;
            iceContribution = iceCoverage*iceAlbedo;
            waterContribution = (surfacePressure == 0.0) ? 0.0 : (hydrographicCoverage*waterAlbedo);

            return (cloudContribution + rockContribution + waterContribution + iceContribution);
        }


        /// <summary>
        /// Estimates length of the planet's day.
        /// Fogg's information for this routine came from Dole "Habitable Planets
        /// for Man", Blaisdell Publishing Company, NY, 1964.  From this, he came
        /// up with his eq.12, which is the equation for the 'base_angular_velocity'
        /// below.  He then used an equation for the change in angular velocity per
        /// time (dw/dt) from P. Goldreich and S. Soter's paper "Q in the Solar
        /// System" in Icarus, vol 5, pp.375-389 (1966).  Using as a comparison the
        /// change in angular velocity for the Earth, Fogg has come up with an
        /// approximation for our new Planet (his eq.13) and take that into account.
        /// This is used to find 'change_in_angular_velocity' below.
        /// </summary>
        /// <param name="mass">Mass in Earth masses.</param>
        /// <param name="radius">Equatorial radius in kilometers.</param>
        /// <param name="eccentricity">The eccentricity.</param>
        /// <param name="density">Average planetary density in grams per cubic centimeter.</param>
        /// <param name="orbitalRadius">Semi-major axis of orbit in AU.</param>
        /// <param name="orbitalPeriod">Orbital period (year) in Earth days.</param>
        /// <param name="stellarMass">Mass of the primary star in this system.</param>
        /// <param name="stellarAge">The stellar age.</param>
        /// <param name="isGasGiant"><c>true</c> if this planet is a gas giant; <c>false</c> otherwise.</param>
        /// <param name="resonance">The resonance value.</param>
        /// <returns>Length of day in Earth hours.</returns>
        public static double ComputeDayLength(
            double mass,
            double radius,
            double eccentricity,
            double density,
            double orbitalRadius,
            double orbitalPeriod,
            double stellarMass,
            double stellarAge,
            bool isGasGiant,
            out double resonance)
        {
            double baseAngularVelocity,
                   planetMassInGrams,
                   k2,
                   angularVelocity,
                   equatorialRadiusInCm,
                   changeInAngularVelocity,
                   spinResonanceFactor,
                   yearInHours,
                   dayInHours;
            bool stopped = false;

            resonance = 0.0;

            if (isGasGiant)
                k2 = 0.24;
            else
                k2 = 0.33;

            planetMassInGrams = mass * PhysicalConstants.EarthMassInGrams;
            equatorialRadiusInCm = radius * PhysicalConstants.CmPerKm;
            dayInHours = yearInHours = orbitalPeriod * 24.0;

            if (mass == 0.0)
                return yearInHours;
            if (radius == 0.0)
                return yearInHours;

            baseAngularVelocity = Math.Sqrt(2.0 * PhysicalConstants.J * (planetMassInGrams) /
                                            (k2 * equatorialRadiusInCm * equatorialRadiusInCm));

            //  This next calculation determines how much the Planet's rotation is
            //  slowed by the presence of the star.
            changeInAngularVelocity = PhysicalConstants.ChangeInEarthAngularVelocity *
                                      (density / PhysicalConstants.EarthDensity) *
                                      (equatorialRadiusInCm / PhysicalConstants.EarthRadius) *
                                      (PhysicalConstants.EarthMassInGrams /planetMassInGrams) *
                                      (stellarMass * stellarMass) *
                                      (1.0 / Math.Pow(orbitalRadius, 6.0));
            angularVelocity = baseAngularVelocity + (changeInAngularVelocity * stellarAge);
            // Now we change from rad/sec to hours/rotation.			    
            if (angularVelocity <= 0.0)
                stopped = true;
            else
                dayInHours = PhysicalConstants.RadiansPerRotation / (PhysicalConstants.SecondsPerHour * angularVelocity);
            if ((dayInHours >= yearInHours) || stopped)
            {
                resonance = 1.0; // had been only w/large eccentricity, but...
                if (eccentricity > 0.1)
                {
                    spinResonanceFactor = (1.0 - eccentricity) / (1.0 + eccentricity);
                    return (spinResonanceFactor * yearInHours);
                }
                else
                    return (yearInHours);
            }
            return (dayInHours);
        }

        /// <summary>
        /// Computes the orbital period of a small celestial object given its separation from 
        /// the large celestial object
        /// </summary>
        /// <param name="separation">The separation between the two bodies, in AU.</param>
        /// <param name="largeMass">Mass of larger body in Solar masses.</param>
        /// <param name="smallMass">Mass of smaller body in Solar masses.</param>
        /// <returns></returns>
        public static double ComputeOrbitalPeriod(double separation, double largeMass, double smallMass)
        {
            return Math.Sqrt(Math.Pow(separation, 3) / (smallMass + largeMass)) * PhysicalConstants.DaysPerYear;
        }

        /// <summary>
        /// Calculates the equatorial radius of the planet given mass, 'zone', and
        /// whether it's a gas giant or not.
        /// This formula is listed as eq.9 in Fogg's article, although some typos
        /// crop up in that eq.  See "The Internal Constitution of Planets", by
        /// Dr. D. S. Kothari, Mon. Not. of the Royal Astronomical Society, vol 96
        /// pp.833-843, 1936 for the derivation.  Specifically, this is Kothari's
        /// eq.23, which appears on page 840.
        /// </summary>
        /// <param name="mass">Mass in Earth masses.</param>
        /// <param name="isGasGiant"><c>true</c> if planet is a gas giant, <c>false</c> otherwise.</param>
        /// <param name="zone">Orbital zone, 1 to 3.</param>
        /// <returns>Equatorial radius in kilometers</returns>
        public static double ComputeKothariRadius(double mass, bool isGasGiant, int zone)
        {
            double temp, temp2, atomicWeight, atomicNum;

            if (mass == 0.0) return 0.0;  // for grit belts (see constructor, mass_by_integration)

            if (zone == 1)
            {
                if (isGasGiant)
                {
                    atomicWeight = 9.5;
                    atomicNum = 4.5;
                }
                else
                {
                    atomicWeight = 15.0;
                    atomicNum = 8.0;
                }
            }
            else if (zone == 2)
            {
                if (isGasGiant)
                {
                    atomicWeight = 2.47;
                    atomicNum = 2.0;
                }
                else
                {
                    atomicWeight = 10.0;
                    atomicNum = 5.0;
                }
            }
            else
            {
                if (isGasGiant)
                {
                    atomicWeight = 7.0;
                    atomicNum = 4.0;
                }
                else
                {
                    atomicWeight = 10.0;
                    atomicNum = 5.0;
                }
            }

            temp = (2.0 * PhysicalConstants.BETA_20 *
                Math.Pow(PhysicalConstants.EarthMassInGrams, (1.0 / 3.0))) /
                (PhysicalConstants.A1_20 * Math.Pow(atomicWeight * atomicNum, (1.0 / 3.0)));

            temp2 = PhysicalConstants.A2_20 * Math.Pow(atomicWeight, (4.0 / 3.0)) *
                Math.Pow(PhysicalConstants.EarthMassInGrams, (2.0 / 3.0));
            temp2 *= Math.Pow(mass, (2.0 / 3.0));
            temp2 /= (PhysicalConstants.A1_20 * atomicNum * atomicNum);
            temp2 = 1.0 + temp2;
            temp /= temp2;
            temp *= Math.Pow(mass, (1.0 / 3.0)) / (PhysicalConstants.CmPerKm);

            // Make Earth actual earth.
            temp /= 1.003864;
            return temp;
        }

        static int ComputeOrbitalZone(double orbitalRadius, double stellarLuminosity)
        {
            if (orbitalRadius < (4.0 * Math.Sqrt(stellarLuminosity)))
                return 1;
            else
            {
                if ((orbitalRadius >= (4.0 * Math.Sqrt(stellarLuminosity))) &&
                    (orbitalRadius < (15.0 * Math.Sqrt(stellarLuminosity))))
                    return 2;
                else
                    return 3;
            }
        }

        /* function for 'soft limiting' temperatures */

        static double Lim(double x)
        {
            //double test =  x / Math.Sqrt(Math.Sqrt(1 + x*x*x*x));
            return x/Math.Pow(1.0 + x*x*x*x, 1.0/4.0);
        }

        static double Soft(double v, double max, double min)
        {
            double dv = v - min;
            double dm = max - min;
            return (Lim(2*dv/dm - 1) + 1)/2*dm + min;
        }
    }
}