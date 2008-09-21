using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration;

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    public class SystemValidator
    {
        static PlanetGenerator pGen = new PlanetGenerator();
        static PlanetClassifier pClas = new PlanetClassifier();

        public void RemovePlanetsInForbiddenZone(Star star, double innerLimit, double outerLimit)
        {
            List<Planet> toRemove = new List<Planet>();

            foreach (Planet planet in star)
            {
              if (planet.CelestialFeatures.SemiMajorAxis >= innerLimit &&
                  planet.CelestialFeatures.SemiMajorAxis <= outerLimit)
                  toRemove.Add(planet);
            }

            if (toRemove.Count > 0)
            {
                foreach (Planet planet in toRemove)
                    star.RemoveObject(planet);
                RenameSystem(star);
            }
        }

        public void EvolveSystem(SolarSystem system)
        {
            foreach (Star star in system)
            {
                StellarFeatures evolvedFeatures = star.StellarFeatures;

                if (evolvedFeatures.SpectralClass.Type == StarType.MainSequence)
                    continue;

                pGen.StellarFeatures = evolvedFeatures;
                List<Planet> toRemove = new List<Planet>();
                double starRadiusAU = evolvedFeatures.SpectralClass.SpectralType == SpectralType.D
                                          ? star.MainSequenceFeatures.Luminosity*Dice.Roll(2000, 5000)
                                          : evolvedFeatures.RadiusAU;

                for (int i = 0; i < star.CelestialBodiesCount; i++)
                {
                    Planet planet = (Planet) star[i];
                    double distanceToStar = planet.CelestialFeatures.SemiMajorAxis -
                                            starRadiusAU;
                    if (distanceToStar < 0.1)
                        toRemove.Add(planet);
                    else
                    {
                        planet.CelestialFeatures = pGen.GenerateEvolvedPlanet(planet.CelestialFeatures);
                        planet.PlanetaryFeatures = planet.CelestialFeatures.IsGasGiant
                                                       ? pClas.ClassifyGasGiant(planet.CelestialFeatures,
                                                                                evolvedFeatures)
                                                       : pClas.ClassifyPlanet(planet.CelestialFeatures, evolvedFeatures);

                        foreach (Planet moon in planet)
                        {
                            moon.CelestialFeatures = pGen.GenerateEvolvedPlanet(moon.CelestialFeatures);
                            moon.PlanetaryFeatures = moon.CelestialFeatures.IsGasGiant
                                                         ? pClas.ClassifyGasGiant(moon.CelestialFeatures,
                                                                                  evolvedFeatures)
                                                         : pClas.ClassifyMoon(moon.CelestialFeatures, evolvedFeatures,
                                                                              planet.CelestialFeatures);
                        }
                    }

                }

                if (toRemove.Count > 0)
                {
                    foreach (Planet planet in toRemove)
                        star.RemoveObject(planet);
                    RenameSystem(star);
                }
            }
        }

        void RenameSystem(Star star)
        {
            for (int i = 0; i < star.CelestialBodiesCount; i++)
            {
                CelestialBody planet = star[i];
                planet.Name = string.Format("{0} {1}", star.Name, GenerationCommon.IntegerToRomanNumeral(i + 1));

                for (int j = 0; j < planet.CelestialBodiesCount; j++)
                {
                    CelestialBody moon = planet[j];
                    moon.Name = planet.Name + Char.ConvertFromUtf32(97 + j);
                }

            }
        }


        static int i;
        static int rocheLimit;

        public SystemValidator()
        {
        }

        public void CheckRocheLimit(SolarSystem system)
        {
            foreach (Star star in system)
            {
                foreach (Planet planet in star)
                {
                    CelestialFeatures primaryBodyFeatures = planet.CelestialFeatures;

                    double hillSphereRadius = PlanetGenerator.ComputeHillSphereRadius(
                                primaryBodyFeatures.SemiMajorAxis,
                                primaryBodyFeatures.Eccentricity,
                                primaryBodyFeatures.Mass,
                                primaryBodyFeatures.Mass * PhysicalConstants.SolarMassInEarthMasses);

                    if (planet.CelestialBodiesCount > 0)
                    {
                        foreach (Planet moon in planet)
                        {
                            CelestialFeatures moonFeatures = moon.CelestialFeatures;
                            double rocheLimitRadius = PlanetGenerator.ComputeRocheLimitRigid(
                                primaryBodyFeatures.Radius,
                                primaryBodyFeatures.Density,
                                moonFeatures.Density);

                            double distanceToPrimary = moonFeatures.SemiMajorAxis - primaryBodyFeatures.SemiMajorAxis;

                            if (distanceToPrimary - rocheLimitRadius <= 0.00001)
                            {
                                rocheLimit++;
                                System.Diagnostics.Debug.WriteLine(rocheLimit,") Inside Roche Limit");
                            }
                            else if (distanceToPrimary >= hillSphereRadius)
                                System.Diagnostics.Debug.WriteLine("Outside Hill Sphere Limit");
                        }
                    }
                }
            }
        }

        public void CheckSatellites(SolarSystem system)
        {
            foreach (Star star in system)
            {
                foreach (Planet planet in star)
                {
                    CelestialFeatures primaryBodyFeatures = planet.CelestialFeatures;

                    if (planet.CelestialBodiesCount > 0)
                    {
                        foreach (Planet moon in planet)
                        {
                            CelestialFeatures moonFeatures = moon.CelestialFeatures;
                            double moonTidalForceByPlanet =
                                PlanetGenerator.ComputeTidalForcesOnSatelliteMoon(primaryBodyFeatures.Mass,
                                                                                  moonFeatures.Mass, moonFeatures.Radius,
                                                                                  moonFeatures.SemiMajorAxis -
                                                                                  primaryBodyFeatures.SemiMajorAxis);
                            double totalTidalEffect = (moonTidalForceByPlanet*star.StellarFeatures.Age)/
                                                      moonFeatures.Mass;

                            if (totalTidalEffect >= 50)
                            {
                                System.Diagnostics.Debug.WriteLine(i + ") Tide Locked by Planet");
                                i++;
                            }
                        }
                    }
                    else
                    {
                        double moonTidalForceByStar =
                            PlanetGenerator.ComputeTidalForcesOnSatelliteMoon(
                                star.StellarFeatures.Mass*PhysicalConstants.SolarMassInEarthMasses,
                                primaryBodyFeatures.Mass,
                                primaryBodyFeatures.Radius,
                                primaryBodyFeatures.SemiMajorAxis);
                        double totalTidalEffect = (moonTidalForceByStar * star.StellarFeatures.Age) /
                                                      primaryBodyFeatures.Mass;

                        if (totalTidalEffect >= 50)
                        {
                            System.Diagnostics.Debug.WriteLine(i + ") Tide Locked by Star");
                            i++;
                        }
                    }
                }
            }
        }
    }
}
