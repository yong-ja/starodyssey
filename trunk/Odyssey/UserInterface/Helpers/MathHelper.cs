#region Disclaimer

/* 
 * MathHelper
 *
 * Created on 29 agosto 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
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

namespace AvengersUtd.Odyssey.UserInterface.Helpers
{

    public static class MathHelper
    {
        public static int Clamp(int value, int minimum, int maximum)
        {
            if (value < minimum)
                return minimum;
            else if (value > maximum)
                return maximum;
            else
                return value;
        }

    }
}