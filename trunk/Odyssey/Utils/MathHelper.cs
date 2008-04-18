#region Disclaimer

/* 
 * MathHelper
 *
 * Created on 06 settembre 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey Utils Library
 *
 * This source code is Intellectual Property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

#region Using Directives

using System;

#endregion

namespace AvengersUtd.Utils.Utils
{
    public class MathHelper
    {
        #region Private fields
        
        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        public static double PolynomialFit(double value, double[] coefficients)
        {
            int count = coefficients.Length;
            double result = coefficients[count - 1];
            for (int i=count-2; i>-1; i--)
            {
                result = (result * value) + coefficients[i];
            }
            return result;

        }

        public static int[] FindInterpolationInterval(double data, double[] table)
        {
            /* logarithmic interpolation of table data */

            int i0, i2;
            /* check if table is crescent or decrescent */
            for (i0 = 0; i0 < table.Length; i0++)
                if (table[i0] != 0)
                    break;
            for (i2 = i0 + 1; i2 < table.Length; i2++)
                if (table[i2] != 0)
                    break;
            int sign = Math.Sign(table[i2] - table[i0]);

            /* (comments from now on suppose sign > 0) */
            /* find first table data above orb data */
            for (i2 = 0; i2 < table.Length; i2++)
                if (table[i2] != 0 && (data - table[i2]) * sign <= 0)
                    break;

            /* pinpoint interpolation interval */
            if (data == table[i2])
                i0 = i2;
            else
            {
                for (i0 = i2 - 1; i0 >= 0; i0--)
                    if (table[i0] != 0)
                        break;
            }

            double i1;
            /* generate interpolation */
            if (i0 == i2)
                i1 = i2;
            else
                i1 = i0 + (i2 - i0) / (Math.Log(table[i2]) - Math.Log(table[i0])) *
                          (Math.Log(data) - Math.Log(table[i0]));

            int[] rval = new int[3];
            rval[0] = i0;
            rval[1] = (int)Math.Round(i1);
            rval[2] = i2;
            return rval;
        }


        private static double CubicKernel2(double inp)
        {
            inp = Math.Abs(inp);
            if (inp < 1.0)
            {
                return ((1.0 - (3.0 * Math.Pow(inp, 2.0))) + (2.0 * Math.Pow(inp, 3.0)));
            }
            return 0.0;
        }

         private static double Evaluate(double x, double[] inp)
        {
            double num2 = 0.0;
            for (int i = 0; i < inp.Length; i++)
            {
                num2 += inp[i] * CubicKernel2(x - i);
            }
            return num2;
        }

        public static double CubicInterpolation(double[] table, int[] iValues)
        {

            int i0 = iValues[0];
            int i1 =iValues[1];
            int i2 = iValues[2];

            if (table[i1] != 0)
                return table[i1];

            int iMidPoints = i2 - i0 - 1;
            double[] inp = new double[] { table[i0], table[i2] };
            int num = ((inp.Length - 1) * (iMidPoints + 1)) + 1;
            double[] numArray = new double[num];
            double x = 0.0;
            for (int i = 0; i < num; i++)
            {
                x = ((double)i) / ((double)(iMidPoints + 1));
                numArray[i] = Evaluate(x,inp);
            }
            return numArray[i2-i1];
        }

 
        public static double Interpolate(double[] table, int[] iValues)
        {
            int i0 = iValues[0];
            int i1 = iValues[1];
            int i2 = iValues[2];

            if (i0 == i2) // exact value
                return table[i1];
            else
                return Math.Exp((Math.Log(table[i0])*(i2 - i1) +
                                 Math.Log(table[i2])*(i1 - i0))/(i2 - i0));
        }

        public static double Lerp(double k, double a, double b)
        {
            return a + k * (b - a);
        }
    }
}