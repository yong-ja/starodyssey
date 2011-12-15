﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey
{
    internal static class Error
    {

        internal static string IndexNotInRange(string varName,string arrayName, int value)
        {
            return string.Format(Resources.ERR_IndexNotInRange, varName, arrayName, value);
        }

        internal static string ArgumentInvalid(string argumentName, object value)
        {
            return string.Format(Resources.ERR_Argument, argumentName, value);
        }

        #region Arguments
        internal static ArgumentException ArgumentInvalid(string argument, Type type, string method, string message = null, string objValue = null)
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

        internal static InvalidEnumArgumentException WrongCase(string param, string method, object value)
        {
            return new InvalidEnumArgumentException(string.Format(Properties.Resources.ERR_WrongCase, param, method));
        }
        #endregion

        internal static KeyNotFoundException KeyNotFound(string key, string collection, string message=null)
        {
            if (message == null)
                message = Resources.ERR_KeyNotFound;

            return new KeyNotFoundException(string.Format(message, collection, key));
        }

        internal static NotSupportedException NotSupported(string message)
        {
            return new NotSupportedException(message);
        }

        internal static InvalidOperationException InvalidOperation(string message)
        {
            return new InvalidOperationException(message);
        }

        internal static void MessageMissingFile(string format, string filename, params object[] args)
        {
            string message = string.Format(format, filename, args);
            MessageBox.Show(message, Resources.ERR_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
