using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    public class CelestialBody: IEnumerable<CelestialBody>
    {
        string name;
        CelestialBody primary;
        List<CelestialBody> celestialBodies;
        
        #region Properties
        public int CelestialBodiesCount
        {
            get { return celestialBodies.Count; }
        }

        public virtual CelestialBody this[int index]
        {
            get { return celestialBodies[index]; }
        }

        public CelestialBody Primary
        {
            get { return primary; }
            internal set { primary = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region IEnumerable<CelestialBody> Members

        IEnumerator<CelestialBody> IEnumerable<CelestialBody>.GetEnumerator()
        {
            foreach (CelestialBody celestialObject in celestialBodies)
                yield return celestialObject;
        }

        #endregion


        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (CelestialBody celestialObject in celestialBodies)
                yield return celestialObject;
        }

        #endregion

        public void AddObject(double semiMajorAxis, CelestialBody celestialBody)
        {

            celestialBody.Primary = this;
            this.celestialBodies.Add(celestialBody);
        }
        public void RemoveObject(CelestialBody celestialBody)
        {
            celestialBodies.Remove(celestialBody);
        }

        #region Constructors

        public CelestialBody(string name)
        {
            this.name = name;
            celestialBodies = new List<CelestialBody>();
        }
        #endregion

    }
}
