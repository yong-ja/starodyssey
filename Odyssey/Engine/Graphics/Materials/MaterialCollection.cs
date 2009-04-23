using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class MaterialCollection : Collection<AbstractMaterial>,IEnumerable<AbstractMaterial>
    {
        bool isSameMaterial;

        #region Properties
        public bool IsSameMaterial
        {

            get { return isSameMaterial; }
            internal set { isSameMaterial = value; }
        }
        #endregion

        public MaterialCollection(IList<AbstractMaterial> collection)
            : base(collection)
        {
        }

        public MaterialCollection()
        {}

        internal void RenderWithMaterial(AbstractMaterial material)
        {
            Effect effect = material.EffectDescriptor.Effect;
            int passes = effect.Begin(FX.None);
            effect.BeginPass(0);
            foreach (AbstractMaterial currentMaterial in Items)
            {

                RenderableNode rNode = currentMaterial.OwningEntity.ParentNode;
                rNode.Update();

                Game.Device.SetTransform(TransformState.World, rNode.CurrentAbsoluteWorldMatrix);
                currentMaterial.Apply();

                IRenderable entity = rNode.RenderableObject;
                entity.DrawMesh();
            }


            Game.Device.SetTransform(TransformState.World, Matrix.Identity);
            effect.EndPass();
            effect.End();
        }

        public void Render()
        {
            if (isSameMaterial)
                RenderWithMaterial(Items[0]);

        }

        public void AddRange(AbstractMaterial[] materials)
        {
            foreach (AbstractMaterial material in materials)
                Add(material);
        }

        public void SortBy(Comparison<AbstractMaterial> comparison)
        {
            List<AbstractMaterial> nodeList = (List<AbstractMaterial>)Items;
            nodeList.Sort(comparison);
        }

        public MaterialCollection[] SplitByTechnique()
        {
            List<MaterialCollection> materialCollectionList = new List<MaterialCollection>();

            string currentTechnique = Items[0].EffectDescriptor.Technique;
            MaterialCollection currentCollection = new MaterialCollection();
            currentCollection.IsSameMaterial = true;

            foreach (AbstractMaterial material in Items)
            {
                if (material.EffectDescriptor.Technique != currentTechnique)
                {
                    materialCollectionList.Add(currentCollection);

                    currentCollection = new MaterialCollection();
                    currentTechnique = material.EffectDescriptor.Technique;
                }
                currentCollection.Add(material);
            }

            materialCollectionList.Add(currentCollection);

            return materialCollectionList.ToArray();
        }

        public AbstractMaterial[] ToArray()
        {
            return Items.ToArray();
        }

        public static int CompareNodesByTechnique(AbstractMaterial material1, AbstractMaterial material2)
        {
            if (material1.LightingTechnique > material2.LightingTechnique)
                return 1;
            else if (material1.LightingTechnique < material2.LightingTechnique)
                return -1;
            else
                return 0;

        }

        #region IEnumerable<AbstractMaterial> Members

        IEnumerator<AbstractMaterial> IEnumerable<AbstractMaterial>.GetEnumerator()
        {
            return Items.GetEnumerator();

        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        #endregion
    }
}
