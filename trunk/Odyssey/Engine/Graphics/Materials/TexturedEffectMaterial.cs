using System;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey.Objects.Effects;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class TexturedEffectMaterial : TexturedMaterial,IEffectMaterial
    {
        
        public override void Apply()
        {
            //base.Apply();
            effectDescriptor.UpdateDynamic();
            effectDescriptor.Effect.CommitChanges();
        }

        // <summary>
        // dispose meshObject and materials
        // </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed components
                }

                // dispose unmanaged components
                effectDescriptor.Effect.Dispose();
            }
            disposed = true;
        }

        ~TexturedEffectMaterial()
        {
            Dispose(false);
        }
    }
}