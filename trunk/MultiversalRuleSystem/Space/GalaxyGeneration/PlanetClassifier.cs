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
        Composition composition;
        Climate climate;

        public PlanetaryFeatures ClassifyGasGiant(CelestialFeatures celestialFeatures, StellarFeatures stellarFeatures)
        {
            this.celestialFeatures = celestialFeatures;
            this.stellarFeatures = stellarFeatures;

            size = ClassifyGasGiantSize(celestialFeatures.Mass);
            density = Density.GasGiant;
            gravity = ClassifyGravity(celestialFeatures.SurfaceGravity);
            temperature = ClassifyPlanetTemperature(celestialFeatures.SurfaceTemperature);
            climate = AssignGasGiantClimate(celestialFeatures.SurfaceTemperature);

            return new PlanetaryFeatures(size, density, gravity, temperature, AtmosphericDensity.Superdense,
                                         Composition.Unknown, climate);
        }

        public PlanetaryFeatures ClassifyPlanet(CelestialFeatures celestialFeatures, StellarFeatures stellarFeatures)
        {
            this.celestialFeatures = celestialFeatures;
            this.stellarFeatures = stellarFeatures;
            size = ClassifyPlanetSize(celestialFeatures.Radius);
            density = ClassifyDensity(celestialFeatures.Density);
            gravity = ClassifyGravity(celestialFeatures.SurfaceGravity);
            temperature = ClassifyPlanetTemperature(celestialFeatures.SurfaceTemperature);
            atmosphericDensity = ClassifyAtmosphericDensity(celestialFeatures.SurfacePressure);


            //if (density == Density.IcyCore)
            //    climate = AssignIcyCoreClimate();
            //else
            //    climate = AssignIronCoreClimate();

            if (size == PlanetSize.Tiny)
                climate = AssignPlanetoidalClass();
            else 
                climate = AssignIronCoreClimate();

            return new PlanetaryFeatures(size, density, gravity, temperature, atmosphericDensity, Composition.Unknown, climate);
        }

        
        public string Report
        {
            get
            {
                StringBuilder sb = new StringBuilder();
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

        static PlanetSize ClassifyGasGiantSize(double mass)
        {
            // mass in JupiterMasses
            double massInJm = mass/PhysicalConstants.JupiterMassInEarthMasses;

            if (massInJm <= 0.7)
                return PlanetSize.SubJovian;
            else if (massInJm <= 3.5)
                return PlanetSize.Jovian;
            else
                return PlanetSize.SuperJovian;
        }

        static PlanetSize ClassifyPlanetSize(double radius)
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
            else if (pressure <= 0.2)
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


        static Climate AssignGasGiantClimate(double exosphericTemperature)
        {
            if (exosphericTemperature > 1500)
                return Climate.HyperThermicJovian;
            else if (exosphericTemperature >= 900 && exosphericTemperature < 1500)
                return Climate.EpistellarJovian;
            else if (exosphericTemperature >= 350 && exosphericTemperature < 900)
                return Climate.AzurianJovian;
            else if (exosphericTemperature >= 150 && exosphericTemperature < 350)
                return Climate.HydroJovian;
            else if (exosphericTemperature >= 50 && exosphericTemperature <= 150)
                return Climate.Jovian;
            else
                return Climate.CryoJovian;


        }

        static Temperature ClassifyPlanetTemperature(double surfaceTemperature)
        {
            if (surfaceTemperature >= 345)
                return Temperature.Extreme;
            if (surfaceTemperature >= 315 && surfaceTemperature < 345)
                return Temperature.VeryHot;
            else if (surfaceTemperature >= 250 && surfaceTemperature < 315)
                return Temperature.Temperate;
            else if (surfaceTemperature >= 215 && surfaceTemperature < 250)
                return Temperature.Cold;
            else if (surfaceTemperature >= 140 && surfaceTemperature < 215)
                return Temperature.Icy;
            else if (surfaceTemperature < 140)
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

        private Climate AssignPanthalassicClass()
        {
            switch (atmosphericDensity)
            {
                case AtmosphericDensity.None:
                case AtmosphericDensity.Traces:

                    switch (temperature)
                    {
                        case Temperature.Extreme:
                            return Climate.Cataclismic;

                        default:
                            if (celestialFeatures.IceCoverage >= 0.10)
                                return Climate.Glacial;
                            else
                                return Climate.Arean;
                    }

                case AtmosphericDensity.Thin:
                case AtmosphericDensity.Standard:
                case AtmosphericDensity.Dense:
                case AtmosphericDensity.Superdense:
                    if (celestialFeatures.HydrographicCoverage >= 0.05)
                        return Climate.Ocean;
                    else if (celestialFeatures.IceCoverage >= 0.10)
                        return Climate.Glacial;
                    else
                        return Climate.Arean;

                default:
                    return Climate.Unknown;


            }

        }


        Climate AssignPlanetoidalClass()
        {
            switch (temperature)
            {
                case Temperature.Frozen:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:

                            if (celestialFeatures.IceCoverage >= 0.10 && density == Density.IcyCore)
                                return Climate.Kuiperian;
                            else if (celestialFeatures.SurfaceTemperature <= 80.0)
                                return Climate.Hadean;
                            else
                                return Climate.Cerean;

                        default:
                            return Climate.Unknown;
                    }


                case Temperature.Extreme:
                    return Climate.Hephaestian;

                default:
                    if (celestialFeatures.IceCoverage >= 0.10 && density == Density.IcyCore)
                        return Climate.Kuiperian;
                    else
                        return Climate.Cerean;


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
                            return Climate.Hadean;

                        default:
                            return Climate.Glacial;
                    }

                case Temperature.Icy:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:
                        
                            if (celestialFeatures.IceCoverage >= 0.10)
                                return density == Density.IcyCore ? Climate.Kuiperian : Climate.Glacial;
                            else
                                return Climate.Arean;

                        case AtmosphericDensity.Thin:
                        case AtmosphericDensity.Standard:
                        case AtmosphericDensity.Dense:
                        case AtmosphericDensity.Superdense:
                            if (stellarFeatures.Mass <= 0.65 &&
                                 (celestialFeatures.MinTemp >= 140))
                                return Climate.Ammonia;
                            if (celestialFeatures.IceCoverage >= 0.10)
                                return Climate.Glacial;
                            else
                                return Climate.Arean;

                        default:
                            return Climate.Unknown;
                    }

                case Temperature.Cold:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:
                            if (celestialFeatures.IceCoverage >= 0.10)
                                return Climate.Glacial;
                            else
                                return Climate.Arean;

                        default:
                            if (celestialFeatures.SurfaceTemperature >= 230)
                                return Climate.Tundra;
                            else
                                return Climate.Glacial;
                    }

                case Temperature.Temperate:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                            return Climate.Arean;

                        case AtmosphericDensity.Traces:
                            if (Dice.Roll() <= CataclismicChance)
                                return Climate.Cataclismic;
                            else if (celestialFeatures.HydrographicCoverage >= 0.1)
                                return Climate.Arid;
                            else
                                return Climate.Arean;

                        case AtmosphericDensity.Thin:
                        case AtmosphericDensity.Standard:
                        case AtmosphericDensity.Dense:
                            return AssignEogaianClimate();

                        case AtmosphericDensity.Superdense:
                            if (Dice.Roll() <= CataclismicChance)
                                return Climate.Cataclismic;
                            else
                                return celestialFeatures.HydrographicCoverage > 0 ? Climate.Pelagic : Climate.Cytherean; ;

                        default:
                            return Climate.Unknown;

                    }

                case Temperature.VeryHot:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:                     
                            return Climate.Arean;

                        case AtmosphericDensity.Thin:
                        case AtmosphericDensity.Standard:
                        case AtmosphericDensity.Dense:
                            if (celestialFeatures.HydrographicCoverage <= 0.2)
                                return Climate.Desert;
                            else if (celestialFeatures.HydrographicCoverage > 0.2 &&
                                celestialFeatures.HydrographicCoverage <= (2.0 / 3.0))
                                return Climate.Arid;
                            else
                                return Climate.Ocean;


                        case AtmosphericDensity.Superdense:
                            if (Dice.Roll() <= CataclismicChance)
                                return Climate.Cataclismic;
                            else
                                return celestialFeatures.HydrographicCoverage > 0 ? Climate.Pelagic : Climate.Cytherean; 

                        default:
                            return Climate.Unknown;
                    }

                case Temperature.Extreme:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:
                        case AtmosphericDensity.Thin:
                            return Climate.Hephaestian;

                        case AtmosphericDensity.Standard:
                        case AtmosphericDensity.Dense:
                        case AtmosphericDensity.Superdense:
                            if (Dice.Roll() <= CataclismicChance)
                                return Climate.Hephaestian;
                            else
                                return celestialFeatures.HydrographicCoverage > 0? Climate.Pelagic : Climate.Cytherean;

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

            if (celestialFeatures.HydrographicCoverage >= 2.0/3.0)
                return Climate.Ocean;
            if (celestialFeatures.HydrographicCoverage >= 0.5 && celestialFeatures.HydrographicCoverage <= 2.0 / 3.0)
                return Climate.Terran;
            else if (celestialFeatures.HydrographicCoverage < 0.5 && celestialFeatures.HydrographicCoverage > 0.2)
            {
                return Climate.Arid;
            }
            else if (celestialFeatures.HydrographicCoverage <= 0.0)
                return celestialFeatures.SurfaceTemperature >= 273.0 ? Climate.Desert : Climate.Tundra;


            return Climate.Unknown;
        }


    }
}
