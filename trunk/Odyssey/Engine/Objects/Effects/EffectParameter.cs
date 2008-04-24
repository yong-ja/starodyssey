using System;
using SlimDX.Direct3D9;
using SlimDX;

namespace AvengersUtd.Odyssey.Objects.Effects
{
    public delegate void Update(EffectParameter ep);

    public delegate Vector4 VectorOp();

    public class EffectParameter
    {
        string name;
        EffectHandle effectHandle;
        Effect ownerEffect;
        Update update;

        public string Name
        {
            get { return name; }
        }


        public EffectParameter(string name, Effect effect, Update updateMethod)
        {
            this.name = name;

            effectHandle = effect.GetParameter(null, name);
            ownerEffect = effect;
            update = updateMethod;
        }

        public void Apply()
        {
            update(this);
        }

        public static EffectParameter DefaultParameter(FXParameterType type, Effect effect)
        {
            string varName = string.Empty;
            Update update = null;
            EffectHandle eH;

            switch (type)
            {
                case FXParameterType.WorldViewProjection:
                    varName = ParamHandles.Matrices.WorldViewProjection;
                    eH = effect.GetParameter(null, varName);

                    update = delegate(EffectParameter fxParam)
                                 {
                                     fxParam.ownerEffect.SetValue(eH, Game.Device.
                                         Game.Device.Transform.World*
                                                                      Game.CurrentScene.Camera.View*
                                                                      Game.CurrentScene.Camera.Projection);
                                 };
                    break;

                case FXParameterType.World:
                    varName = ParamHandles.Matrices.World;
                    eH = effect.GetParameter(null, varName);

                    update =
                        delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, Game.CurrentScene.Device.Transform.World); };
                    break;

                case FXParameterType.View:
                    varName = ParamHandles.Matrices.View;
                    eH = effect.GetParameter(null, varName);

                    update =
                        delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, Game.CurrentScene.Camera.View); };
                    break;

                case FXParameterType.Projection:
                    varName = ParamHandles.Matrices.Projection;
                    eH = effect.GetParameter(null, varName);

                    update =
                        delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, Game.CurrentScene.Camera.Projection); };
                    break;

                case FXParameterType.LightDirection:
                    varName = ParamHandles.Vectors.LightDirection;
                    eH = effect.GetParameter(null, varName);

                    update = delegate(EffectParameter fxParam)
                                 {
                                     fxParam.ownerEffect.SetValue(eH,
                                                                  Game.CurrentScene.LightManager.GetParameter(0,
                                                                                                              FXParameterType
                                                                                                                  .
                                                                                                                  LightDirection));
                                 };
                    break;

                case FXParameterType.EyePosition:
                    varName = ParamHandles.Vectors.EyePosition;
                    update = delegate(EffectParameter fxParam)
                                 {
                                     fxParam.ownerEffect.SetValue(fxParam.effectHandle,
                                                                  Game.CurrentScene.Camera.PositionV4);
                                 };
                    break;

                case FXParameterType.AmbientColor:
                    varName = ParamHandles.Colors.Ambient;
                    eH = effect.GetParameter(null, varName);

                    update = delegate(EffectParameter fxParam)
                                 {
                                     fxParam.ownerEffect.SetValue(eH,
                                                                  Game.CurrentScene.LightManager.GetParameter(0,
                                                                                                              FXParameterType
                                                                                                                  .
                                                                                                                  AmbientColor));
                                 };
                    break;
            }

            return new EffectParameter(varName,
                                       effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, float value)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, value); };
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, Texture value)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, value); };
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, Vector4 value)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, value); };
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, VectorOp vectorOp)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, vectorOp()); };
            return new EffectParameter(varName, effect, update);
        }
    }
}