using System.Drawing;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using SlimDX;
using AvengersUtd.Odyssey.Objects.Effects;


namespace AvengersUtd.Odyssey.Engine
{
    /// <summary>
    /// Descrizione di riepilogo per LightManager.
    /// </summary>
    public class LightManager
    {
        Device device;
        static Color4 cAmbient = new Color4(1f, 0.1f, 0.1f, 0.1f);

        public LightManager()
        {
            device = Game.Device;
        }

        //public void SetPointLight(int num, Color color, Vector3 position, Vector3 attenuation)
        //{
        //    device.Lights[num].Type = LightType.Point;
        //    device.Lights[num].Diffuse = color;
        //    device.Lights[num].Position = position;
        //    device.Lights[num].Attenuation0 = attenuation.X;
        //    device.Lights[num].Attenuation1 = attenuation.Y;
        //    device.Lights[num].Attenuation2 = attenuation.Z;
        //    device.Lights[num].Enabled = true;
        //}

        public Light GetLight(int num)
        {
            return device.GetLight(num);
        }

        public Vector4 GetParameter(int num, FXParameterType type)
        {
            Vector4 vOut = new Vector4();

            switch (type)
            {
                case FXParameterType.LightDirection:
                    //Vector3 vTmp = device.GetLight(num).Position;

                    //Vector3 vTmp = new Vector3(-1f, 5f, 5f);
                    Vector3 vTmp = new Vector3(1000, 1000, 1000);
                    Vector3 vLight = Vector3.Normalize(vTmp);
                    vOut = new Vector4(vLight.X, vLight.Y, vLight.Z, 1f);
                    break;

                case FXParameterType.LightPosition:
                    return new Vector4(30, 0, -0, 1);

                case FXParameterType.AmbientColor:
                    //Color cAmbient = device.Lights[num].Ambient;
                    //device.Lights[num].AmbientColor.Alpha;
                    vOut = new Vector4(cAmbient.Red,
                                       cAmbient.Green,
                                       cAmbient.Blue,
                                       cAmbient.Alpha
                        );
                    break;
            }

            return vOut;
        }
    }
}
  