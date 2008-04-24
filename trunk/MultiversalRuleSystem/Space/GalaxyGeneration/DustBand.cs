#region Disclaimer

/* 
 * DustBand
 *
 * Created on 01 settembre 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Multiversal Rule System Library
 * Originally based on the work of Carl Burke
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

    [Flags]
    public enum IntersectionResult
    {
        None = 0,
        Inner = 1,
        Outer = 2,
        Contained = 4
    }

    public class DustBand
    {
        #region Private fields

        double innerEdge;
        double outerEdge;
        bool isDustPresent;
        bool isGasPresent;
        DustBand nextBand;

        #endregion

        #region Properties

        public double InnerEdge
        {
            get { return innerEdge; }
            set { innerEdge = value; }
        }

        public double OuterEdge
        {
            get { return outerEdge; }
            set { outerEdge = value; }
        }

        public bool IsDustPresent
        {
            get { return isDustPresent; }
            set { isDustPresent = value; }
        }

        public bool IsGasPresent
        {
            get { return isGasPresent; }
            set { isGasPresent = value; }
        }

        public DustBand NextBand
        {
            get { return nextBand; }
            set { nextBand = value; }
        }

        #endregion

        #region Constructors

        public DustBand(double dustInnerLimit, double dustOuterLimit)
        {
            nextBand = null;
            outerEdge = dustOuterLimit;
            innerEdge = dustInnerLimit;
            isDustPresent = true;
            isGasPresent = true;
        }

        public DustBand(DustBand db)
        {
            if (db == null)
                return;
            innerEdge = db.InnerEdge;
            outerEdge = db.OuterEdge;
            isDustPresent = db.IsDustPresent;
            isGasPresent = db.IsGasPresent;
            nextBand = db.NextBand;
        }

        #endregion

        /// <summary>
        /// Calculates the intersection of this dust band with a range of distances 
        /// and returns a mask constructed from the <see cref="IntersectionResult"/>
        /// flags exported by this class.
        /// Typically used to identify bands which overlap the effect radius
        /// of a protoplanet.
        /// </summary>
        /// <param name="inner">Inner edge of the intersecting band (in AU.)</param>
        /// <param name="outer">Outer edge of the intersecting band (in AU).</param>
        /// <returns></returns>
        public IntersectionResult Intersect(double inner, double outer)
        {
            IntersectionResult result = IntersectionResult.None;
            if (outerEdge <= inner || innerEdge >= outer)
                return IntersectionResult.None;
            if (innerEdge < inner)
                result |= IntersectionResult.Inner;
            if (outerEdge > outer)
                result |= IntersectionResult.Outer;
            if (innerEdge >= inner && outerEdge <= outer)
                result |= IntersectionResult.Contained;
            return result;
        }

        public bool IsCompatibleWith(DustBand dustBand)
        {
            return (isDustPresent == dustBand.isDustPresent) &&
                   (isGasPresent == dustBand.isGasPresent);
        }

        /// <summary>
        /// Merge a dust band with its successor, allowing the successor to be
        /// garbage collected.
        /// </summary>
        /// <returns><c>true</c> is the merge was successful; <c>false</c> otherwise.</returns>
        public bool MergeWithNext()
        {
            if (nextBand != null)
            {
                if (IsCompatibleWith(nextBand))
                {
                    outerEdge = nextBand.outerEdge;
                    nextBand = nextBand.nextBand;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Calculate the amount of dust which the specified protoplanet can
        /// accrete from this dust band, if any.
        /// </summary>
        /// <param name="innerReducedLimit">The innermost limit of gravitational influence, using reduced mass.</param>
        /// <param name="outerReducedLimit">The outermost limit of gravitational influence, using reduced mass.</param>
        /// <param name="p">The protoplanet.</param>
        /// <returns>Quantity of dust, in units of solar masses</returns>
        public double CollectDust(double innerReducedLimit, double outerReducedLimit, Protoplanet p)
        {
            double gather = 0.0;

            /* as last_mass increases, temp approaches 1.
             * reduced_mass approaches 1 even quicker.
             * as reduced_mass approaches 1, r_inner approaches 0 and
             * r_outer approaches 2*a*(1.0 + e).  Apparently the integration
             * is from 0 to 2a.
             * The masses are expressed in terms of solar masses; temp is therefore
             * the ratio of the planetary mass to the total system (sun + planet)
             */
            if (Intersect(innerReducedLimit, outerReducedLimit) != IntersectionResult.None)
            {
                double massDensity;
                double gasDensity;
                double bandwidth = (outerReducedLimit - innerReducedLimit);
                double outside = outerReducedLimit - outerEdge;
                double inside = innerEdge - innerReducedLimit;
                double width;
                double term1;
                double term2;
                double volume;

                p.ComputeMassDensity(isDustPresent, isGasPresent, out massDensity, out gasDensity);

                if (bandwidth <= 0.0)
                    bandwidth = 0.0;
                if (outside < 0.0)
                    outside = 0.0;
                if (inside < 0.0) 
                    inside = 0.0;
                width = bandwidth - outside - inside;

                if (width < 0.0)
                    width = 0.0;

                term1 = 4.0 * Math.PI * Math.Pow(p.A, 2.0) * Protoplanet.ComputeReducedMass(p.Mass);
                term2 = (1.0 - p.E * (outside - inside) / bandwidth);
                volume = term1 * width * term2;
                gather = volume * massDensity;
                p.GasMass = volume * gasDensity;
                p.DustMass = gather - p.GasMass;
            }
            return gather;
        }
    }
}