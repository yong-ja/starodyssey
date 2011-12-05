using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public partial class Designer
    {
        public void DrawPoints()
        {
            CheckParameters(Options.Shader);

            SaveState();
            Width = Height  = 4;
            IGradientShader tempShader = Shader;
            for (int i = 0; i < Points.Length; i++)
            {
                Vector4 point = Points[i];
                if (i==0)
                {
                    Shader = LinearShader.CreateUniform(new Color4(0, 1, 0));
                }
                else if (i == 1)
                {
                    Shader = LinearShader.CreateUniform(new Color4(1, 1, 0));
                }
                else if (i == 5)
                {
                    Shader = LinearShader.CreateUniform(new Color4(1, 0, 1));
                }
                //else if (i == 7)
                //{
                //    Shader = LinearShader.CreateUniform(new Color4(0, 0, 1));
                //}
                else
                {
                    Shader = tempShader;
                }

                Position = new Vector3(point.X, point.Y, point.Z);
                FillRectangle();
            }
            RestoreState();
        }
    }
}
