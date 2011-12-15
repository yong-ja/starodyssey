using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class SceneNodeCollection<TNode> : Collection<TNode>
        where TNode : SceneNode
    {

        public SceneNodeCollection(IEnumerable<TNode> nodes) : this(nodes.ToList())
        {}

        public SceneNodeCollection(IList<TNode> collection)
            : base(collection)
        {
        }

        public SceneNodeCollection()
        {
        }

        public void SortBy(Comparison<SceneNode> comparison)
        {
            var nodeList = (List<SceneNode>) Items;
            nodeList.Sort(comparison);
        }
    }
}