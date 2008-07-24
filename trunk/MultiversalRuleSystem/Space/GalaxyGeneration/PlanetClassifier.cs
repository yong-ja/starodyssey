using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.MultiversalRuleSystem.Space;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.MultiversalRuleSystem;

namespace AvengersUTD.MultiversalRuleSystem.Space.GalaxyGeneration
{
    public class PlanetClassifier
    {
        const float CataclismicChance = 0.1f;

        CelestialFeatures celestialFeatures;
        StellarFeatures stellarFeatures;
        PlanetSize size;
        Density density;
        Temperature temperature;
        Gravity gravity;
        AtmosphericDensity atmosphericDensity;
        Climate climate;

        public void Classify(CelestialFeatures celestialFeatures, StellarFeatures stellarFeatures)
        {
            this.celestialFeatures = celestialFeatures;
            this.stellarFeatures = stellarFeatures;
            size = ClassifySize(celestialFeatures.Radius);
            density = ClassifyDensity(celestialFeatures.Density);
            gravity = ClassifyGravity(celestialFeatures.SurfaceGravity);
            temperature = ClassifyTemperature(celestialFeatures.SurfaceTemperature, celestialFeatures.MinTemp, celestialFeatures.MaxTemp);
            atmosphericDensity = ClassifyAtmosphericDensity(celestialFeatures.SurfacePressure);


            //if (density == Density.IcyCore)
            //    climate = AssignIcyCoreClimate();
            //else
            //    climate = AssignIronCoreClimate();

            if (size == PlanetSize.Tiny)
                climate = AssignPlanetoidalClass();
            else
                climate = AssignIronCoreClimate();
        }

        public string Report
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(celestialFeatures.Name);
                sb.AppendLine(string.Format("Climate: {0}", climate));
                sb.AppendLine(string.Format("Size: {0} Density: {1}", size, density));
                sb.AppendLine(string.Format("Gravity: {0} AtmDensity: {1}", gravity,atmosphericDensity));
                sb.AppendLine(string.Format("Temperature: {0}", temperature));
                sb.AppendLine("Composition:");

                if (celestialFeatures.Atmosphere != null)
                {
                    foreach (Gas g in celestialFeatures.Atmosphere)
                    {
                        double f = 100.0 * (g.Pressure / celestialFeatures.SurfacePressure);
                        sb.AppendLine(string.Format("{0}: {1:F2}", g.Symbol, f));
                    }
                }
                sb.AppendLine();

