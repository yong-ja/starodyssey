#region Disclaimer

/* 
 * PictureBox
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
    using Microsoft.DirectX.Direct3D;
#else
using SlimDX;
using SlimDX.Direct3D9;
#endif

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class PictureBox : SimpleShapeControl, ISpriteControl
    {
        Texture texture;

        public Texture Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public PictureBox()
        {
            ApplyControlStyle(ControlStyle.EmptyStyle);
        }

        #region ISpriteControl Members

        public void Render()
        {
            OdysseyUI.CurrentHud.SpriteManager.Draw(texture, new Rectangle(Point.Empty, Size), new Vector3(),
                                                    new Vector3(AbsolutePosition.X, AbsolutePosition.Y, 0), Color.White);
        }

        #endregion

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        protected override void UpdatePositionDependantParameters()
        {
            return;
        }
    }
}