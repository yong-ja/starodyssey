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

#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration;

#endregion

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    public class Star : CelestialBody
    {
        #region Private fields

        private SolarSystem solarSystem;
        readonly StellarFeatures stellarFeatures;
        readonly StellarFeatures mainSequenceFeatures;
        EvolutionResult evolutionResult;


        #endregion

        #region Properties

        public SolarSystem SolarSystem
        {
            get { return solarSystem; }
            internal set { solarSystem = value; }
        }

        public EvolutionResult EvolutionResult
        {
            get { return evolutionResult; }
        }

        public StellarFeatures StellarFeatures
        {
            get { return stellarFeatures; }
        }

        public StellarFeatures MainSequenceFeatures
        {
            get { return mainSequenceFeatures; }
        }

        #endregion

        public Star(string name,
                    StellarFeatures stellarFeatures)
            : this(name, stellarFeatures, stellarFeatures, EvolutionResult.MainSequence){}

        public Star(string name,
            StellarFeatures stellarFeatures, StellarFeatures mainSequenceFeatures,
            EvolutionResult evolutionResult) :
            base(name)
        {
            this.stellarFeatures = stellarFeatures;
            this.mainSequenceFeatures = mainSequenceFeatures;
            this.evolutionResult = evolutionResult;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Name);
            sb.Append(stellarFeatures.ToString());
            foreach (CelestialBody celestialBody in this)
            {
                sb.Append(celestialBody.ToString());
            }

            return sb.ToString();
        }

    }
}