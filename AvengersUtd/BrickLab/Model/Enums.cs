using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.BrickLab.Model
{
    public enum Condition
    {
        NewOrUsed,
        Used,
        New
    }

    public enum ItemType
    {
        Unknown,
        Part,
        MiniFigure,
    }

    public enum Currency
    {
        Unknown,
        USD,
        EUR,
        GBP,
        AUD
    }
}
