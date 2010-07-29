using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Utils.Collections;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public class MaterialNode : SceneNode
    {
        static int count;
        const string nodeTag = "MAT_";
        AbstractMaterial material;

        #region Events
        static readonly object EventMaterialChanged;
        public event NodeEventHandler MaterialChanged
        {
            add { eventHandlerList.AddHandler(EventMaterialChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventMaterialChanged, value); }
        }

        protected virtual void OnMaterialChanged(object sender, NodeEventArgs e)
        {
            Update();
            EventHandler handler = (EventHandler)eventHandlerList[EventMaterialChanged];
            if (handler != null)
                handler(this, e);

        } 
        #endregion

        #region Constructor
        static MaterialNode()
        {
            EventMaterialChanged = new object();
        }

        public MaterialNode(string label, AbstractMaterial material)
            : base(label, SceneNodeType.Material)
        {
            this.material = material;
            material.OwningNode = this;
        }

        public MaterialNode(AbstractMaterial material)
            : this(nodeTag + count++, material)
        {
        }

        #endregion

        public AbstractMaterial Material
        {
            get { return material; }
            set {
                if (material != value) 
                {
                    OnMaterialChanged(this, new NodeEventArgs(this));
                    material = value;
                }
            }
        }

        public override void Update()
        {
            
        }

        protected override object OnClone()
        {
            return new MaterialNode(material);
        }

        public override void Init()
        {
            material.InitParameters();
            base.Init();
        }
    }
}
