using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    public class PrimaryBody: IEnumerable<CelestialObject>
    {
        private string name;
        SortedList<double, CelestialObject> celestialObjects;
        readonly StellarFeatures stellarFeatures;

        #region Properties
        public int CelestialObjectsCount
        {
            get { return celestialObjects.Values.Count; }
        }

        public CelestialObject this[int index]
        {
            get { return celestialObjects.Values[index]; }
        }
                public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public StellarFeatures StellarFeatures
        {
            get { return stellarFeatures; }
        }

        #endregion

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

        public void AddObjects(params CelestialObject[] celestialObjects)
        {
            foreach (CelestialObject celestialObject in celestialObjects)
            {
                celestialObject.Primary = this;
                this.celestialObjects.Add(celestialObject.CelestialFeatures.OrbitalRadius, celestialObject);
            }
        }

       #region Constructors

        public PrimaryBody(string name, StellarFeatures stellarFeatures)
        {
            this.name = name;
            this.stellarFeatures = stellarFeatures;
            celestialObjects = new SortedList<double, CelestialObject>();
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Name);
            sb.AppendLine(stellarFeatures.ToString());
            return sb.ToString();
        }
    }
}
