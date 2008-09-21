using System;
using System.Globalization;

namespace AvengersUtd.Odyssey.UserInterface.Helpers
{
    internal static class Error
    {
        internal static ArgumentNullException InCreatingFromObject(string paramName, Type instance, Type argument)
        {
            return
                new ArgumentNullException(paramName,
                                          string.Format(CultureInfo.CurrentCulture,
                                                        "You cannot create a {0} object from a null {1} type.",
                                                        instance.Name, argument.Name));
        }
    }
}