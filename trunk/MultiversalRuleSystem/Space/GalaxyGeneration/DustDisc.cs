#region Disclaimer

/* 
 * DustDisc
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

#region Using Directives

using System;
using System.Collections.Generic;

#endregion

namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{
    public class DustDisc
    {
        #region Private fields

        bool isDustLeft;
        bool isGasAvailable; // true if gas available in current working band
        double cloudEccentricity;
        DustBand dustHead;
        double bodyInnerLimit;
        double bodyOuterLimit;

        #endregion

        #region Properties

        public bool IsDustLeft
        {
            get { return isDustLeft; }
            set { isDustLeft = value; }
        }

        public bool IsGasAvailable
        {
            get { return isGasAvailable; }
            set { isGasAvailable = value; }
        }

        public double CloudEccentricity
        {
            get { return cloudEccentricity; }
            set { cloudEccentricity = value; }
        }

        public DustBand DustHead
        {
            get { return dustHead; }
            set { dustHead = value; }
        }

        public double BodyInnerLimit
        {
            get { return bodyInnerLimit; }
            set { bodyInnerLimit = value; }
        }

        public double BodyOuterLimit
        {
            get { return bodyOuterLimit; }
            set { bodyOuterLimit = value; }
        }

        #endregion

        #region Constructors

        public DustDisc(double dustInnerLimit, double dustOuterLimit, double bodyInnerLimit, double bodyOuterLimit)
        {
            dustHead = new DustBand(dustInnerLimit, dustOuterLimit);
            this.bodyInnerLimit = bodyInnerLimit;
            this.bodyOuterLimit = bodyOuterLimit;
            isDustLeft = true;
        }

        #endregion

        /// <summary>
        /// Determines whether dust is present within the effect radius of
        /// a specific Protoplanet.
        /// </summary>
        /// <param name="p">The specified Protoplanet.</param>
        /// <returns><c>true</c> if there is a band containing dust which this body
        /// can accrete; <c>false</c> otherwise</returns>
        public bool IsDustAvailable(Protoplanet p)
        {
            double insideRange = Protoplanet.InnerSweptLimit(p.A, p.E, p.Mass, cloudEccentricity);
            double outsideRange = Protoplanet.OuterSweptLimit(p.A, p.E, p.Mass, cloudEccentricity);
            bool isDustHere = false;

            for (DustBand curr = dustHead; curr != null; curr = curr.NextBand)
            {
                // check if band has dust left
                if (curr.IsDustPresent && (curr.OuterEdge >= insideRange)
                    && (curr.InnerEdge <= outsideRange))
                {
                    isDustHere = true;
                }
            }

            return isDustHere;

        }

        /// <summary>
        /// Removes a band of dust from the specified DustBand, supplementing it
        /// with 2 new bands.
        /// </summary>
        /// <param name="node1">Band from which dust has been removed.</param>
        /// <param name="min">Inner limit of cleared lane (in AU).</param>
        /// <param name="max">Outer limit of cleared lane (in AU)</param>
        /// <returns>Next band in disc, outside affected band <c>node1</c>.</returns>
        public DustBand SplitBand(DustBand node1, double min, double max)
        {
            DustBand node2 = new DustBand(node1);
            DustBand node3 = new DustBand(node1);
            node2.IsDustPresent = false; // dust sucked up by planetesimal
            node2.IsGasPresent = node1.IsGasPresent && isGasAvailable;
            node2.InnerEdge = min;
            node2.OuterEdge = max;
            node3.InnerEdge = max;
            node1.OuterEdge = min;
            node1.NextBand = node2;
            node2.NextBand = node3;
            return node3.NextBand;
        }

        /// <summary>
        /// Removes outer portion of the specified DustBand, following it
        /// with a new band.
        /// </summary>
        /// <param name="node1">Band from which dust has been removed.</param>
        /// <param name="outer">Inner limit of cleared lane (in AU).</param>
        /// <returns>Next band in disc, outside affected band <c>node1</c>.</returns>
        public DustBand SplitHigh(DustBand node1, double outer)
        {
            DustBand node2 = new DustBand(node1);
            node1.NextBand = node2;
            node1.IsDustPresent = false;
            node1.IsGasPresent = node1.IsGasPresent && isGasAvailable;
            node2.InnerEdge = outer;
            node1.OuterEdge = outer;
            return node2.NextBand;
        }

        /// <summary>
        /// Removes inner portion of the specified DustBand, preceding it
        /// with a new band.
        /// </summary>
        /// <param name="node1">Band from which dust has been removed.</param>
        /// <param name="inner">Outer limit of cleared lane (in AU).</param>
        /// <returns>Next band in disc, outside affected band <c>node1</c>.</returns>
        public DustBand SplitLow(DustBand node1, double inner)
        {
            DustBand node2 = new DustBand(node1);
            node1.NextBand = node2;
            node2.IsDustPresent = false;
            node2.IsGasPresent = node1.IsGasPresent && isGasAvailable;
            node2.InnerEdge = inner;
            node1.OuterEdge = inner;
            return node2.NextBand;
        }

        public void UpdateDustLanes(double min, double max)
        {
            DustBand node1;

            isDustLeft = false;
            // update dust bands under influence of Protoplanet
            node1 = dustHead;
            while (node1 != null)
            {
                IntersectionResult result = node1.Intersect(min, max);
                if (result == (IntersectionResult.Inner | IntersectionResult.Outer))
                {
                    node1 = SplitBand(node1, min, max);
                }
                else if (result == IntersectionResult.Outer)
                {
                    node1 = SplitHigh(node1, max);
                }
                else if (result == IntersectionResult.Inner)
                {
                    node1 = SplitLow(node1, min);
                }
                else if (result == IntersectionResult.Contained)
                {
                    node1.IsDustPresent = false;
                    node1.IsGasPresent = node1.IsGasPresent && isGasAvailable;
                    node1 = node1.NextBand;
                }
                else
                    node1 = node1.NextBand;
            }

            CheckDustLeft();
        }

        /// <summary>
        /// Checks if there is any dust remaining in any bands inside the
        /// bounds where planets can form.
        /// </summary>
        /// <returns><c>true</c> if there is dust left; <c>false</c> otherwise.</returns>
        void CheckDustLeft()
        {
            for (DustBand current = dustHead; current != null; current = current.NextBand)
            {
                isDustLeft |=
                    (current.IsDustPresent &&
                     (current.OuterEdge >= bodyInnerLimit) &&
                     (current.InnerEdge <= bodyOuterLimit));
                while (current.MergeWithNext()) ;
            }
        }

        void CompressDustLanes()
        {
            DustBand NextBand;
            for (DustBand current = dustHead; current != null; current = NextBand)
            {
                NextBand = current.NextBand;
                if (NextBand != null && (current.IsDustPresent == NextBand.IsDustPresent)
                    && (current.IsGasPresent == NextBand.IsGasPresent))
                {
                    current.OuterEdge = NextBand.OuterEdge;
                    current.NextBand = NextBand.NextBand;
                    NextBand = current;
                }
            }
        }

        public void AccreteDust(Protoplanet p)
        {
            double startMass = p.Mass;
            double minimumAccretion = 0.0001*startMass;
            double innerReducedLimit, outerReducedLimit, gatherLast, gatherNow;
            DustBand db;

            gatherNow = 0.0;
            do
            {
                gatherLast = gatherNow;
                // calculate new mass of protoplanet, considering last calculated
                // quantity of accreted matter, then calculate region to be swept
                // based on the updated mass.

                p.Mass = startMass + gatherLast;

                isGasAvailable = !p.AccretesGas;
                innerReducedLimit = Protoplanet.InnerSweptLimit(p.A, p.E, Protoplanet.ComputeReducedMass(p.Mass), cloudEccentricity);
                outerReducedLimit = Protoplanet.OuterSweptLimit(p.A, p.E, Protoplanet.ComputeReducedMass(p.Mass), cloudEccentricity);

                if (innerReducedLimit < 0.0) innerReducedLimit = 0.0;

                // sweep through all dust bands, collecting matter within the
                // effective reach of the protoplanet's gravity.
                gatherNow = 0.0;
                for (db = dustHead; db != null; db = db.NextBand)
                {
                    gatherNow += db.CollectDust(innerReducedLimit, outerReducedLimit, p);
                }
            } while ((gatherNow - gatherLast) >= minimumAccretion);

            UpdateDustLanes(innerReducedLimit, outerReducedLimit);
        }
    }
}