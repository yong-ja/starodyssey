#region Disclaimer

/* 
 * CelestialObject
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

using System.Text;

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    #region Using Directives

    using System;

    #endregion

    public class CelestialObject
    {
        #region Private fields

        string name;
        CelestialFeatures celestialFeatures;
        PlanetaryFeatures planetaryFeatures;
        Star primary;

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public CelestialFeatures CelestialFeatures
        {
            get { return celestialFeatures; }
        }

        public PlanetaryFeatures PlanetaryFeatures
        {
            get { return planetaryFeatures; }
        }

        public Star Primary
        {
            get { return primary; }
            internal set { primary = value; }
        }

        #endregion

        #region Constructors

        public CelestialObject(CelestialFeatures celestialFeatures, PlanetaryFeatures planetaryFeatures)
        {
            this.celestialFeatures = celestialFeatures;
            this.planetaryFeatures = planetaryFeatures;
        }

        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(name);
            sb.AppendLine(celestialFeatures.ToString());
            sb.AppendLine(planetaryFeatures.ToString());
            sb.AppendLine("---------");
            return sb.ToString();
        }
    }
}