#region Disclaimer

/* 
 * PlanetaryFeatures
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

#endregion

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    public class PlanetaryFeatures
    {
        #region Private fields

        string name;
        PlanetSize size;
        Density density;
        Temperature temperature;
        Gravity gravity;
        AtmosphericDensity atmosphericDensity;
        Composition composition;
        PlanetClass climate;
        double radius;

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public PlanetSize Size
        {
            get { return size; }
            set { size = value; }
        }

        public Density Density
        {
            get { return density; }
            set { density = value; }
        }

        public Gravity Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }

        public Temperature Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        public AtmosphericDensity AtmosphericDensity
        {
            get { return atmosphericDensity; }
            set { atmosphericDensity = value; }
        }

        public Composition Composition
        {
            get { return composition; }
            set { composition = value; }
        }

        public PlanetClass Climate
        {
            get { return climate; }
            set { climate = value; }
        }

        #endregion

        #region Constructors

        public PlanetaryFeatures(string name, PlanetSize size, Density density, Gravity gravity,
                                 Temperature temperature,
                                 AtmosphericDensity atmosphericDensity, Composition composition,
                                 PlanetClass climate, double radius)
        {
            this.name = name;
            this.size = size;
            this.density = density;
            this.gravity = gravity;
            this.temperature = temperature;
            this.atmosphericDensity = atmosphericDensity;
            this.composition = composition;
            this.climate = climate;
            this.radius = radius;
        }

        #endregion

        public override string ToString()
        {
            if (climate != PlanetClass.GasGiant)
                return string.Format("{0} {1} world", size.ToString(), climate.ToString());
            else
                return string.Format("{0} {1}", size.ToString(), climate.ToString());
        }
    }
}