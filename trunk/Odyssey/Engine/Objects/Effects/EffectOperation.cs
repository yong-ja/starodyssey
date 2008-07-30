using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Meshes;
using SlimDX;

namespace AvengersUtd.Odyssey.Objects.Effects
{
    public static class EffectOperation
    {


        public static VectorOp CreateCustomOperation(FXOperationType type, IEntity entity)
        {
            switch (type)
            {
                case FXOperationType.PointLightDirection:
                    VectorOp vectorOp = delegate()
                    {
                        Vector4 vLightPos =
                            Game.CurrentScene.LightManager.GetParameter(0,FXParameterType.LightPosition);
                        Vector4 vPos =
                            new Vector4(entity.Position.X, entity.Position.Y, entity.Position.Z, 1);
                        Vector4 vLightDir = vLightPos - vPos;
                        vLightDir.Normalize();
                        return vLightDir;
                    };
                    return vectorOp;

                default:
                    return null;
            }
        }
    }
}
