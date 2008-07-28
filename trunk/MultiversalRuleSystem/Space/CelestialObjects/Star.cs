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

using System.Text;

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    #endregion

    public class Star : IEnumerable<CelestialObject>
    {
        #region Private fields

        string name;
        SolarSystem solarSystem;
        readonly StellarFeatures stellarFeatures;
        SortedList<double, CelestialObject> celestialObjects;

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public SolarSystem SolarSystem
        {
            get { return solarSystem; }
            internal set { solarSystem = value; }
        }

        public StellarFeatures StellarFeatures
        {
            get { return stellarFeatures; }
        }

        public int CelestialObjectsCount
        {
            get { return celestialObjects.Values.Count; }
        }

        public CelestialObject this[int index]
        {
            get { return celestialObjects.Values[index]; }
        }


        #endregion

        #region Constructors

        public Star(string name, StellarFeatures stellarFeatures)
        {
            this.name = name;
            this.stellarFeatures = stellarFeatures;
            celestialObjects = new SortedList<double, CelestialObject>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(name);
            sb.AppendLine(stellarFeatures.ToString());
            return sb.ToString();
        }

        #endregion

        public void AddObjects(params CelestialObject[] celestialObjects)
        {
            foreach (CelestialObject celestialObject in celestialObjects)
            {
                celestialObject.Primary = this;
                this.celestialObjects.Add(celestialObject.CelestialFeatures.OrbitalRadius, celestialObject);
            }
        }

        #region IEnumerable<CelestialObject> Members

        IEnumerator<CelestialObject> IEnumerable<CelestialObject>.GetEnumerator()
        {
            foreach (CelestialObject celestialObject in celestialObjects.Values)
                yield return celestialObject;
        }

        #endregion


        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (CelestialObject celestialObject in celestialObjects.Values)
                yield return celestialObject;
        }

        #endregion
    }
}