using System;
using System.Collections.Generic;
using System.Text;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class DepthMaterial:AbstractMaterial
    {
        public DepthMaterial()
        {
            fxType = FXType.DepthMap;
        }

        public override void CreateIndividualParameters()
        {
            throw new NotImplementedException();
        }


    }
}
