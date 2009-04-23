using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Utils.Collections;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public class MaterialNode : SceneNode
    {
        static int count;
        const string nodeTag = "MAT_";
        MaterialCollection materials;

        public bool HasMultipleMaterials
        {
            get { return materials.Count > 1; }
        }

        public string Technique { get; private set; }

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
            materials = new MaterialCollection { material };
            Technique = material.EffectDescriptor.Technique;
        }

        public MaterialNode(AbstractMaterial material)
            : this(nodeTag + count++, material)
        {
        }

        public MaterialNode(AbstractMaterial[] materials)
            : this((count++).ToString(), materials)
        { }

        public MaterialNode(string label, AbstractMaterial[] materials)
            : base(label, SceneNodeType.Material)
        {
            this.materials = new MaterialCollection();
            this.materials.AddRange(materials);
            Technique = materials[0].EffectDescriptor.Technique;
        } 
        #endregion

        public MaterialCollection Materials
        {
            get { return materials; }
        }

        public override void Update()
        {
            foreach (AbstractMaterial material in materials)
                material.ApplyStaticParameters();
        }

        protected override object OnClone()
        {
            MaterialNode mNode = materials.Count > 1 ? new MaterialNode(materials[0]):
            new MaterialNode(materials.ToArray());
            return mNode;
        }
    }
}
