﻿using System;
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
        FillShader = 8,
        BorderShader = 16,
    }

    internal struct MainParameters
    {
        public Vector3 Position { get; private set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Thickness BorderSize { get; set; }
        public ColorShader FillShader { get; set; }
        public ColorShader BorderShader { get; set; }

        internal static MainParameters FromDesigner(Designer designer)
        {
            return new MainParameters
                       {
                               Position = designer.Position,
                               Width = designer.Width,
                               Height = designer.Height,
                               FillShader = designer.FillShader,
                               BorderShader = designer.BorderShader
                       };
        }
    }

    public partial class Designer
    {
        public Vector3 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Thickness BorderSize { get; set; }
        public ColorShader FillShader { get; set; }
        public ColorShader BorderShader { get; set; }

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

        void SaveState()
        {
            parameterStack.Push(MainParameters.FromDesigner(this));
        }

        void RestoreState()
        {
            MainParameters mainParameters = parameterStack.Pop();
            Position = mainParameters.Position;
            Width = mainParameters.Width;
            Height = mainParameters.Height;
            BorderSize = mainParameters.BorderSize;
            FillShader = mainParameters.FillShader;
            BorderShader = mainParameters.BorderShader;
        }

        static bool CheckFlag(Options flags, Options check)
        {
            return ((flags & check) == check);
        }


        void CheckParameters(Options flags)
        {
            // Position = Empty is allowed

            if (CheckFlag(flags, Options.Size))
            {
                if (Width == 0 || Height == 0)               
                    throw Error.ArgumentInvalid("Size", typeof (Designer), "CheckParameters");
            }


            if (CheckFlag(flags, Options.BorderSize))
            {
                if (BorderSize.IsEmpty)
                    throw Error.ArgumentInvalid("BorderSize", typeof(Designer), "CheckParameters");
            }
            
            if (CheckFlag(flags, Options.FillShader))
            {
                if (FillShader == null)
                    throw Error.ArgumentInvalid("FillShader", typeof (Designer), "CheckParameters");
            }

            if (CheckFlag(flags, Options.BorderShader))
            {
                if (FillShader == null)
                    throw Error.ArgumentInvalid("BorderShader", typeof(Designer), "CheckParameters");
            }

        }
    }
}
