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
    using System.Collections.Generic;

    #endregion

    public class SolarSystem
    {
        #region Private fields

        const int MaximumStars = 3;
        const int MaximumOrbits = 12;

        int starCount;
        string name;
        List<Star> stars;

        double x;
        double y;

        #endregion

        #region Properties

        public Star Primary
        {
            get { return stars[0]; }
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

        public SolarSystem(string name)
        {
            this.name = name;
            stars = new List<Star>(MaximumStars);
       }

        public void AddStars(params Star[] stars)
        {
            this.stars.AddRange(stars);
            foreach (Star star in stars)
                star.SolarSystem = this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Star s in stars)
                sb.AppendLine(s.ToString());            
            return sb.ToString();
        }

        #endregion
    }
}