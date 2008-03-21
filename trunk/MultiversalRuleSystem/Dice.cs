using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.MultiversalRuleSystem.Space;
using AvengersUtd.MultiversalRuleSystem.Space;

namespace AvengersUtd.MultiversalRuleSystem
{
    public static class Dice
    {
        static readonly EnhancedRandom rnd = new EnhancedRandom();

        internal static EnhancedRandom Rnd
        {
            get { return rnd; }
        }

        public static double Roll()
        {
            return rnd.NextDouble();
        }

        public static double Roll(double maximumValue)
        {
            return rnd.NextDouble()*maximumValue;
        }

        public static double Roll(double minimumValue, double maximumValue)
        {
            return rnd.NextDouble(minimumValue,maximumValue);
        }
        

        public static int RollxDy(int xDices, int yDiceFaces)
        {
            int result = 0;
            for (int i = 0; i < xDices; i++)
            {
                result += rnd.Next(1, yDiceFaces + 1);
            }
            return result;
        }

        public static int Roll1D(int diceFaces)
        {
            return rnd.Next(1, diceFaces + 1);
        }
    }
}