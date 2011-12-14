using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AvengersUtd.Odyssey.Log
{
    public struct TraceData
    {
        public Type Type { get; private set; }
        public MethodBase Method { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }
       
        public TraceData(Type type, MethodBase method, params object[] values) : this()
        {
            Type = type;
            Method = method;
            Parameters = method.GetParameters();
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
    }
}
