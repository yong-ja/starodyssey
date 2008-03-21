using System;
using System.Text;

namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{

    #region Galaxy Generation Enums

    public enum GalaxyAge
    {
        Young,
        Mature,
        Ancient
    }

    public enum GalaxySize
    {
        Huge,
        Large,
        Normal,
        Small,
        Tiny
    }

    public enum GalaxyShape
    {
        Cluster,
        Spiral
    }

    public enum GalaxyPlanetDensity
    {
        Many,
        Average,
        Few
    }

    public enum MetalRichness
    {
        Plentyful,
        Abundant,
        Scarce
    }

    #endregion

    public enum Zone : int
    {
        Unknown = -1,
        Planetary = 0,
        GasGiant = 1,
        Frozen = 2
    }

    public enum EvolutionResult
    {
        MainSequence,
        SubGiant,
        Giant,
        WhiteDwarf,
        Neutron,
        BlackHole
    }

    public enum WorldType : int
    {
        Empty = -1,
        Planet = 0,
        IceWorld = 1,
        GasGiant = 2,
        AsteroidBelt = 3,
    }


    public static class GenerationCommon
    {
        static string[,] romanDigits = new string[,]
            {
                {"M", "C", "X", "I"},
                {"MM", "CC", "XX", "II"},
                {"MMM", "CCC", "XXX", "III"},
                {null, "CD", "XL", "IV"},
                {null, "D", "L", "V"},
                {null, "DC", "LX", "VI"},
                {null, "DCC", "LXX", "VII"},
                {null, "DCCC", "LXXX", "VIII"},
                {null, "CM", "XC", "IX"}
            };


        public static string ArabicToRoman(int value)
        {
            if (value > 3000)
                throw new ArgumentOutOfRangeException("value");


            StringBuilder result = new StringBuilder(15);

            for (int index = 0; index < 4; index++)
            {
                int power = (int) Math.Pow(10, 3 - index);
                int digit = value/power;
                value -= digit*power;
                if (digit > 0)
                    result.Append(romanDigits[digit - 1, index]);
            }

            return result.ToString();
        }
    }
}