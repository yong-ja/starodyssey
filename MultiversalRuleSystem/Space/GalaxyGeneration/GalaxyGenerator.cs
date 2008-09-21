using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration;
using AvengersUtd.Odyssey.Utils.Xml;


namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{
    public class GalaxyGenerator
    {
        const double minimumMass = 0.08;
        List<string> starNames;
        GalaxyOptions galaxyOptions;
        StarGenerator starGen;
        PlanetGenerator planetGenerator;
        PlanetClassifier planetClassifier;
        SystemValidator systemValidator;

        StringBuilder sb = new StringBuilder();

        public GalaxyGenerator()
            : this(Data.Deserialize<GalaxyOptions>(Paths.GalaxyOptionsPath))
        {
        }

        public GalaxyGenerator(GalaxyOptions galaxyOptions)
        {
            this.galaxyOptions = galaxyOptions;
            starGen = new StarGenerator(galaxyOptions);
            planetGenerator = new PlanetGenerator();
            planetClassifier = new PlanetClassifier();
            systemValidator = new SystemValidator();
            starNames = SpaceResourceManager.LoadStarNames();
        }

        /// <summary>
        /// Generates the whole galaxy.
        /// </summary>
        public SortedDictionary<string, SolarSystem> GenerateStars()
        {
            StarChances starChances = galaxyOptions.StarChances;

            if (!starChances.Check())
                throw new ArgumentException("Star chances total sum must be equal to 1.0");

            double blueDwarf = starChances[StarColor.Blue];
            double yellowDwarf = starChances[StarColor.Red];
            double orangeDwarf = starChances[StarColor.Orange];
            double redDwarf = starChances[StarColor.Red];

            int starCount = galaxyOptions.StarCount;

            starCount = 70;


            SortedDictionary<string, SolarSystem> solarSystems = new SortedDictionary<string, SolarSystem>();

            int yd, rd, bd, od, rg, bh, ns, wd, bugs;
            yd = rd = bd = od = rg = bh = ns = wd = bugs = 0;
            double averageAge = 0;

            for (int i = 0; i < starCount; i++)
            {
                double chance = Dice.Roll();
                double min, max;
                double mass;
                StellarFeatures StellarFeatures;

                // Assign min/max mass for each star type
                // F type stars
                if (chance <= blueDwarf)
                {
                    min = 2.0;
                    max = 30.0;
                } // G type stars
                else if (chance <= blueDwarf + yellowDwarf)
                {
                    min = 0.78;
                    max = 2.0;
                } // K type stars
                else if (chance <= blueDwarf + yellowDwarf + orangeDwarf)
                {
                    min = 0.48;
                    max = 0.78;
                } // M type stars
                else
                {
                    min = minimumMass;
                    max = 0.48;
                }

                // generates a random mass contained between min and max
                mass = Dice.Roll(max - min) + min;

                double starSystemChance = Dice.Roll();
                SolarSystem system;

                if (starSystemChance <= 0.6)
                {
                    // generates data for that particular star              
                    system = CreateSingleStarSystem(GetRandomSystemName(), mass);
                }
                else if (starSystemChance <= 0.9)
                {
                    system = CreateMultipleStarSystem(GetRandomSystemName(), mass,2);
                }
                else
                {
                    system = CreateMultipleStarSystem(GetRandomSystemName(), mass,3);
                }

                // populates the galaxy
                solarSystems.Add(system.Name, system);

                //SolarSystem system = CreateBinaryStarSystem(GetRandomSystemName(), mass);

                
                //averageAge += star.StellarFeatures.Age;

                #region Debug Code

                //switch (star.StellarFeatures.Type)
                //{
                //    case StarType.BlackHole:
                //        bh++;
                //        break;

                //    case StarType.BlueDwarf:
                //        bd++;
                //        break;

                //    case StarType.NeutronStar:
                //        ns++;
                //        break;

                //    case StarType.OrangeDwarf:
                //        od++;
                //        break;

                //    case StarType.RedDwarf:
                //        rd++;
                //        break;

                //    case StarType.RedGiant:
                //        rg++;
                //        break;

                //    case StarType.WhiteDwarf:
                //        wd++;
                //        break;

                //    case StarType.YellowDwarf:
                //        yd++;
                //        break;

                //    default:
                //        bugs++;
                //        break;
                //}

                #endregion
            }

            averageAge /= starCount;
            //DebugManager.LogToScreen(
            //    string.Format("Bd: {0} Yd: {1} Od: {2} Rd {3} RG: {4} Wd: {5} BH: {6} NS: {7} Bugs: {8}",
            //                  bd, yd, od, rd, rg, wd, bh, ns, bugs));


            return solarSystems;
        }

        /// <summary>
        /// Creates a single star system (a star system with only one star).
        /// </summary>
        /// <param name="systemName">Name of the star system.</param>
        /// <param name="mass">The mass of the primary star.</param>
        /// <returns>The generated <see cref="SolarSystem"/> object.</returns>
        public SolarSystem CreateSingleStarSystem(string systemName, double mass)
        {
            EvolutionResult evolutionResult;
            StellarFeatures mainSequenceFeatures;
            Star star = starGen.GenerateSingleStarSystem(systemName, mass);
            StellarFeatures evolvedFeatures = star.StellarFeatures;

            if (evolvedFeatures.SpectralClass.Type == StarType.Other)
                return new SolarSystem(systemName);

            SolarSystem solarSystem = new SolarSystem(star.Name);
            GenerateInnerSystem(star);
            solarSystem.AddStars(star);
            
            //sv.CheckRocheLimit();

            systemValidator.EvolveSystem(solarSystem);

            sb.Append(star);
            return solarSystem;
        }

        public SolarSystem CreateMultipleStarSystem(string systemName, double primaryMass, int starCount)
        {
            double[] starMasses = new double[starCount];
            starMasses[0] = primaryMass;
            for(int i=1; i < starCount; i++)
            {
                starMasses[i] = Dice.Roll(minimumMass, starMasses[i - 1]);
            }

            EvolutionResult[] evolutionResults;
            StellarFeatures[] mainSequenceFeatures;
            double[] meanSeparationDistanceArray;
            Star[] stars = starGen.GenerateMultipleStarSystem(systemName, starMasses,
                out meanSeparationDistanceArray);

            SolarSystem system = new SolarSystem(systemName);

            if (stars[0].StellarFeatures.SpectralClass.Type != StarType.Other)
                GenerateInnerSystem(stars[0]);

            for (int i = 1; i < stars.Length; i++)
            {
                if (stars[i].StellarFeatures.SpectralClass.Type != StarType.Other 
                    && meanSeparationDistanceArray[i-1] > 100.0)
                    GenerateInnerSystem(stars[i]);
                
                double minimumSeparation = meanSeparationDistanceArray[i-1] * (1 - stars[i].StellarFeatures.Eccentricity);
                double maximumSeparation = meanSeparationDistanceArray[i-1] * (1 + stars[i].StellarFeatures.Eccentricity);

                systemValidator.RemovePlanetsInForbiddenZone(stars[i], minimumSeparation / 3, maximumSeparation * 3);
            }

            systemValidator.EvolveSystem(system);
            system.AddStars(stars);

            sb.AppendLine(starCount == 2 ? "Binary System" : "Trinary System");
            foreach (Star star in stars)
                sb.Append(star);

            return system;
        }

        public SolarSystem CreateBinaryStarSystem(string systemName, double primaryMass)
        {
            double[] starMasses = new double[2];
            starMasses[0] = primaryMass;
            starMasses[1] = Dice.Roll(minimumMass, primaryMass);
            EvolutionResult[] evolutionResults;
            StellarFeatures[] mainSequenceFeatures;
            double[] meanSeparationDistanceArray;
            Star[] stars = starGen.GenerateMultipleStarSystem(systemName, starMasses,
                out meanSeparationDistanceArray);

            SolarSystem system = new SolarSystem(systemName);
            if (stars[0].StellarFeatures.SpectralClass.Type != StarType.Other)
                GenerateInnerSystem(stars[0]);

            double minimumSeparation = meanSeparationDistanceArray[0] * (1 - stars[0].StellarFeatures.Eccentricity);
            double maximumSeparation = meanSeparationDistanceArray[0] * (1 + stars[0].StellarFeatures.Eccentricity);
            systemValidator.RemovePlanetsInForbiddenZone(stars[0], minimumSeparation / 3, maximumSeparation * 3);

            

            if (stars[1].StellarFeatures.SpectralClass.Type != StarType.Other && meanSeparationDistanceArray[0] >= 100)
                GenerateInnerSystem(stars[0]);

            systemValidator.EvolveSystem(system);

                system.AddStars(stars);

            sb.AppendLine("Binary System");
            sb.Append(stars[0].ToString());
            sb.Append(stars[1].ToString());

            return system;
        }


        void GenerateInnerSystem(Star star)
        {
            StellarFeatures evolvedFeatures = star.StellarFeatures;
            StellarFeatures mainSequenceFeatures = star.MainSequenceFeatures;
            EvolutionResult evolutionResult = star.EvolutionResult;

            Protosystem protoSystem = new Protosystem(mainSequenceFeatures);
            LinkedListNode<Protoplanet> node = protoSystem.DistributePlanetaryMasses();
         
            // If the star has become either a Subgiant or Giant the solar System is 
            // generated as if it were a main sequence star, the effects of the stellar 
            // evolution will be computed later.
            // Otherwise the planet generator will use the evolved features to compute
            // planetary values.

            planetGenerator.StellarFeatures = evolutionResult != EvolutionResult.MainSequence
                                                  ? mainSequenceFeatures
                                                  : evolvedFeatures;

            int planetIndex = 1;
            while (node != null)
            {
                bool isGasGiant;
                CelestialFeatures celestialFeatures = planetGenerator.GeneratePlanet(node.Value, out isGasGiant);
                PlanetaryFeatures planetaryFeatures =
                    isGasGiant
                        ? planetClassifier.ClassifyGasGiant(celestialFeatures, evolvedFeatures)
                        : planetClassifier.ClassifyPlanet(celestialFeatures, evolvedFeatures);
                Planet celestialObject = new Planet(
                    string.Format("{0} {1}", star.Name,
                                  GenerationCommon.IntegerToRomanNumeral(planetIndex)),
                    celestialFeatures, planetaryFeatures);
                star.AddObject(celestialFeatures.SemiMajorAxis, celestialObject);
                planetIndex++;
                node = node.Next;


                Protosystem ps = new Protosystem(evolvedFeatures, celestialFeatures);
                LinkedListNode<Protoplanet> node1 = ps.DistributeMoonMassesRandomly();

                int moonIndex = 1;
                while (node1 != null)
                {
                    node1.Value.A += celestialFeatures.SemiMajorAxis;
                    CelestialFeatures moonCelestialFeatures = planetGenerator.GeneratePlanet(node1.Value, out isGasGiant);
                    PlanetaryFeatures moonPlanetaryFeatures =
                        isGasGiant
                            ? planetClassifier.ClassifyGasGiant(moonCelestialFeatures, evolvedFeatures)
                            : planetClassifier.ClassifyMoon(moonCelestialFeatures, evolvedFeatures,
                                                              celestialFeatures);

                    Planet moon = new Planet(celestialObject.Name + Char.ConvertFromUtf32(96 + moonIndex),
                                             moonCelestialFeatures, moonPlanetaryFeatures);
                    celestialObject.AddObject(moonCelestialFeatures.SemiMajorAxis, moon);
                    moonIndex++;

                    node1 = node1.Next;
                }

            }

        }

     
        string GetRandomSystemName()
        {
            int starNameIndex = Dice.Roll1D(starNames.Count) - 1;
            string systemName = starNames[starNameIndex];
            starNames.RemoveAt(starNameIndex);
            return systemName;
        }

        public string Log
        {
            get { return sb.ToString(); }
        }
    }
}