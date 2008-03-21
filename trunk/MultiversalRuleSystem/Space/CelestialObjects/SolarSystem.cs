#region Disclaimer

/* 
 * SolarSystem
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

    public class SolarSystem
    {
        #region Private fields

        const int maximumOrbits = 12;

        int starCount;
        string name;
        Star primary;
        CelestialObject[] celestialObjects;

        double x;
        double y;

        #endregion

        #region Properties

        public Star Primary
        {
            get { return primary; }
        }

        public string Name
        {
            get { return name; }
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool IsSingleStarsystem
        {
            get { return starCount == 1; }
        }

        public bool IsBinaryStarSystem
        {
            get { return starCount == 2; }
        }

        public bool IsTrinaryStarSystem
        {
            get { return starCount == 3; }
        }

        #endregion

        #region Constructors

        public SolarSystem(string name, Star primary)
        {
            this.name = name;
            this.primary = primary;
            celestialObjects = new CelestialObject[maximumOrbits];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(primary.ToString());
            //foreach (CelestialObject c in celestialObjects)
            //{
            //    sb.AppendLine(c.ToString());
            //}
            return sb.ToString();
        }

        #endregion
    }
}