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

                        if (!CoalescePlanetesimals(p0))
                            InsertPlanet(p0);

                    }
                    else
                    {
                        //System.out.println(".. failed due to large neighbor.");
                    }
                }
            }

            return protoplanets.First;
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
                    p0.CriticalMass = Protoplanet.ComputeCriticalMass(celestialFeatures.OrbitalRadius, celestialFeatures.Eccentricity, stellarFeatures.Luminosity);
                    disc.AccreteDust(p0);
                    p0.DustMass += Protoplanet.ProtoplanetMass;
                    if (p0.Mass != 0.0 && p0.Mass != Protoplanet.ProtoplanetMass)
                    {
                        p0.IsGasGiant = p0.Mass >= p0.CriticalMass;

                        if (!CoalescePlanetesimals(p0))
                            InsertPlanet(p0);

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
                double distance, dist1, dist2;
                currentProtoplanet = node.Value;
                distance = currentProtoplanet.A - p.A;
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

        void InsertPlanet(Protoplanet tsml)
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
    }
}
