#region Disclaimer

/* 
 * Protoplanet
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

namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{
    #region Using Directives

    using System;

    #endregion

    public class Protoplanet
    {
        #region Private fields

        internal const double ProtoplanetMass = 1.0E-15;
        static readonly EnhancedRandom rnd = Dice.Rnd;

        double a;
        double e;
        double mass;
        double dustMass;
        double gasMass;
        double criticalMass;
        double dustDensity;
        bool isGasGiant;
        Protoplanet nextProtoplanet;

        #endregion

        #region Properties

        public double A
        {
            get { return a; }
            set { a = value; }
        }

        public double E
        {
            get { return e; }
            set { e = value; }
        }

        public double Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        public double DustMass
        {
            get { return dustMass; }
            set { dustMass = value; }
        }

        public double GasMass
        {
            get { return gasMass; }
            set { gasMass = value; }
        }

        public double DustDensity
        {
            get { return dustDensity; }
        }

        public double CriticalMass
        {
            get { return criticalMass; }
            set { criticalMass = value; }
        }

        /// <summary>
        /// Determines if the protoplanet is massy enough to accrete gas.
        /// </summary>
        /// <value><c>true</c> if this protoplanet is a gas giant; otherwise, <c>false</c>.</value>
        public bool AccretesGas
        {
            get { return mass > criticalMass; }
        }


        public bool IsGasGiant
        {
            get { return isGasGiant; }
            set { isGasGiant = value; }
        }


        public Protoplanet NextProtoplanet
        {
            get { return nextProtoplanet; }
            set { nextProtoplanet = value; }
        }

        #endregion

        #region Constructors

        public Protoplanet()
        {
            a = 1;
            e = 0.016720;
            mass = 1;
            isGasGiant = false;
            nextProtoplanet = null;
        }

        /// <summary>
        /// Constructs a new seed protoplanet at a random location within
        /// the specified range.
        /// </summary>
        /// <param name="minimumSMAxis">The minimum Semi Major axis.</param>
        /// <param name="maximumSMAxis">The maximum Semi Major axis.</param>
        public Protoplanet(double minimumSMAxis, double maximumSMAxis)
        {
            a = rnd.NextDouble(minimumSMAxis, maximumSMAxis);
            e = RandomEccentricity();
            mass = ProtoplanetMass;
            dustMass = 0.0;
            gasMass = 0.0;
            isGasGiant = false;
            criticalMass = 0.0;
            dustDensity = 0.0;
            nextProtoplanet = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Protoplanet"/> class,
        /// copying the values from the source Protoplanet.
        /// </summary>
        /// <param name="p">The .</param>
        public Protoplanet(Protoplanet p)
        {
            a = p.A;
            e = p.E;
            mass = p.Mass;
            dustMass = p.DustMass;
            gasMass = p.GasMass;
            nextProtoplanet = p.NextProtoplanet;
            isGasGiant = p.IsGasGiant;
            criticalMass = p.CriticalMass;
            dustDensity = p.DustDensity;
        }

        #endregion

        static double AphelionDistance(double radius, double eccentricity)
        {
            return radius * (1.0 + eccentricity);
        }

        static double PerihelionDistance(double radius, double eccentricity)
        {
            return radius * (1.0 - eccentricity);
        }

        static double LowBound(double inner, double cloudEccentricity)
        {
            return (inner / (1.0 + cloudEccentricity));
        }

        static double HighBound(double outer, double cloudEccentricity)
        {
            return (outer / (1.0 - cloudEccentricity));
        }


        public static double InnerEffectLimit(double semiMajorAxis, double orbitEccentricity, double mass)
        {
            return PerihelionDistance(semiMajorAxis, orbitEccentricity) * (1.0 - mass);
        }

        public static double OuterEffectLimit(double semiMajorAxis, double orbitEccentricity, double mass)
        {
            return AphelionDistance(semiMajorAxis, orbitEccentricity) * (1.0 + mass);
        }

        /// <summary>
        /// Calculates innermost limit of gravitational influence.
        /// </summary>
        /// <returns>Inner effect limit in AU.</returns>
        public static double InnerSweptLimit(double semiMajorAxis, double orbitEccentricity, double mass, double cloudEccentricity)
        {
            return LowBound(InnerEffectLimit(semiMajorAxis, orbitEccentricity, mass), cloudEccentricity);
        }

        /// <summary>
        /// Calculates outermost limit of gravitational influence.
        /// </summary>
        /// <returns>Inner effect limit in AU.</returns>
        public static double OuterSweptLimit(double semiMajorAxis, double orbitEccentricity, double mass, double cloudEccentricity)
        {
            return HighBound(OuterEffectLimit(semiMajorAxis, orbitEccentricity, mass), cloudEccentricity);
        }

        /// <summary>
        /// Performs the mass 'reduction' calculation for the inner accretion loop.
        /// </summary>
        public static double ComputeReducedMass(double mass)
        {
            double reducedMass;

            if (mass < 0.0) 
                reducedMass = 0.0;
            else
            {
                double temp = mass/(1.0 + mass);
                try
                {
                    reducedMass = Math.Pow(temp, (1.0/4.0));
                }
                catch (ArithmeticException)
                {
                    reducedMass = 0.0;
                }
            }
            return reducedMass;
        }

        public void ComputeStellarDustDensity(double stellarMass)
        {
            dustDensity = PhysicalConstants.DustDensityCoefficient*Math.Sqrt(stellarMass)
                          *Math.Exp(-PhysicalConstants.Alpha*Math.Pow(a, (1.0/PhysicalConstants.N)));
        }

        public void ComputePlanetDustDensity(double planetMass)
        {
            dustDensity =  100 *PhysicalConstants.DustDensityCoefficient * Math.Sqrt(planetMass /PhysicalConstants.SolarMassInEarthMasses)
                          * Math.Exp(-PhysicalConstants.Alpha * Math.Pow(a, (1.0 / PhysicalConstants.N)));
        }

        /// <summary>
        /// Calculates the mass at which a protoplanet orbiting the given star
        /// will accrete gas as well as dust.
        /// </summary>
        /// <param name="luminosity">The luminosity of the given star.</param>
        /// <returns>
        /// Critical mass of protoplanet, in Solar masses.
        /// </returns>
        public static double ComputeCriticalMass(double semimajorAxis, double eccentricity, double luminosity)
        {
            double perihelionDistance = (semimajorAxis - semimajorAxis * eccentricity);
            double temp = perihelionDistance*Math.Sqrt(luminosity);
            return (PhysicalConstants.B*Math.Pow(temp, -0.75));
            
        }

        /// <summary>
        /// Calculates unit density of material to be accreted from the
        /// specified dust band.
        /// </summary>
        /// <param name="dust">A boolean value specifying dust availability.</param>
        /// <param name="gas">A boolean value specifying gas availability.</param>
        /// <returns></returns>
        public void ComputeMassDensity(bool dust, bool gas, out double massDensity, out double gasDensity)
        {
            double tempDensity;

            if (!dust)
                tempDensity = 0.0;
            else
                tempDensity = dustDensity;

            if (((mass < criticalMass) || (!gas)))
            {
                gasDensity = 0;
                massDensity = tempDensity;
            }
            else
            {

                massDensity = PhysicalConstants.K*dustDensity/
                       (1.0 + Math.Sqrt(criticalMass/mass)*(PhysicalConstants.K - 1.0));
                gasDensity = massDensity - tempDensity;

            }
        }

        static double RandomEccentricity()
        {
            return (1.0 - Math.Pow(rnd.NextDouble(), PhysicalConstants.EccentricityCoefficient));
        }
    }
}