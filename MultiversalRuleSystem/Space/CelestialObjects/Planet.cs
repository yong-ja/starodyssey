using System;
using System.Collections.Generic;
using System.Text;

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    public class Planet : CelestialBody
    {
        CelestialFeatures celestialFeatures;
        PlanetaryFeatures planetaryFeatures;

        public PlanetaryFeatures PlanetaryFeatures
        {
            get { return planetaryFeatures; }
            set { planetaryFeatures = value; }
        }

        public CelestialFeatures CelestialFeatures
        {
            get { return celestialFeatures; }
            set { celestialFeatures = value; }
        }

        public string OldTitle
        {
            get { return string.Format("{4}: {0} {1} world - M: {2:f3} - {3}", planetaryFeatures.Size,
                planetaryFeatures.Climate, celestialFeatures.Mass, planetaryFeatures.AtmosphericDensity, Name); }
        }

        public string Title
        {
            get
            {
                return string.Format("{0} {1} world", planetaryFeatures.Size,
                                     planetaryFeatures.Climate);
            }
        }

        public Planet(string name, CelestialFeatures celestialFeatures, PlanetaryFeatures planetaryFeatures):
            base(name)
        {
            this.celestialFeatures = celestialFeatures;
            this.planetaryFeatures = planetaryFeatures;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Name);
            sb.AppendLine(celestialFeatures.ToString());
            sb.AppendLine(planetaryFeatures.ToString());
            if (CelestialBodiesCount > 0)
            {
                sb.AppendLine("---------");
                for (int i = 0; i < CelestialBodiesCount; i++)
                {
                    Planet moon = (Planet) this[i];
                    sb.AppendLine(string.Format("({0}) {1}", i + 1, moon.OldTitle));
                }
            }
            sb.AppendLine("---------\n");
            return sb.ToString();
        }
    }
}
