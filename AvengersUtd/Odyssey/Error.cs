using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AvengersUtd.Odyssey
{
    internal static class Error
    {
        internal static ArgumentException ArgumentInvalid(string argument, Type type, string method, string message = null, string objValue= null)
        {
            if (message == null)
                message = Properties.Resources.ERR_NoDetails;
            if (message != null && objValue != null)
                message = string.Format(message, objValue);

            return new ArgumentException(string.Format(Properties.Resources.ERR_Argument, argument, type.Name, method),
                argument, new Exception(message));
        }

        internal static ArgumentNullException ArgumentNull(string argument, Type type, string method, string message = null)
        {
            if (message == null)
                message = Properties.Resources.ERR_NoDetails;

            return new ArgumentNullException(string.Format(Properties.Resources.ERR_ArgumentNull, argument, type.Name, method),
                new ArgumentException(message, argument));
        }
        internal static ArgumentNullException InCreatingFromObject(string paramName, Type instance, Type argument)
        {
            return new ArgumentNullException(paramName,
                    string.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.ERR_CreatingFromObject,
                        instance.Name, argument.Name));
        }

        internal static KeyNotFoundException KeyNotFound(string key, string collection, string message=null)
        {
            if (message == null)
                message = Properties.Resources.ERR_KeyNotFound;

            return new KeyNotFoundException(string.Format(message, collection, key));
        }

        internal static ArgumentOutOfRangeException IndexNotPresentInArray(string arrayName, int element)
        {
            return new ArgumentOutOfRangeException(arrayName, element,
                string.Format(Properties.Resources.ERR_ArrayElements, arrayName, element));
        }

        internal static ArgumentOutOfRangeException WrongCase(string param, string method, object value)
        {
            return new ArgumentOutOfRangeException(param, value, string.Format(Properties.Resources.ERR_WrongCase, param, method));
        }

        internal static InvalidOperationException InvalidOperation(string message)
        {
            return new InvalidOperationException(message);
        }

        internal static void MessageMissingFile(string filename, string message=null)
        {
            if (message == null)
                message = string.Format(Properties.Resources.ERR_MissingFile, filename);
            MessageBox.Show(message, Properties.Resources.ERR_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
