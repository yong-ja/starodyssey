#region Disclaimer

/* 
 * Protosystem
 *
 * Created on 01 settembre 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.Avengersutd.com
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
using System.Collections.Generic;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
#endregion

namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{
    public class Protosystem
    {
        #region Private fields

        readonly StellarFeatures stellarFeatures;
        readonly CelestialFeatures celestialFeatures;
        readonly DustDisc disc;
        readonly double bodyInnerLimit;
        readonly double bodyOuterLimit;

        readonly LinkedList<Protoplanet> protoplanets;

        #endregion

        #region Properties

        public LinkedList<Protoplanet> Protoplanets
        {
            get { return protoplanets; }
        }

        #endregion

        #region Constructors

        public Protosystem(StellarFeatures stellarFeatures)
        {
            this.stellarFeatures = stellarFeatures;

            bodyInnerLimit = stellarFeatures.InnerLimit;
            bodyOuterLimit = stellarFeatures.OuterLimit;
            disc = new DustDisc(0.0, stellarFeatures.StellarDustLimit,
                                bodyInnerLimit, bodyOuterLimit);
            disc.CloudEccentricity = 0.2;

            protoplanets = new LinkedList<Protoplanet>();
        }

        public Protosystem(StellarFeatures stellarFeatures, CelestialFeatures celestialFeatures)
        {
            this.stellarFeatures = stellarFeatures;
            this.celestialFeatures = celestialFeatures;

            bodyInnerLimit = celestialFeatures.NearestMoon;
            bodyOuterLimit = celestialFeatures.FarthestMoon;

            disc = new DustDisc(0.0, stellarFeatures.StellarDustLimit,
                                bodyInnerLimit, bodyOuterLimit);
            disc.CloudEccentricity = 0.2;

            protoplanets = new LinkedList<Protoplanet>();
        }

        #endregion

        /// <summary>
        /// Accretes protoplanets from the dust disc in this system.
        /// </summary>
        /// <returns>First protoplanet of accreted system, as the head
        /// element of a list of protoplanets.</returns>
        public LinkedListNode<Protoplanet> DistributePlanetaryMasses()
        {
            Protoplanet p0;

            while (disc.IsDustLeft)
            {
                p0 = new Protoplanet(disc.BodyInnerLimit, disc.BodyOuterLimit);
                if (disc.IsDustAvailable(p0))
                {
                    p0.ComputeStellarDustDensity(stellarFeatures.Mass);
                    p0.CriticalMass = Protoplanet.ComputeCriticalMass(p0.A, p0.E,stellarFeatures.Luminosity);
                    disc.AccreteDust(p0);
                    p0.DustMass += Protoplanet.ProtoplanetMass;
                    if (p0.Mass != 0.0 && p0.Mass != Protoplanet.ProtoplanetMass)
                    {
                        p0.IsGasGiant = p0.Mass >= p0.CriticalMass;

                        MigrateInward(p0);
                        if (!CoalescePlanetesimals(p0))
                            InsertPlanet(protoplanets,p0);

                    }
                    else
                    {
                        //System.out.println(".. failed due to large neighbor.");
                    }
                }
            }

            return protoplanets.First;
        }


        public LinkedListNode<Protoplanet> DistributeMoonMassesRandomly()
        {
            
            PlanetSize size = 
                celestialFeatures.IsGasGiant ? PlanetClassifier.ClassifyGasGiantSize(celestialFeatures.Mass) :
                PlanetClassifier.ClassifyPlanetSize(celestialFeatures.Radius);

            int iSize = (int) size;
            if (iSize <= (int) PlanetSize.Medium)
                return null;
            else if (iSize <= (int)PlanetSize.Immense && celestialFeatures.SemiMajorAxis <= 0.5)
                return null;
            else if (iSize >= (int)PlanetSize.SubJovian && iSize <= (int)PlanetSize.SuperJovian 
                && celestialFeatures.SemiMajorAxis <= 0.05)
                return null;

            const int moonChancesIdx = 0,
                      maxMoonsIdx = 1,
                      chance1Idx = 2,
                      minMass1Idx = 3,
                      maxMass1Idx = 4,
                      chance2Idx = 5,
                      maxMass2Idx = 6,
                      percentOriginalMassIdx = 7;

            double[,] moonChances = new[,]
                {
                    // MC = moonChances
                    // mMs = maxMoons
                    // c? = chance 1/2/3
                    // mM = minMass MM = maxMass
                    // pOM = percentOriginalMass minMax
                    // MC  mMs  c1   mM1    MM1     c2     MM2    POM 
                    {0.25, 1.0, 0.9, 0.005, 0.0075, 0.95, 0.0100, 0.50}, // Medium
                    {0.30, 1.0, 0.9, 0.005, 0.01,   0.95, 0.0125, 1.00}, // Large
                    {0.35, 2.0, 0.8, 0.005, 0.0125, 0.95, 0.0150, 0.75}, // Huge
                    {0.40, 3.0, 0.8, 0.005, 0.0125, 0.95, 0.0150, 0.50}, // Immense
                    {0.60, 4.0, 0.85, 0.005, 0.0125, 0.95, 0.0300, 0.015}, // Neptunian
                    {0.70, 5.0, 0.75, 0.005, 0.0150, 0.95, 0.0750, 0.010}, // SubJovian
                    {0.80, 6.0, 0.75, 0.005, 0.0150, 0.95, 0.1000, 0.0075}, // Jovian
                    {0.70, 5.0, 0.80, 0.005, 0.0125, 0.95, 0.5000, 0.0050}, // SuperJovian
                    {0.60, 4.0, 0.85, 0.005, 0.0150, 0.95, 1.5000, 0.0025}, // MacroJovian
                };
            int arrayIndex = iSize - 3;


            LinkedList<Protoplanet> list = new LinkedList<Protoplanet>();
            double minMass1 = moonChances[arrayIndex, minMass1Idx];
            double maxMass1 = moonChances[arrayIndex, maxMass1Idx];
            double minMass2 = maxMass1;
            double maxMass2 = moonChances[arrayIndex, maxMass2Idx];

            double minMass3 = maxMass2;
            double maxMass3 = celestialFeatures.Mass * moonChances[arrayIndex, percentOriginalMassIdx];

            int generatedMoons = 0;
            for (int i = 0; i < moonChances[arrayIndex,maxMoonsIdx]; i++)
            {

                if (Dice.Roll() <= moonChances[arrayIndex, moonChancesIdx])
                {
                    Protoplanet planet = new Protoplanet();

                    double chance = Dice.Roll();
                    double minMass = 0, maxMass = 0;

                    if (chance <=  moonChances[arrayIndex,chance1Idx])
                    {
                        minMass = minMass1;
                        maxMass = maxMass1;
                    }
                    else if (chance <=  moonChances[arrayIndex,chance2Idx])
                    {
                        minMass = minMass2;
                        maxMass = maxMass2;
                    }
                    else
                    {
                        minMass = minMass3;
                        maxMass = maxMass3;
                    }

                    double finalMass = Dice.Roll(minMass, maxMass);
                    planet.Mass =  finalMass / PhysicalConstants.SolarMassInEarthMasses;
                    planet.A = Dice.Roll(celestialFeatures.NearestMoon, celestialFeatures.FarthestMoon);
                    planet.IsGasGiant = finalMass >= PhysicalConstants.GasGiantMinimumMass;

                    if (planet.IsGasGiant)
                    {
                        double dustRatio = Dice.Roll(0.15, 0.3);
                        planet.DustMass = planet.Mass * dustRatio;
                        planet.GasMass = planet.Mass * (1 - dustRatio);
                    }
                    else
                        planet.DustMass = planet.Mass;

                    InsertPlanet(list, planet);
                    generatedMoons++;
                }
            }

          

            if (generatedMoons > 0)
                return list.First;
            else
                return null;

        }

        public LinkedListNode<Protoplanet> DistributeMoonMasses()
        {
            Protoplanet p0;

            while (disc.IsDustLeft)
            {
                p0 = new Protoplanet(disc.BodyInnerLimit, disc.BodyOuterLimit);
                if (disc.IsDustAvailable(p0))
                {
                    p0.ComputePlanetDustDensity(celestialFeatures.Mass);
                    p0.CriticalMass = Protoplanet.ComputeCriticalMass(celestialFeatures.SemiMajorAxis, celestialFeatures.Eccentricity, stellarFeatures.Luminosity);
                    disc.AccreteDust(p0);
                    p0.DustMass += Protoplanet.ProtoplanetMass;
                    if (p0.Mass != 0.0 && p0.Mass != Protoplanet.ProtoplanetMass)
                    {
                        p0.IsGasGiant = p0.Mass >= p0.CriticalMass;

                        if (!CoalescePlanetesimals(p0))
                            InsertPlanet(protoplanets,p0);

                    }
                    else
                    {
                        //System.out.println(".. failed due to large neighbor.");
                    }
                }
            }

            return protoplanets.First;
        }

        static void CoalesceTwoProtoplanets(Protoplanet first, Protoplanet second)
        {
            double newMass = first.Mass + second.Mass;
            double newA = newMass / ((first.Mass / first.A) + (second.Mass / second.A));
            double term1 = first.Mass * Math.Sqrt(first.A * (1.0 - first.E * first.E));
            double term2 = second.Mass * Math.Sqrt(second.A * (1.0 - second.E * second.E));
            double term3 = (term1 + term2) / (newMass * Math.Sqrt(newA));
            double term4 = 1.0 - term3 * term3;
            if (term4 < 0 || term4 >= 1)
                term4 = 0;
            double newE = Math.Sqrt(Math.Abs(term4));

            first.Mass = newMass;
            first.A = newA;
            first.E = newE;
            first.IsGasGiant = first.IsGasGiant || second.IsGasGiant;
        }

        /// <summary>
        /// Searches the planetesimals already present in this system
        /// for a possible collision.  Does not run any long-term simulation
        /// of orbits, doesn't try to eject bodies.
        /// </summary>
        /// <param name="p">Newly injected accreting protoplanet to test.</param>
        bool CoalescePlanetesimals(Protoplanet p)
        {
            // P is not consumed by this method.

            Protoplanet currentProtoplanet;
           
            LinkedListNode<Protoplanet> node = protoplanets.First;
            while (node!= null)
            { 
                double dist1, dist2;
                currentProtoplanet = node.Value;
                double distance = currentProtoplanet.A - p.A;
                if ((distance > 0.0))
                {
                    dist1 = Protoplanet.OuterEffectLimit(p.A, p.E,
                        Protoplanet.ComputeReducedMass(p.Mass)) - p.A;

                    /* x aphelion */
                    dist2 = currentProtoplanet.A - Protoplanet.InnerEffectLimit(currentProtoplanet.A, currentProtoplanet.E,
                                                         Protoplanet.ComputeReducedMass(currentProtoplanet.Mass));
                }
                else
                {
                    dist1 = p.A - Protoplanet.InnerEffectLimit(p.A, p.E, Protoplanet.ComputeReducedMass(p.Mass));

                    /* x perihelion */
                    dist2 = Protoplanet.OuterEffectLimit(currentProtoplanet.A, currentProtoplanet.E,
                                                         Protoplanet.ComputeReducedMass(currentProtoplanet.Mass)) -
                                                         currentProtoplanet.A;
                }
                if (((Math.Abs(distance) <= Math.Abs(dist1)) || (Math.Abs(distance) <= Math.Abs(dist2))))
                {
                   //Debug.WriteLine("Collision between two planetesimals!");

                    CoalesceTwoProtoplanets(currentProtoplanet, p);
                    return true;
                }
                else
                {
                    //currentProtoplanet = currentProtoplanet.NextProtoplanet;
                    node = node.Next;
                }
            }

            return false;

        }

        void InsertPlanet(LinkedList<Protoplanet> protoplanets, Protoplanet tsml)
        {
            
            if (protoplanets.Count == 0)
                protoplanets.AddFirst(tsml);
            else
            {
                LinkedListNode<Protoplanet> node = protoplanets.First;
                while (node != null && tsml.A > node.Value.A) 
                {
                    node = node.Next;
                }
                if (node != null)
                    protoplanets.AddBefore(node, tsml);
                else
                    protoplanets.AddLast(tsml);

            }
            return;         
        }

        void MigrateInward(Protoplanet planetesimal)
        {
            const double epistellarChance = 0.25;
            const double eccentricChance = 0.35;
            const double panthalassicChance = 0.25;

            if (planetesimal.IsGasGiant)
            {
                PlanetSize gasGiantSize =
                    PlanetClassifier.ClassifyGasGiantSize(planetesimal.Mass * PhysicalConstants.SolarMassInEarthMasses);

                if ((gasGiantSize == PlanetSize.SubJovian || gasGiantSize == PlanetSize.Jovian))
                {
                    double chance = Dice.Roll();
                  
                    if (chance <= epistellarChance)
                    {
                        planetesimal.A = Dice.Roll()*0.2*stellarFeatures.InnerLimit;
                        planetesimal.E = Dice.RollGaussian(0.1, 0.005);
                    }
                    else if (chance <= eccentricChance)
                    {
                        planetesimal.A = Dice.Roll()*0.75 * stellarFeatures.SnowlineRadius;
                        planetesimal.E = Dice.Rnd.About(0.8, 0.1);
                    }
                }
            }
            else if (planetesimal.Mass >= 2.0/PhysicalConstants.SolarMassInEarthMasses && planetesimal.A >= stellarFeatures.SnowlineRadius)
            {
                if (Dice.Roll()<= panthalassicChance)
                    planetesimal.A = Dice.Roll() * stellarFeatures.SnowlineRadius;
            }


        }
    }
}
