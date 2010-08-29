using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using SlimDX.Direct3D11;
using ResourceDictionary = AvengersUtd.Odyssey.Properties.Resources;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderMapper : Dictionary<MaterialNode, RenderableCollection>
    {
        public bool ContainsKey(string techniqueId)
        {
            return FindBy(techniqueId) != null;
        }

        public void Dispose()
        {
            //foreach (MaterialNode mNode in Keys)
            //    mNode.Material.Dispose();

            foreach (RenderableNode rNode in Values.SelectMany(rCollection => rCollection))
            {
                rNode.RenderableObject.Dispose();
            }
        }

        MaterialNode FindBy(string techniqueId)
        {
            return Keys.FirstOrDefault(mNode => mNode.Material.TechniqueName == techniqueId);
        }

        IEnumerable<MaterialNode> Select(RenderingOrderType renderingOrderType)
        {
            return (from mNode in Keys
                    where mNode.Material.RenderableCollectionDescription.RenderingOrderType == renderingOrderType
                    select mNode);
        }

        public IEnumerable<MaterialNode> OpaqueToTransparent
        {
            get
            {
                foreach (MaterialNode mNode in Select(RenderingOrderType.First))
                    yield return mNode;

                foreach (MaterialNode mNode in Select(RenderingOrderType.OpaqueGeometry))
                    yield return mNode;

                foreach (MaterialNode mNode in Select(RenderingOrderType.MixedGeometry))
                    yield return mNode;

                foreach (MaterialNode mNode in Select(RenderingOrderType.AdditiveBlendingGeometry))
                    yield return mNode;

                foreach (MaterialNode mNode in Select(RenderingOrderType.Last))
                    yield return mNode;
            }
        }

        public RenderableCollection this[string techniqueId]
        {
            get { 
                MaterialNode mNode = FindBy(techniqueId);
                if (mNode == null)
                    throw Error.KeyNotFound(techniqueId, GetType().Name, ResourceDictionary.ERR_TechniqueNotFound);

                return this[mNode];
            }
        }
    }
}
