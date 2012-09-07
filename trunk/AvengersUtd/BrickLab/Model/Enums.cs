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

    public static class Converter
    {

        internal static string ConditionFromEnum(Condition condition)
        {
            switch (condition)
            {
                case Condition.NewOrUsed:
                    return "N/A";

                default:
                case Condition.New:
                    return "N";

                case Condition.Used:
                    return "U";
            }
        }

        internal static string ItemTypeFromEnum(ItemType itemType)
        {
            switch (itemType)
            {
                default:
                case ItemType.Part:
                    return "P";
                case ItemType.MiniFigure:
                    return "M";
            }
        }
    }

}
