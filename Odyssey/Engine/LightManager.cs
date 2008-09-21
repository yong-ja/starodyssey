using System.Collections.Generic;
using System.Drawing;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Objects.Entities;


namespace AvengersUtd.Odyssey.Engine
{
    /// <summary>
    /// Descrizione di riepilogo per LightManager.
    /// </summary>
    public class LightManager : IEnumerable<BaseLight>
    {
        Device device;

        List<BaseLight> lights;

        public LightManager()
        {
            device = Game.Device;
            lights = new List<BaseLight>();
            Light light = new Light();
            
        }

        public int LightCount
        {
            get { return lights.Count; }
        }


        public void SetLight(int num, BaseLight light)
        {
            lights.Insert(num, light);
        }

        public BaseLight GetLight(int num)
        {
            return lights[num];
        }

        public Vector4 GetParameter(int num, FXParameterType type)
        {
            Vector4 vOut = new Vector4();

            switch (type)
            {
                case FXParameterType.LightDirection:
                    return new Vector4(((Spotlight) lights[num]).DirectionV3, 1);

                case FXParameterType.LightPosition:
                    return lights[num].PositionV4;
            }

            return vOut;
        }

        public TLight[] GetLights<TLight>()
            where TLight : BaseLight
        {
            List<TLight> list = new List<TLight>();
            foreach (BaseLight light in this)
            {
                TLight tLight = light as TLight;
                if (tLight != null)
                    list.Add(tLight);
            }

            return list.ToArray();
        }

        #region IEnumerable<BaseLight> Members

        IEnumerator<BaseLight> IEnumerable<BaseLight>.GetEnumerator()
        {
            foreach (BaseLight light in lights)
                yield return light;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<BaseLight>).GetEnumerator();
        }

        #endregion
    }
}