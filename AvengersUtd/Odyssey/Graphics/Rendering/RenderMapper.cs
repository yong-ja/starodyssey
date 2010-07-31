using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
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

        MaterialNode FindBy(string techniqueId)
        {
            foreach (MaterialNode mNode in Keys)
            {
                if (mNode.Material.TechniqueName == techniqueId)
                    return mNode;
            }
            return null;
        }



        public RenderableCollection this[string techniqueID]
        {
            get { 
                MaterialNode mNode = FindBy(techniqueID);
                if (mNode == null)
                    throw new KeyNotFoundException(ResourceDictionary.ERR_TechniqueNotFound);

                return this[mNode];
            }
        }
    }
}
