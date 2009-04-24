#region Disclaimer

/* 
 * StaticReflection
 *
 * Created on 21 agosto 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in any 
 * commercial project without the prior express and 
 * written consent of the Author.
 *
 */

#endregion

using System;

namespace AvengersUtd.Odyssey.UserInterface.Helpers
{

    #region Using Directives

    #endregion

    public static class StaticReflection
    {
        public static Type FindUnderlyingType(Type genericCollectionType)
        {
            Type[] implementedInterfaces = genericCollectionType.GetInterfaces();

            foreach (Type interfaceType in implementedInterfaces)
            {
                if (interfaceType.IsGenericType)
                    return interfaceType.GetGenericArguments()[0];
            }

            // genericCollectionType is not a generic type.
            return null;
        }
    }
}