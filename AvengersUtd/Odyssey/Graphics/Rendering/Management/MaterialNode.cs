using System;
using System.Linq;
using AvengersUtd.Odyssey.Utils;
using AvengersUtd.Odyssey.Utils.Collections;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class MaterialNode : SceneNode
    {
        static int count;
        IMaterial material;

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

        public MaterialNode(IMaterial material)
            : base(Text.GetCapitalLetters(typeof(FreeTransformNode).GetType().Name) + '_' + ++count, SceneNodeType.Material)
        {
            this.Material = material;

        }

        #endregion

        public IMaterial Material
        {
            get { return material; }
            set {
                if (material == value) return;
                OnMaterialChanged(this, new NodeEventArgs(this));
                material = value;
            }
        }

        public override void Init()
        {
            material.SetParentNode(this);
            base.Init();
        }

        public override void Update()
        {
            
        }

        

    }
}
