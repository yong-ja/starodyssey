using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using SlimDX.Direct3D11;
using ResourceDictionary = AvengersUtd.Odyssey.Properties.Resources;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderMapper : Dictionary<IMaterial, RenderableCollection>
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

        IMaterial FindBy(string techniqueId)
        {
            return Keys.FirstOrDefault(material => material.TechniqueName == techniqueId);
        }

        IEnumerable<IMaterial> Select(RenderingOrderType renderingOrderType)
        {
            return (from material in Keys
                    where material.RenderableCollectionDescription.RenderingOrderType == renderingOrderType
                    select material);
        }

        public IEnumerable<IMaterial> OpaqueToTransparent
        {
            get
            {
                foreach (IMaterial material in Select(RenderingOrderType.First))
                    yield return material;

                foreach (IMaterial material in Select(RenderingOrderType.OpaqueGeometry))
                    yield return material;

                foreach (IMaterial material in Select(RenderingOrderType.MixedGeometry))
                    yield return material;

                foreach (IMaterial material in Select(RenderingOrderType.AdditiveBlendingGeometry))
                    yield return material;

                foreach (IMaterial material in Select(RenderingOrderType.Last))
                    yield return material;
            }
        }

        public RenderableCollection this[string techniqueId]
        {
            get {
                IMaterial material = FindBy(techniqueId);
                if (material == null)
                    throw Error.KeyNotFound(techniqueId, GetType().Name, ResourceDictionary.ERR_TechniqueNotFound);

                return this[material];
            }
        }
    }
}
