using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public class EllipticalOrbitNode : TransformNode
    {
        const string nodeTag = "EoN_";
        static int count;
        double semiMajorAxis;
        double semiMinorAxis;
        double eccentricity;
        double orbitalPosition;
        double orbitalDelta;

        Vector3 position;

        #region Properties
        public Vector3 Position
        {
            get { return position; }
        }

        public double SemiMajorAxis
        {
            get { return semiMajorAxis; }
            set { semiMajorAxis = value; }
        }

        public double SemiMinorAxis
        {
            get { return semiMinorAxis; }
            set { semiMinorAxis = value; }
        }

        public double Eccentricity
        {
            get { return eccentricity; }
            set { eccentricity = value; }
        }

        public double OrbitalDelta
        {
            get { return orbitalDelta; }
            set { orbitalDelta = value; }
        } 
        #endregion

        public EllipticalOrbitNode(double semiMajorAxis, double semiMinorAxis, double eccentricity, double orbitalDelta)
            : this((count++).ToString(), semiMajorAxis, semiMinorAxis, eccentricity, orbitalDelta)
        {}

        public EllipticalOrbitNode(string label, double semiMajorAxis, double semiMinorAxis, double eccentricity, double orbitalDelta)
            : base(nodeTag + label, true)
        {
            this.semiMajorAxis = semiMajorAxis;
            this.semiMinorAxis = semiMinorAxis;
            this.eccentricity = eccentricity;
            this.orbitalDelta = orbitalDelta;
        }

        public override void UpdateLocalWorldMatrix()
        {
            double a = semiMajorAxis;
            double b = semiMinorAxis;

            double x = a * Math.Cos(orbitalPosition);
            double z = b * Math.Sin(orbitalPosition);

            orbitalPosition += orbitalDelta;// * Game.FrameTime;
            orbitalPosition %= 360.0f;

            position = new Vector3((float)x, 0, (float)z);

            LocalWorldMatrix = Matrix.Translation(position);
        }

        protected override object OnClone()
        {
            return new EllipticalOrbitNode(Label,
                                           semiMajorAxis, semiMinorAxis, eccentricity, orbitalDelta);
        }
    }
}
