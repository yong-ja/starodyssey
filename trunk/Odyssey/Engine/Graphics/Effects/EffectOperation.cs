using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Effects
{
    public static class EffectOperation
    {
        public static VectorOp CreateCustomVectorOperation(FXVectorOperationType type, IEntity entity)
        {
            VectorOp vectorOp;
            switch (type)
            {
                case FXVectorOperationType.PointLightDirection:
                    vectorOp = delegate()
                                   {
                                       Vector4 vLightPos =
                                           Game.CurrentScene.LightManager.GetParameter(0, FXParameterType.LightPosition);
                                       Vector4 vPos =
                                           new Vector4(entity.PositionV3.X, entity.PositionV3.Y, entity.PositionV3.Z, 1);
                                       Vector4 vLightDir = vLightPos - vPos;
                                       vLightDir.Normalize();
                                       return vLightDir;
                                   };
                    return vectorOp;

                case FXVectorOperationType.EntityPosition:
                    vectorOp = delegate()
                                   {
                                       Vector4 vCenter = entity.PositionV4;
                                       return vCenter;
                                   };

                    return vectorOp;

                default:
                    return null;
            }
        }
    }
}