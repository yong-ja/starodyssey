using System;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Effects;
using SlimDX;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Objects.Entities
{


    public class Spotlight : BaseLight
    {
        Matrix mViewProjection;
        Vector3 direction;
        Vector3 target;
        float innerConeAngle;
        float outerConeAngle;
        float falloff;
        public const string TechniqueTag = "Spot";


        #region Properties
        public Vector3 TargetV3
        {
            get { return target; }
        }

        public Vector4 TargetV4
        {
            get { return new Vector4(target, 1); }
        }

        public Vector3 DirectionV3
        {
            get { return direction; }
            set { direction = value; }
        }

        public Vector4 DirectionV4
        {
            get { return new Vector4(direction, 0); }
        }



        public float InnerConeAngle
        {
            get { return innerConeAngle; }
            set { innerConeAngle = value; }
        }

        /// <summary>
        /// Returns cosine of inner cone angle / 2.
        /// </summary>
        public float CosTheta
        {
            get
            {
                float theta = MathHelper.DegreeToRadian(innerConeAngle / 2);
                return (float) Math.Cos(theta);
            }
        }

        public float OuterConeAngle
        {
            get { return outerConeAngle; }
            set { outerConeAngle = value; }
        }

        /// <summary>
        /// Returns cosine of outer cone angle / 2.
        /// </summary>
        public float CosPhi
        {
            get
            {
                float phi = MathHelper.DegreeToRadian(outerConeAngle/2);
                return (float)Math.Cos(phi);
            }
        }

        public float Falloff
        {
            get { return falloff; }
        }

        public Vector3 Direction
        {
            get { return direction; }
        } 
        #endregion

        public Spotlight(Vector3 position, Vector3 target) : base(position)
        {
            this.target = target;
            this.direction = Vector3.Normalize(position - target);
            falloff = 1;
            Radius = 100;
            innerConeAngle = 60;
            outerConeAngle = 90;

            mViewProjection = CastsShadows ? CreateLightViewProjectionMatrix(this) : mViewProjection;
        }

        public override EffectParameter CreateEffectParameter(LightParameter parameter, Effect effect)
        {
            string varName=string.Empty;
            EffectHandle eh;
            Update update = null;
            try
            {
                switch (parameter)
                {
                    case LightParameter.SpotlightTarget:
                        varName = ParamHandles.Vectors.SpotlightTarget;
                        eh = new EffectHandle(varName);
                        update = (fxParam => fxParam.OwnerEffect.SetValue(eh, TargetV4));
                        break;

                    case LightParameter.SpotlightDirection:
                        varName = ParamHandles.Vectors.SpotlightDirection;
                        eh = new EffectHandle(varName);
                        update = (fxParam => fxParam.OwnerEffect.SetValue(eh, DirectionV4));
                        break;

                    case LightParameter.SpotlightInnerConeCosine:
                        varName = ParamHandles.Floats.SpotlightInnerConeCosine;
                        eh = new EffectHandle(varName);
                        update = (fxParam => fxParam.OwnerEffect.SetValue(eh, CosTheta));
                        break;

                    case LightParameter.SpotlightOuterConeCosine:
                        varName = ParamHandles.Floats.SpotlightOuterConeCosine;
                        eh = new EffectHandle(varName);
                        update = (fxParam => fxParam.OwnerEffect.SetValue(eh, CosPhi));
                        break;

                    case LightParameter.SpotlightFalloff:
                        varName = ParamHandles.Floats.SpotlightFalloff;
                        eh = new EffectHandle(varName);
                        update = (fxParam => fxParam.OwnerEffect.SetValue(eh, falloff));
                        break;

                    case LightParameter.LightWorldViewProjection:
                        varName = ParamHandles.Matrices.LightWorldViewProjection;
                        eh = new EffectHandle(varName);
                        update = delegate(EffectParameter fxParam)
                                     {
                                         mViewProjection = HasMoved ? CreateLightViewProjectionMatrix(this) : mViewProjection;
                                         Matrix mWorld = Game.CurrentScene.Camera.World;

                                         fxParam.OwnerEffect.SetValue(eh, mWorld * mViewProjection);
                                     };
                        break;

                    default:
                        return base.CreateEffectParameter(parameter, effect);
                }
            }
            catch (Direct3D9Exception)
            {
                MessageBox.Show(varName + Properties.Resources.ERR_FXVarNotFound,
                                                     Properties.Resources.ERR_Fatal,
                                                     MessageBoxButtons.OK);
                Application.Exit();
            }

            return new EffectParameter(varName, effect, update);
        }
    }
}