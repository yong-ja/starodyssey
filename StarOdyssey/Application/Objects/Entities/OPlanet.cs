using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.StarOdyssey.Resources;
using AvengersUtd.MultiversalRuleSystem.Space;
using SlimDX;

namespace AvengersUtd.StarOdyssey.Objects.Entities
{
    public class OPlanet : CelestialEntity, ISphere
    {
        float radius;
        Planet mrsPlanet;


        public OPlanet(EntityDescriptor eDesc, CelestialEntity primary, Planet planet) : 
            base(eDesc,planet.CelestialFeatures.OrbitalParameters, primary)
        {
            mrsPlanet = planet;

            switch (planet.PlanetaryFeatures.Size)
            {
                case PlanetSize.Asteroid:
                    radius = 0;
                    break;
                case PlanetSize.Tiny:
                    radius = 2;
                    break;
                case PlanetSize.Small:
                    radius = 5;
                    break;
                case PlanetSize.Medium:
                    radius = 7;
                    break;
                case PlanetSize.Large:
                    radius = 10;
                    break;
                case PlanetSize.Huge:
                    radius = 15;
                    break;
                case PlanetSize.Immense:
                    radius = 20;
                    break;
                case PlanetSize.Neptunian:
                    radius = 30;
                    break;
                case PlanetSize.SubJovian:
                    radius = 50;
                    break;
                case PlanetSize.Jovian:
                    radius = 75;
                    break;
                case PlanetSize.SuperJovian:
                    radius = 100;
                    break;
                case PlanetSize.MacroJovian:
                    radius = 125;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

       public static OPlanet ConvertFromPlanet(Planet planet, CelestialEntity primary)
        {
            EntityDescriptor eDesc = ResourceLocator.GetRandomEntityDescriptor(planet.PlanetaryFeatures);
            return new OPlanet(eDesc, primary, planet);    
        }


       #region ISphere Members

       Vector3 ISphere.Center
       {
           get { return positionV3; }
       }

       float ISphere.Radius
       {
           get { return radius; }
       }

        BoundingSphere ISphere.BoundingSphere
        {
            get { return new BoundingSphere(positionV3, radius); }
        }


       #endregion
    }
}
