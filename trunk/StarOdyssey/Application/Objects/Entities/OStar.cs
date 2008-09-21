using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.Odyssey.Objects.Entities;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.MultiversalRuleSystem.Space;

namespace AvengersUtd.StarOdyssey.Objects.Entities
{
    public class OStar : CelestialEntity, IBillboard
    {
        readonly float billboardSize;
        Star mrsStar;

        public Star MrsStar
        {
            get { return mrsStar; }
        }

        public OStar(EntityDescriptor entityDesc, Star star, CelestialEntity primary, float billboardSize) : 
            base(entityDesc, star.StellarFeatures.OrbitalParameters, primary)
        {
            this.billboardSize = billboardSize;
            mrsStar = star;
        }

        public static OStar ConvertFromStar(Star star, CelestialEntity primary)
        {
            StarType type = star.StellarFeatures.SpectralClass.Type;
            StarColor color = star.StellarFeatures.SpectralClass.StarColor;
            float size;

            switch (type)
            {
                case StarType.WhiteDwarf:
                case StarType.MainSequence:
                    size = 8;
                    break;

                case StarType.SubGiant:
                    size = 10;
                    break;

                case StarType.Giant:
                    size = 12;
                    break;

                case StarType.SuperGiant:
                    size = 14;
                    break;

                case StarType.HyperGiant:
                    size = 16;
                    break;

                default:
                    size = 1;
                    break;
            }

            EntityDescriptor eDesc = EntityManager.GetEntityDescriptor(color.ToString() + type.ToString());

            return new OStar(eDesc, star, primary, size);
        }

        #region IBillboard Members

        float IBillboard.BillboardSize
        {
            get { return billboardSize; }
        }

        #endregion
    }
}
