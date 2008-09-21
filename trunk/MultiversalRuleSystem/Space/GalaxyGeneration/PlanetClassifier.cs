using System.Text;
using AvengersUtd.MultiversalRuleSystem.Space;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.MultiversalRuleSystem;

namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{
    public class PlanetClassifier
    {
        const float CataclismicChance = 0.1f;

        CelestialFeatures primaryBodyFeatures;

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
                                         Composition.Hydrogen, climate);
        }

        public PlanetaryFeatures ClassifyMoon(CelestialFeatures celestialFeatures, StellarFeatures stellarFeatures, CelestialFeatures primaryBodyFeatures)
        {
            this.primaryBodyFeatures = primaryBodyFeatures;
            this.celestialFeatures = celestialFeatures;
            this.stellarFeatures = stellarFeatures;

            ClassifyParameters();

            return new PlanetaryFeatures(size, density, gravity, temperature, atmosphericDensity, composition, climate);
        }

        public PlanetaryFeatures ClassifyPlanet(CelestialFeatures celestialFeatures, StellarFeatures stellarFeatures)
        {
            this.primaryBodyFeatures = null;
            this.celestialFeatures = celestialFeatures;
            this.stellarFeatures = stellarFeatures;

            ClassifyParameters();

            return new PlanetaryFeatures(size, density, gravity, temperature, atmosphericDensity, composition, climate);
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
                sb.AppendLine(string.Format("Composition: {0}", composition));

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


        void ClassifyParameters()
        {
            size = ClassifyPlanetSize(celestialFeatures.Radius);
            density = ClassifyDensity(celestialFeatures.Density);
            gravity = ClassifyGravity(celestialFeatures.SurfaceGravity);
            temperature = ClassifyPlanetTemperature(celestialFeatures.SurfaceTemperature);
            atmosphericDensity = ClassifyAtmosphericDensity(celestialFeatures.SurfacePressure);
            
            
            if (size == PlanetSize.Tiny)
                climate = AssignPlanetoidalClass();
            else if (size == PlanetSize.Huge || size == PlanetSize.Immense)
                climate = AssignSuperEarthClass();
            else
                climate = AssignIronCoreClimate();

            composition = ClassifyAtmosphericComposition(climate, atmosphericDensity,
                celestialFeatures.Atmosphere);
        }

        static Composition ClassifyAtmosphericComposition(Climate climate, AtmosphericDensity atmosphericDensity, Gas[] gases)
        {
            switch (climate)
            {
                case Climate.Arean:
                case Climate.Selenian:
                case Climate.Ferrinian:
                case Climate.Volcanic:
                case Climate.Hadean:
                case Climate.Kuiperian:
                case Climate.Hephaestian:
                case Climate.Cerean:
                    return Composition.Vacuum;

                case Climate.Desert:
                case Climate.Tundra:
                case Climate.Terran:
                case Climate.Arid:
                    return Composition.Oxygen;

                case Climate.Ammonia:
                    return Composition.Ammonia;

                case Climate.Ocean:
                case Climate.Pelagic:
                    return Composition.Nitrogen;

                case Climate.Glacial:
                case Climate.Cataclismic:
                    return (int) atmosphericDensity > (int) AtmosphericDensity.Traces
                               ? Composition.CarbonDioxide
                              : Composition.Vacuum;

                case Climate.Cytherean:
                    return Composition.CarbonDioxide;

                case Climate.Titanian:
                    return Composition.Methane;

                default:
                    return Composition.Unknown;
            
            }
        }

        public static PlanetSize ClassifyGasGiantSize(double mass)
        {
            // mass in JupiterMasses
            double massInJm = mass/PhysicalConstants.JupiterMassInEarthMasses;

            if (massInJm <= 0.2)
                return PlanetSize.Neptunian;
            else if (massInJm <= 0.7)
                return PlanetSize.SubJovian;
            else if (massInJm <= 3.5)
                return PlanetSize.Jovian;
            else if (massInJm <= 7.5)
                return PlanetSize.SuperJovian;
            else
                return PlanetSize.MacroJovian;

        }

        public static PlanetSize ClassifyPlanetSize(double radius)
        {
            if (radius <= 1750.0)
                return PlanetSize.Tiny;
            else if (radius <= 2500.0)
                return PlanetSize.Small;
            else if (radius <= 5000.0)
                return PlanetSize.Medium;
            else if (radius <= 8000.0)
                return PlanetSize.Large;
            else if (radius <= 12500.0)
                return PlanetSize.Huge;
            else
                return PlanetSize.Immense;
        }

        public static Density ClassifyDensity(double density)
        {
            density /= PhysicalConstants.EarthDensity;
            if (density <= 0.65)
                return Density.IcyCore;
            else if (density <= 1.20)
                return Density.SmallIronCore;
            else
                return Density.LargeIronCore;
        }


        public static Gravity ClassifyGravity(double gravity)
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

        public static AtmosphericDensity ClassifyAtmosphericDensity(double pressure)
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


        static Climate AssignGasGiantClimate(double surfaceTemperature)
        {
            if (surfaceTemperature > 1500)
                return Climate.HyperthermicJovian;
            else if (surfaceTemperature >= 900 && surfaceTemperature < 1500)
                return Climate.EpistellarJovian;
            else if (surfaceTemperature >= 350 && surfaceTemperature < 900)
                return Climate.AzurianJovian;
            else if (surfaceTemperature >= 150 && surfaceTemperature < 350)
                return Climate.HydroJovian;
            else if (surfaceTemperature >= 50 && surfaceTemperature <= 150)
                return Climate.EuJovian;
            else
                return Climate.CryoJovian;


        }

        public static Temperature ClassifyPlanetTemperature(double surfaceTemperature)
        {
            if (surfaceTemperature >= 400)
                return Temperature.Extreme;
            if (surfaceTemperature >= 315 && surfaceTemperature < 400)
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

        }

        private Climate AssignSuperEarthClass()
        {
            switch (atmosphericDensity)
            {
                case AtmosphericDensity.None:
                case AtmosphericDensity.Traces:

                    switch (temperature)
                    {
                        case Temperature.Extreme:
                            if (Dice.Roll() <= CataclismicChance)
                                return Climate.Cataclismic;
                            else
                                return celestialFeatures.SurfaceTemperature > 700 ? Climate.Hephaestian : Climate.Arean;

                        default:
                            if (celestialFeatures.IceCoverage >= 0.50)
                                return Climate.Glacial;
                            else
                                return Climate.Arean;
                    }

                case AtmosphericDensity.Thin:
                case AtmosphericDensity.Standard:
                case AtmosphericDensity.Dense:

                    if (celestialFeatures.HydrographicCoverage >= 0.50)
                        return Climate.Ocean;
                    else if (celestialFeatures.IceCoverage >= 0.50)
                        return Climate.Glacial;
                    else if (celestialFeatures.HydrographicCoverage >= 0.1)
                        return Climate.Arid;
                    else return Climate.Arean;

                case AtmosphericDensity.Superdense:
                    switch (temperature)
                    {
                        default:
                            return Climate.Unknown;

                        case Temperature.Frozen:
                        case Temperature.Icy:
                        case Temperature.Cold:
                            if (celestialFeatures.IceCoverage >= 0.50)
                                return Climate.Glacial;
                            else
                                return Climate.Arean;
       
                        case Temperature.Temperate:
                            if (celestialFeatures.HydrographicCoverage > 0.05 &&
                                celestialFeatures.HydrographicCoverage <= 0.50)
                                return Climate.Arid;
                            else if (celestialFeatures.HydrographicCoverage > 0.50)
                                return Climate.Ocean;
                            else
                                return (celestialFeatures.HydrographicCoverage == 0 && 
                                    celestialFeatures.SurfaceTemperature >= 273) ? Climate.Desert : Climate.Tundra;

                            
                        case Temperature.VeryHot:
                        case Temperature.Extreme:
                            return celestialFeatures.HydrographicCoverage > 0 ? Climate.Pelagic : Climate.Cytherean;
                            
                    }
                    
                    

                default:
                    return Climate.Unknown;
            }

        }


        Climate AssignPlanetoidalClass()
        {
            double tidalForces = ComputeTidalForces();
            
            switch (temperature)
            {
                case Temperature.Frozen:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                            
                            if (tidalForces >= 0.5)
                                    return Climate.Volcanic;
                            else if (celestialFeatures.IceCoverage >= 0.50 && density == Density.IcyCore)
                                return Climate.Kuiperian; 
                            else if (celestialFeatures.SurfaceTemperature <= 80.0)
                                return Climate.Hadean;
                            else 
                                return Climate.Cerean;

                        default:
                            return Climate.Unknown;
                    }


                case Temperature.Extreme:
                    return celestialFeatures.SurfaceTemperature <= 700 && density == Density.LargeIronCore
                               ? Climate.Ferrinian
                               : (celestialFeatures.SurfaceTemperature > 700) ? Climate.Hephaestian : Climate.Cerean;

                default:
                    if (celestialFeatures.IceCoverage >= 0.50 && density == Density.IcyCore)
                        return Climate.Kuiperian;
                    else
                        return Climate.Cerean;


            }
        }

        double ComputeTidalForces()
        {
            double primaryMass, primaryOrbitalRadius;
            if (primaryBodyFeatures != null)
            {
                primaryMass = primaryBodyFeatures.Mass;
                primaryOrbitalRadius = primaryBodyFeatures.SemiMajorAxis;
            }
            else
            {
                primaryMass = stellarFeatures.Mass* PhysicalConstants.SolarMassInEarthMasses;
                primaryOrbitalRadius = 0;
            }

            return PlanetGenerator.ComputeTidalForcesOnSatelliteIo(
                                    primaryMass, celestialFeatures.Mass, celestialFeatures.Radius,
                                    celestialFeatures.SemiMajorAxis - primaryOrbitalRadius);
        }

        Climate AssignIronCoreClimate()
        {
            double tidalForces = ComputeTidalForces();

            switch (temperature)
            {
                case Temperature.Frozen:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                            // If planet is a moon
                            if (tidalForces >= 0.5)
                                return Climate.Volcanic;
                            else
                                return Climate.Hadean;

                        default:
                            if (tidalForces >= 0.1)
                                return Climate.Titanian;
                            if (celestialFeatures.IceCoverage >= 0.50)
                                return Climate.Glacial;
                            else
                                return Climate.Arean;
                               
                    }

                case Temperature.Icy:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:
                        
                            if (celestialFeatures.IceCoverage >= 0.50)
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
                            if (celestialFeatures.IceCoverage >= 0.50)
                                return Climate.Glacial;
                            else return Climate.Arean;

                        default:
                            return Climate.Unknown;
                    }

                case Temperature.Cold:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:
                            if (celestialFeatures.IceCoverage >= 0.50)
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
                            switch (density)
                            {
                                case Density.IcyCore:
                                    return Climate.Selenian;
                                case Density.LargeIronCore:
                                    return Climate.Ferrinian;
                                default:
                                    return Climate.Arean;
                            }

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
                                return celestialFeatures.HydrographicCoverage > 0 ? Climate.Ocean : Climate.Unknown;

                        default:
                            return Climate.Unknown;

                    }

                case Temperature.VeryHot:
                    switch (atmosphericDensity)
                    {
                        case AtmosphericDensity.None:
                        case AtmosphericDensity.Traces:
                            switch (density)
                            {
                                case Density.IcyCore:
                                    return Climate.Selenian;
                                case Density.LargeIronCore:
                                    return Climate.Ferrinian;
                                default:
                                    return Climate.Arean;
                            }

                        case AtmosphericDensity.Thin:
                        case AtmosphericDensity.Standard:
                        case AtmosphericDensity.Dense:
                            if (celestialFeatures.HydrographicCoverage <= 0.25)
                                return Climate.Desert;
                            else if (celestialFeatures.HydrographicCoverage > 0.25 &&
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
                            if (density == Density.LargeIronCore && celestialFeatures.SurfaceTemperature <= 700)
                                return Climate.Ferrinian;
                            else
                                return celestialFeatures.SurfaceTemperature > 700 ? Climate.Hephaestian : Climate.Arean;

                        case AtmosphericDensity.Standard:
                        case AtmosphericDensity.Dense:
                        case AtmosphericDensity.Superdense:
                            if (Dice.Roll() <= CataclismicChance)
                                return Climate.Cataclismic;
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

            if (celestialFeatures.HydrographicCoverage >= 0.7)
                return Climate.Ocean;
            if (celestialFeatures.HydrographicCoverage >= 0.5 && celestialFeatures.HydrographicCoverage <= 0.7)
                return Climate.Terran;
            else if (celestialFeatures.HydrographicCoverage < 0.5 && celestialFeatures.HydrographicCoverage > 0.2)
            {
                return Climate.Arid;
            }
            else if (celestialFeatures.HydrographicCoverage == 0.0)
                return celestialFeatures.SurfaceTemperature >= 273.0 ? Climate.Desert : Climate.Tundra;


            return Climate.Unknown;
        }


    }
}
