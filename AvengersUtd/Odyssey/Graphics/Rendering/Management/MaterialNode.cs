using System;
using System.Linq;
using AvengersUtd.Odyssey.Utils.Collections;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class MaterialNode : SceneNode
    {
        static int count;
        const string NodeTag = "MAT_";
        AbstractMaterial material;

        #region Events
        static readonly object EventMaterialChanged;

        public event NodeEventHandler MaterialChanged
        {
            add { EventHandlerList.AddHandler(EventMaterialChanged, value); }
            remove { EventHandlerList.RemoveHandler(EventMaterialChanged, value); }
        }

        protected virtual void OnMaterialChanged(object sender, NodeEventArgs e)
        {
            Update();
            EventHandler handler = (EventHandler)EventHandlerList[EventMaterialChanged];
            if (handler != null)
                handler(this, e);

        } 
        #endregion

        #region Constructor
        static MaterialNode()
        {
            EventMaterialChanged = new object();
        }

        public MaterialNode(AbstractMaterial material)
            : base(NodeTag + (++count), SceneNodeType.Material)
        {
            this.material = material;
            material.OwningNode = this;
        }

        #endregion

        public AbstractMaterial Material
        {
            get { return material; }
            set {
                if (material == value) return;
                OnMaterialChanged(this, new NodeEventArgs(this));
                material = value;
            }
        }

        public RenderableCollection RenderableCollection
        {
            get
            {
                RenderableCollection rCollection = new RenderableCollection(material.RenderableCollectionDescription);
                foreach (RenderableNode rNode in PostOrderVisit(this).OfType<RenderableNode>())
                {
                    rCollection.Add(rNode);
                }
                return rCollection;
            }
        }

        public override void Update()
        {
            
        }

    }
}
