#region Disclaimer

/* 
 * SpaceResourceManager
 *
 * Created on 31 agosto 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Multiversal Rule System Library
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

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvengersUtd.MultiversalRuleSystem.Space
{
    #region Using Directives

    using System;
    using AvengersUtd.Odyssey.Utils.Xml;

    #endregion

    public static class SpaceResourceManager
    {
        #region Private fields

        #endregion

        #region Properties

        public static List<string> LoadStarNames()
        {
            return new List<string>(Data.DeserializeCollection<string>(Paths.StarNamesPath));
        }

        #endregion

        #region Constructors

        #endregion
    }
}