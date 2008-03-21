#region Disclaimer

/* 
 * Star
 *
 * Created on 30 agosto 2007
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

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    #region Using Directives

    using System;

    #endregion

    public class Star
    {
        #region Private fields

        SolarSystem solarSystem;
        readonly StellarFeatures stellarFeatures;
        Star nextCompanion;

        #endregion

        #region Properties

        public SolarSystem SolarSystem
        {
            get { return solarSystem; }
            internal set { solarSystem = value; }
        }

        public StellarFeatures StellarFeatures
        {
            get { return stellarFeatures; }
        }

        public Star NextCompanion
        {
            get { return nextCompanion; }
            set { nextCompanion = value; }
        }

        #endregion

        #region Constructors

        public Star(StellarFeatures stellarFeatures)
        {
            this.stellarFeatures = stellarFeatures;
        }

        public override string ToString()
        {
            return stellarFeatures.ToString();
        }

        #endregion
    }
}