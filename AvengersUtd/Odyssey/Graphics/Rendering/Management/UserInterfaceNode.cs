using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class UserInterfaceNode : MaterialNode
    {
        public IMaterial TextMaterial { get; private set; }

        public UserInterfaceNode() : base(new UIMaterial())
        {
            TextMaterial = new TextMaterial();
        }

        public RenderableCollection GetTextNodes()
        {
            return new RenderableCollection(TextMaterial.RenderableCollectionDescription, SelectDescendants<RenderableNode>());
        }
    }
}
