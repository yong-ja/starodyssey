#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

#endregion

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public struct RenderableCollectionDescription : IEquatable<RenderableCollectionDescription>,
                                                    IComparable<RenderableCollectionDescription>
    {
        public string TechniqueName { get; set; }
        public Type PreferredRenderCommandType { get; set; }
        public PrimitiveTopology PrimitiveTopology { get; set; }
        public bool CommonResources { get; set; }
        public RenderingOrderType RenderingOrderType { get; set; }
        public Format IndexFormat { get; set; }
        public InputElement[] InputElements { get; set; }

        #region IComparable<RenderableCollectionDescription> Members

        public int CompareTo(RenderableCollectionDescription other)
        {
            if (RenderingOrderType == RenderingOrderType.OpaqueGeometry &&
                other.RenderingOrderType != RenderingOrderType.OpaqueGeometry)
                return -1;
            if (RenderingOrderType != RenderingOrderType.OpaqueGeometry &&
                other.RenderingOrderType == RenderingOrderType.OpaqueGeometry)
                return 1;
            else return 0;
        }

        #endregion

        #region IEquatable<RenderableCollectionDescription> Members

        public bool Equals(RenderableCollectionDescription other)
        {
            return TechniqueName == other.TechniqueName &&
                   PreferredRenderCommandType == other.PreferredRenderCommandType &&
                   PrimitiveTopology == other.PrimitiveTopology && CommonResources == other.CommonResources &&
                   RenderingOrderType == other.RenderingOrderType && IndexFormat == other.IndexFormat;
        }

        #endregion

        public static bool operator ==(RenderableCollectionDescription left, RenderableCollectionDescription right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RenderableCollectionDescription left, RenderableCollectionDescription right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (RenderableCollectionDescription)) return false;
            return Equals((RenderableCollectionDescription) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = PrimitiveTopology.GetHashCode();
                result = (result*397) ^ PreferredRenderCommandType.GetHashCode();
                result = (result*397) ^ CommonResources.GetHashCode();
                result = (result*397) ^ RenderingOrderType.GetHashCode();
                result = (result*397) ^ (InputElements != null ? InputElements.GetHashCode() : 0);
                result = (result*397) ^ IndexFormat.GetHashCode();
                return result;
            }
        }
    }

    public class RenderableCollection : SceneNodeCollection<RenderableNode>
    {
        public RenderableCollection(RenderableCollectionDescription rDescription, IEnumerable<RenderableNode> rNodeCollection) : base(rNodeCollection)
        {
            Contract.Requires<NullReferenceException>(rNodeCollection != null);
            Description = rDescription;
        }

        public RenderableCollection(RenderableCollectionDescription rDescription)
        {
            Description = rDescription;
        }

        public RenderableCollectionDescription Description { get; set; }
    }
}