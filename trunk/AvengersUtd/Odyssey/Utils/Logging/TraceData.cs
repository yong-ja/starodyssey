using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics.Contracts;

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public struct TraceData
    {
        public Type Type { get; private set; }
        public MethodBase Method { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }
        Dictionary<string, string> data;
               
        public TraceData(Type type, MethodBase method) : this()
        {
            Type = type;
            Method = method;
            Parameters = method.GetParameters();
            data = new Dictionary<string, string>();

        }

        public TraceData(Type type, MethodBase method, Dictionary<string, string> data)
            : this()
        {
            Contract.Requires(data != null);
            Type = type;
            Method = method;
            Parameters = method.GetParameters();
            this.data = data;
        }

        public string MethodSignature 
        {
            get
            {
                string arguments = string.Empty;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    arguments += string.Format("{0} {1}", Parameters[i].ParameterType.Name, Parameters[i].Name);
                    if (i < Parameters.Length-1)
                        arguments += ", ";
                }

                return string.Format("{0}.{1}({2})", Type.Name, Method.Name, arguments);
            }
        }

        public string GetValue(string key)
        {
            Contract.Ensures(data.ContainsKey(key));
            return data[key];
        }
    }
}
