using System;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Objects.Entities
{
    public abstract class BaseLight : IEntity
    {
        static readonly Color4 DefaultAmbientColor = new Color4(1.0f, 0.05f, 0.05f, 0.05f);
        static readonly Color4 DefaultWhiteLightColor = new Color4(1f, 1f, 1f, 1f);
        Vector3 position;
        Vector3 rotationDelta;
        Color4 lightColor;
        Color4 specularColor;
        Color4 ambientColor;
        bool isActive;
        bool hasMoved;
        bool castsShadows;

        #region Properties

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public bool HasMoved
        {
            get { return hasMoved; }
        }

        public Vector3 Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    hasMoved = true;
                }
            }
        }

        public float Radius { get; set; }
        public Quaternion CurrentRotation { get; set; }

        public bool CastsShadows
        {
            get { return castsShadows; }
            set { castsShadows = value; }
        }

        public Color4 AmbientColor
        {
            get { return ambientColor; }
            set { ambientColor = value; }
        }



        #endregion

        protected BaseLight(Vector3 position)
        {
            this.position = position;
            Radius = 100;
            lightColor = specularColor = DefaultWhiteLightColor;
            ambientColor = DefaultAmbientColor;
            isActive = true;
            castsShadows = true;
            hasMoved = true;
        }

        public Vector3 PositionV3
        {
            get { return position; }
            set { position = value; }
        }

        public Vector4 PositionV4
        {
            get { return new Vector4(position, 1.0f); }
        }

        public Vector3 RotationDelta
        {
            get { return rotationDelta; }
            set { rotationDelta = value; }
        }

        public void UpdatePosition()
        {
            return;
        }

        public bool IsInViewFrustum()
        {
            return BoundingFrustum.Contains(Game.CurrentScene.Camera.Frustum, position) ==
                   ContainmentType.Contains;
        }

        public static Matrix CreateLightViewProjectionMatrix(Spotlight spotlight)
        {
            float nearClip = Game.CurrentScene.Camera.NearClip;
            float farClip = spotlight.Radius;

            Matrix mLightsView = Matrix.LookAtLH(
                spotlight.PositionV3,
                spotlight.TargetV3,
                new Vector3(0, 1, 0));

            Matrix mLightsProj = Matrix.PerspectiveFovLH(
                MathHelper.DegreeToRadian(90f),
                1f, nearClip, farClip);

            return mLightsView*mLightsProj;
        }

        public static Matrix CreateTextureBiasMatrix(int shadowMapSize, float biasValue)
        {
            float fTexOffs = 0.5f + (0.5f / (shadowMapSize));
            Matrix mTextureBias = new Matrix
                               {
                                   M11 = 0.5f,
                                   M22 = (-0.5f),
                                   M33 = 1.0f,
                                   M41 = fTexOffs,
                                   M42 = fTexOffs,
                                   M43 = (-biasValue),
                                   M44 = 1.0f
                               };
            return mTextureBias;
        }

        public virtual EffectParameter CreateEffectParameter(LightParameter parameter, Effect effect)
        {
            string varName;
            Update update = null;
            EffectHandle eh;
            switch (parameter)
            {
                case LightParameter.Position:
                    varName = ParamHandles.Vectors.LightPosition;
                    eh = new EffectHandle(varName);
                    update = (fxParam => fxParam.OwnerEffect.SetValue(eh, PositionV4));
                    break;

                case LightParameter.Radius:
                    varName = ParamHandles.Floats.LightRadius;
                    eh = new EffectHandle(varName);
                    update = (fxParam => fxParam.OwnerEffect.SetValue(eh, Radius));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(parameter.ToString(), Properties.Resources.ERR_UnrecognizedParameter);
            }

            return new EffectParameter(varName, effect, update);
        }
        
    }
}