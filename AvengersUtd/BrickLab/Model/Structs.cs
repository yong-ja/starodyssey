using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.BrickLab.Model
{
    public class PriceInfo
    {
        public Condition Condition { get; private set; }
        public PriceInfoType PriceInfoType { get; private set; }

        public PriceInfo(Condition condition, PriceInfoType priceInfoType)
        {
            Condition = condition;
            PriceInfoType = priceInfoType;
        }
    }
}