                return sb.ToString();
            }
        }

        static PlanetSize ClassifySize(double radius)
        {
            if (radius <= 1250.0)
                return PlanetSize.Tiny;
            else if (radius <= 2500.0)
                return PlanetSize.Small;
            else if (radius <= 5000.0)
                return PlanetSize.Medium;
            else if (radius <= 7500.0)
                return PlanetSize.Large;
            else if (radius <= 10000.0)
                return PlanetSize.Huge;
            else
                return PlanetSize.Immense;
        }

        static Density ClassifyDensity(double density)
        {
            density /= PhysicalConstants.EarthDensity;
            if (density <= 0.65)
                return Density.IcyCore;
            else if (density <= 1.20)
                return Density.LargeIronCore;
            else
                return Density.LargeIronCore;
        }


        static Gravity ClassifyGravity(double gravity)
        {
            if (gravity <= 0.25)
            {
                return Gravity.VeryLow;
            }
            else if (gravity <= 0.75)
            {
                return Gravity.Low;
            }
            else if (gravity <= 1.25)
            {
                return Gravity.Average;
            }
            else if (gravity <= 2.5)
            {
                return Gravity.High;
            }
            else if (gravity > 5)
            {
                return Gravity.VeryHigh;
            }

            return Gravity.None;
        }

        static AtmosphericDensity ClassifyAtmosphericDensity(double pressure)
        {
            pressure /= 1000;

            if (pressure <= 0.01)
                return AtmosphericDensity.None;
            else if (pressure <= 0.25)
                return AtmosphericDensity.Traces;
            else if (pressure <= 0.5)
                return AtmosphericDensity.Thin;
            else if (pressure <= 1.25)
                return AtmosphericDensity.Standard;
            else if (pressure <= 2.5)
                return AtmosphericDensity.Dense;
            else
                return AtmosphericDensity.Superdense;
        }

        static Temperature ClassifyTemperature(double surfaceTemperature, double minimumTemperature, double maximumTemperature)
        {
            if (surfaceTemperature > 344)
                return Temperature.Extreme;
            if (surfaceTemperature >= 290 && surfaceTemperature <= 344)
                return Temperature.VeryHot;
            else if (surfaceTemperature >= 250 && surfaceTemperature <= 340)
                return Temperature.Temperate;
            else if (surfaceTemperature >= 140 && surfaceTemperature <= 250)
                return Temperature.Cold;
            else if (surfaceTemperature <= 140)
                return Temperature.Frozen;
            else
            {
                System.Diagnostics.Debug.WriteLine("Temperature Error");
                return Temperature.Extreme;
            }


            //if (surfaceTemperature <= 240)
            //    return Temperature.Frozen;
            //else if (surfaceTemperature <= 280)
            //    return Temperature.Cold;
            //else if (surfaceTemperature <= 310)
            //    return Temperature.Temperate;
            //else if (surfaceTemperature <= 340)
            //    return Temperature.VeryHot;
            //else
            //    return Temperature.Extreme;
        }

        Climate AssignPlanetoidalClass()
        {
            switch (temperature)
            {
                case Temperature.Frozen:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                            switch (celestialFeatures.OrbitalZone)
                            {
                                case OrbitalZone.Snowline:
                                    return celestialFeatures.SurfaceTemperature > 80 ? Climate.Cerean : Climate.Hadean;

                                case OrbitalZone.Outer:
                                    return Climate.Cerean;

                                case OrbitalZone.External:
                                    return Climate.Kuiperian;

                                default:
                                    return Climate.Unknown;
                            }

                        default:
                            return Climate.Unknown;
                    }

                default:
                    switch (celestialFeatures.OrbitalZone)
                    {
                        case OrbitalZone.Snowline:
                            return Climate.Cerean;

                        default:
                            return Climate.Kuiperian;
                    }

            }
        }

        Climate AssignIcyCoreClimate()
        {
            switch (temperature)
            {
                case Temperature.Frozen:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                            return Climate.Unknown;
           
                        case AtmosphericDensity.Traces:
                        case AtmosphericDensity.Thin:
                            return Climate.Volcanic;

                        default:
                            if (stellarFeatures.Mass <= 0.65 &&
                                (celestialFeatures.MinTemp >= 140))
                                return
                                    Climate.Ammonia;
                            else
                                return Climate.Ice;
                    }

                default:
                    return Climate.Unknown;
            }
        }

        Climate AssignIronCoreClimate()
        {
            switch (temperature)
            {
                case Temperature.Frozen:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                            return Climate.Unknown;

                        default:
                            return Climate.Barren;
                    }

                case Temperature.Cold:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:
                        case AtmosphericDensity.Thin:
                            return Climate.Barren;

                        case AtmosphericDensity.Standard:
                        case AtmosphericDensity.Dense:
                        case AtmosphericDensity.Superdense:
                            return Climate.Glacial;

                        default:
                            return Climate.Unknown;
                    }

                case Temperature.Temperate:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:
                            return Climate.Barren;

                        
                        case AtmosphericDensity.Thin:
                            if (Dice.Roll() <= CataclismicChance)
                                return Climate.Cataclismic;
                            else
                                return AssignEogaianClimate();
                            
                        case AtmosphericDensity.Standard:
                        case AtmosphericDensity.Dense:
                            return AssignEogaianClimate();

                        case AtmosphericDensity.Superdense:
                            if (Dice.Roll() <= CataclismicChance)
                                return Climate.Cataclismic;
                            else
                                return Climate.Radiated;

                        default:
                            return Climate.Unknown;

                    }

                case Temperature.VeryHot:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:                     
                            return Climate.Barren;

                        case AtmosphericDensity.Thin:
                        case AtmosphericDensity.Standard:
                        case AtmosphericDensity.Dense:
                            return Climate.Desert;

                        case AtmosphericDensity.Superdense:
                            if (Dice.Roll() <= CataclismicChance)
                                return Climate.Cataclismic;
                            else
                                return Climate.Radiated;

                        default:
                            return Climate.Unknown;
                    }

                case Temperature.Extreme:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:
                        case AtmosphericDensity.Thin:
                            return Climate.Barren;

                        case AtmosphericDensity.Standard:
                        case AtmosphericDensity.Dense:
                        case AtmosphericDensity.Superdense:
                            if (Dice.Roll() <= CataclismicChance)
                                return Climate.Cataclismic;
                            else
                                return Climate.Radiated;

                        default:
                            return Climate.Unknown;

                    }

                default:
                    return Climate.Unknown;

            }
        }

        private Climate AssignEogaianClimate()
        {

            double deltaTemp = Math.Abs(Math.Abs(celestialFeatures.NightTemp) - Math.Abs(celestialFeatures.DayTemp));

            if (deltaTemp <= 25)
            {
                if (celestialFeatures.HydrographicCoverage <= 2.0 / 3.0)
                    return Climate.Terran;
                else
                    return Climate.Ocean;
            }
            else if (deltaTemp <= 50)
            {
                if (celestialFeatures.IceCoverage >= 0)
                    return Climate.Tundra;
                else
                    return Climate.Arid;
            }
            else
                return Climate.Unknown;

        }

        
    }
}
