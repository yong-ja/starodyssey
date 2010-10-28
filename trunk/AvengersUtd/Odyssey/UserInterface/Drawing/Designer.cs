using System;
using System.Collections.Generic;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    [Flags]
    internal enum Options
    {
        None = 0,
        Position =1,
        Size = 2,
        BorderSize = 4,
        Shader = 8,
    }

    internal struct MainParameters
    {
        public Vector3 Position { get; private set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Thickness BorderSize { get; set; }
        public IGradientShader FillShader { get; set; }
        public LinearShader BorderShader { get; set; }

        internal static MainParameters FromDesigner(Designer designer)
        {
            return new MainParameters
                       {
                               Position = designer.Position,
                               Width = designer.Width,
                               Height = designer.Height,
                               FillShader = designer.Shader,
                       };
        }
    }

    public partial class Designer
    {
        public Vector3 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Thickness BorderSize { get; set; }
        public IGradientShader Shader { get; set; }

        private readonly Stack<MainParameters> parameterStack;

        readonly List<ShapeDescription> shapes;

        public ShapeDescription Output
        {
            get { return ShapeDescription.Join(shapes.ToArray()); }
        }

        public Designer()
        {
            shapes = new List<ShapeDescription>();
            parameterStack = new Stack<MainParameters>();
        }

        public void SaveState()
        {
            parameterStack.Push(MainParameters.FromDesigner(this));
        }

        public void RestoreState()
        {
            MainParameters mainParameters = parameterStack.Pop();
            Position = mainParameters.Position;
            Width = mainParameters.Width;
            Height = mainParameters.Height;
            BorderSize = mainParameters.BorderSize;
        }

        static bool CheckFlag(Options flags, Options check)
        {
            return ((flags & check) == check);
        }


        void CheckParameters(Options flags)
        {
            // Position = Empty is allowed

            //if (CheckFlag(flags, Options.Size))
            //{
            //    if (Width == 0 || Height == 0)               
            //        throw Error.ArgumentInvalid("Size", typeof (Designer), "CheckParameters");
            //}


            if (CheckFlag(flags, Options.BorderSize))
            {
                if (BorderSize.IsEmpty)
                    throw Error.ArgumentInvalid("BorderSize", typeof(Designer), "CheckParameters");
            }
            
            if (CheckFlag(flags, Options.Shader))
            {
                if (Shader == null)
                    throw Error.ArgumentInvalid("Shader", typeof (Designer), "CheckParameters");
            }

           
        }
    }
}
