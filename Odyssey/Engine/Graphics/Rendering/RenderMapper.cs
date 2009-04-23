using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderMapper : Dictionary<MaterialNode,SceneNodeCollection>
    {
        public bool ContainsKey(string techniqueId)
        {
            return FindBy(techniqueId) != null;
        }

        MaterialNode FindBy(string techniqueId)
        {
            foreach (MaterialNode mNode in Keys)
            {
                if (mNode.Technique == techniqueId)
                    return mNode;
            }
            return null;
        }



        public SceneNodeCollection this[string techniqueID]
        {
            get {
                MaterialNode mNode = FindBy(techniqueID);
                if (mNode == null)
                    throw new KeyNotFoundException(Properties.Resources.ERR_TechniqueNotFound);

                return this[mNode];
            }
        }
    }
}
