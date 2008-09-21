using System;
using AvengersUtd.MultiversalRuleSystem.Space;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX;

namespace AvengersUtd.StarOdyssey.Objects.Entities
{
    public class OSolarSystem : SceneGraph
    {
        public OSolarSystem(SceneNode rootNode) : base(rootNode)
        {
        }

        public static OSolarSystem ConvertFromSolarSystem(SolarSystem solarSystem)
        {

            FixedNode centerOfMass = new FixedNode();
            centerOfMass.Label = "FN_CenterOfMass";

            OStar primaryStar = OStar.ConvertFromStar(solarSystem.Primary, null);

            if (solarSystem.IsSingleStarsystem)
            {
                FixedNode primaryNode = new FixedNode(solarSystem.Primary.Name, primaryStar.PositionV3);
                RenderableNode primaryObjectNode = new RenderableNode(primaryStar);
                primaryNode.AppendChild(primaryObjectNode);
                
                centerOfMass.AppendChild(primaryNode);
            }
            else
            {
                for (int i=1; i< solarSystem.Stars.Length; i++)
                {
                    OrbitalParameters orbitalParameters = solarSystem.Primary.StellarFeatures.OrbitalParameters;
                    EllipticalOrbitNode starNode = new EllipticalOrbitNode(solarSystem.Primary.Name,
                    orbitalParameters.SemiMajorAxis, orbitalParameters.SemiMinorAxis, orbitalParameters.Eccentricity,
                        orbitalParameters.OrbitalDelta);

                    centerOfMass.AppendChild(starNode);
                    RenderableNode starObjectNode = new RenderableNode(OStar.ConvertFromStar(solarSystem.Stars[i], primaryStar));

                    starNode.AppendChild(starObjectNode);
                    starNode.Label = solarSystem.Primary.Name;
                }
            }

            foreach (CelestialBody cBody in solarSystem.Primary)
            //for (int i = 0; i < 3; i++ )
            {
                //CelestialBody cBody = solarSystem.Primary[i];
                Planet planet = (Planet)cBody;
                OrbitalParameters orbitalParameters = planet.CelestialFeatures.OrbitalParameters;
                OPlanet planetEntity = OPlanet.ConvertFromPlanet(planet, primaryStar);
                const double orbitalRadiusMultiplier = 100;
                const double orbitalRadiusOffset = 100;
                double a = orbitalRadiusOffset + orbitalParameters.SemiMajorAxis * orbitalRadiusMultiplier;
                double b = orbitalRadiusOffset + orbitalParameters.SemiMinorAxis * orbitalRadiusMultiplier;

                EllipticalOrbitNode planetNode = new EllipticalOrbitNode(planet.Name,
                    a, b, orbitalParameters.Eccentricity,
                        orbitalParameters.OrbitalDelta);

                planetNode.AppendChild(new RenderableNode(planetEntity));
                centerOfMass.FirstChild.AppendChild(planetNode);
            }

            return new OSolarSystem(centerOfMass);
        }
    }

    

}
