﻿#region #Disclaimer

// Author: Adalberto L. Simeone (Taranto, Italy)
// E-Mail: avengerdragon@gmail.com
// Website: http://www.avengersutd.com/blog
// 
// This source code is Intellectual property of the Author
// and is released under the Creative Commons Attribution 
// NonCommercial License, available at:
// http://creativecommons.org/licenses/by-nc/3.0/ 
// You can alter and use this source code as you wish, 
// provided that you do not use the results in commercial
// projects, without the express and written consent of
// the Author.

#endregion

#region Using directives

using SlimDX;

#endregion

namespace AvengersUtd.Odyssey.Geometry
{
    public interface IPolygon
    {

        Vertices Vertices { get; }
        Vector2D Centroid { get; }
        double Area { get; }
        Vector4[] ComputeVector4Array(float zIndex);
        bool IsPointInside(Vector2D point);
    }
}