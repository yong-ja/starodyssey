using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.Odyssey.Utils.Xml;


namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{
    public class GalaxyGenerator
    {
        const double minimumMass = 0.01;
        List<string> starNames;
        GalaxyOptions galaxyOptions;
        StarGenerator starGen;
        PlanetGenerator planetGenerator;

        StringBuilder sb = new StringBuilder();

        public GalaxyGenerator()
            : this(Data.Deserialize<GalaxyOptions>(Global.GalaxyOptionsPath))
        {
        }

        public GalaxyGenerator(GalaxyOptions galaxyOptions)
        {
            this.galaxyOptions = galaxyOptions;
            starGen = new StarGenerator(galaxyOptions);
            planetGenerator = new PlanetGenerator();
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
            double yellowDwarf = starChances[StarColor.Yellow];
            double orangeDwarf = starChances[StarColor.Orange];
            double redDwarf = starChances[StarColor.Red];

            int starCount = galaxyOptions.StarCount;



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

                if (starSystemChance <= 0.6)
                {
                    
                }

                // generates data for that particular star              
                CreateSingleStarSystem(GetRandomSystemName(), mass);
                //SolarSystem system = CreateBinaryStarSystem(GetRandomSystemName(), mass);

                // populates the star system
                //SolarSystem system = CreateSingleStarSystem(GetRandomSystemName(), star);
                //solarSystems.Add(system.Name, system);
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
            StellarFeatures mainSequenceFeatures, evolvedFeatures;
            Star star = starGen.GenerateSingleStarSystem(systemName, mass, out evolutionResult, out mainSequenceFeatures);
            evolvedFeatures = star.StellarFeatures;

            sb.Append(star.ToString());

            Protosystem protoSystem = new Protosystem(mainSequenceFeatures);
            LinkedListNode<Protoplanet> node = protoSystem.DistributePlanetaryMasses();

            planetGenerator.StellarFeatures = evolvedFeatures;

            int index=1;
            while (node != null)
            {
                CelestialFeatures cf = planetGenerator.GeneratePlanet(star.StellarFeatures.Name, index, node.Value);
                node = node.Next;
                sb.Append(cf.ToString());
                index++;
            }

            return null;
        }

        public SolarSystem CreateBinaryStarSystem(string systemName, double primaryMass)
        {
            double[] starMasses = new double[2];
            starMasses[0] = primaryMass;
            starMasses[1] = Dice.Roll(minimumMass, primaryMass);
            EvolutionResult[] primaryEvolutionResult, companionEvolutionResult;
            StellarFeatures[] mainSequenceFeatures, primaryFeatures, companionFeatures;
            Star[] stars = starGen.GenerateMultipleStarSystem(systemName, starMasses, out primaryEvolutionResult, out mainSequenceFeatures);

            sb.Append(stars[0].ToString());
            sb.Append(stars[1].ToString());

            return null;
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