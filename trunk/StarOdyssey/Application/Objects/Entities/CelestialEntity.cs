using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.MultiversalRuleSystem.Space;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using SlimDX;

namespace AvengersUtd.StarOdyssey.Objects.Entities
{
    public class CelestialEntity : BaseEntity
    {
        const double orbitalRadiusMultiplier = 100;
        const double orbitalRadiusOffset = 130;

        double orbitalPosition;
        double orbitalDelta;

        CelestialEntity primary;
        OrbitalParameters orbitalParameters;
        

        public CelestialEntity(EntityDescriptor entityDesc, OrbitalParameters orbitalParameters, CelestialEntity primary) : base(entityDesc)
        {
            this.orbitalParameters = orbitalParameters;
            this.primary = primary;

            orbitalDelta = 30/365.0 * (360 / orbitalParameters.OrbitalPeriod);

        }

        public override void UpdatePosition()
        {
            double h=0, k=0;
            if (primary != null)
            {
                h = primary.PositionV3.X;
                k = primary.PositionV3.Z;
            }

            double a = orbitalRadiusOffset + orbitalParameters.SemiMajorAxis * orbitalRadiusMultiplier;
            double b = orbitalRadiusOffset + orbitalParameters.SemiMinorAxis * orbitalRadiusMultiplier; 
            
            double x = h + a * Math.Cos(orbitalPosition);
            double z = k + b * Math.Sin(orbitalPosition);

            orbitalPosition += orbitalDelta * Game.FrameTime;

            positionV3 = new Vector3((float)x, 0, (float)z);
        }

    }
}
