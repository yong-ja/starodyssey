#region Disclaimer

/* 
 * EnhancedRandom
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

namespace AvengersUtd.MultiversalRuleSystem.Space
{
    #region Using Directives

    using System;

    #endregion

    public sealed class EnhancedRandom : Random
    {
        #region Private fields

        bool normDone;
        double normValue;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public EnhancedRandom() : base()
        {
        }

        public EnhancedRandom(int seed)
            : base(seed)
        {
        }

        #endregion

        public double Gaussian()
        {
            double v1, v2, s, multiplier;

            if (normDone)
            {
                normDone = false;
                return normValue;
            }
            else
            {
                do
                {
                    v1 = 2*NextDouble() - 1; // between -1.0 and 1.0
                    v2 = 2*NextDouble() - 1; // between -1.0 and 1.0
                    s = v1*v1 + v2*v2;
                } while (s >= 1 || s == 0);

                multiplier = Math.Sqrt(-2*Math.Log(s)/s);
                normValue = v1*multiplier;
                normDone = true;
                return v2*multiplier;
            }
        }

        /// <summary>
        /// Produces a random variate whose natural logarithm is from the
        /// Gaussian with mean = 0 and the specified deviation.
        /// </summary>
        /// <param name="sigma">Sigma Standard deviation</param>
        /// <returns></returns>
        public double LogNormalDeviate(double sigma)
        {
            return (Math.Exp((Gaussian()*sigma)));
        }

        /// <summary>
        /// Returns a uniformly distributed random real number between the specified
        /// limits.
        /// </summary>
        /// <param name="minimum">Inclusive minimum limit.</param>
        /// <param name="maximum">Exclusive maximum limit.</param>
        /// <returns></returns>
        public double NextDouble(double minimum, double maximum)
        {
            return (NextDouble()*(maximum - minimum) + minimum);
        }

        /// <summary>
        /// Returns a value within a certain uniform variation from the central value.
        /// </summary>
        /// <param name="value">Central value.</param>
        /// <param name="variation">Maximum (uniform) variation above or below center.</param>
        /// <returns></returns>
        public double About(double value, double variation)
        {
            return (value + (value*NextDouble(-variation, variation)));
        }

        public double AboutLow(double value, double variation)
        {
            return (value + (value * NextDouble(-variation, 0)));
        }

        public double AboutHigh(double value, double variation)
        {
            return (value + (value * NextDouble(0,variation)));
        }
    }
}