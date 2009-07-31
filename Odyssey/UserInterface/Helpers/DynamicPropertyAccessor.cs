using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace AvengersUtd.Odyssey.UserInterface.Helpers
{
    //
    // Author: James Nies
    // Description: The PropertyAccessor class provides fast dynamic access
    //		to a property of a specified target class.
    //
    // *** This code was written by James Nies and has been provided to you, ***
    // *** free of charge, for your use.  I assume no responsibility for any ***
    // *** undesired events resulting from the use of this code or the		 ***
    // *** information that has been provided with it .						 ***
    //
    // Formatted and mofidied to suit the UI's needs (Adalberto Simeone)

    /// <summary>
    /// The GenericPropertyAccessor class provides fast dynamic access
    /// to a property of a specified target class.
    /// </summary>
    public class DynamicPropertyAccessor : IPropertyAccessor
    {
        static Hashtable mTypeHash;
        bool canRead;
        bool canWrite;
        IPropertyAccessor emittedPropertyAccessor;
        string propertyName;
        Type propertyType;
        Type targetType;

        /// <summary>
        /// Thanks to Ben Ratzlaff for this snippet of code
        /// http://www.codeproject.com/cs/miscctrl/CustomPropGrid.asp
        /// 
        /// "Initialize a private hashtable with type-opCode pairs 
        /// so i dont have to write a long if/else statement when outputting msil"
        /// </summary>
        static DynamicPropertyAccessor()
        {
            mTypeHash = new Hashtable();
            mTypeHash[typeof (sbyte)] = OpCodes.Ldind_I1;
            mTypeHash[typeof (byte)] = OpCodes.Ldind_U1;
            mTypeHash[typeof (char)] = OpCodes.Ldind_U2;
            mTypeHash[typeof (short)] = OpCodes.Ldind_I2;
            mTypeHash[typeof (ushort)] = OpCodes.Ldind_U2;
            mTypeHash[typeof (int)] = OpCodes.Ldind_I4;
            mTypeHash[typeof (uint)] = OpCodes.Ldind_U4;
            mTypeHash[typeof (long)] = OpCodes.Ldind_I8;
            mTypeHash[typeof (ulong)] = OpCodes.Ldind_I8;
            mTypeHash[typeof (bool)] = OpCodes.Ldind_I1;
            mTypeHash[typeof (double)] = OpCodes.Ldind_R8;
            mTypeHash[typeof (float)] = OpCodes.Ldind_R4;
        }

        /// <summary>
        /// Creates a new property accessor.
        /// </summary>
        /// <param name="property">Property name.</param>
        public DynamicPropertyAccessor(Type targetType, string property)
        {
            this.targetType = targetType;
            propertyName = property;

            PropertyInfo propertyInfo = targetType.GetProperty(property);

            //
            // Make sure the property exists
            //
            if (propertyInfo == null)
            {
                throw new NullReferenceException(
                    string.Format("Property \"{0}\" does not exist for type "
                                  + "{1}.", property, targetType));
            }
            else
            {
                canRead = propertyInfo.CanRead;
                canWrite = propertyInfo.CanWrite;
                propertyType = propertyInfo.PropertyType;
            }
        }

        /// <summary>
        /// Whether or not the Property supports read access.
        /// </summary>
        public bool CanRead
        {
            get { return canRead; }
        }

        /// <summary>
        /// Whether or not the Property supports write access.
        /// </summary>
        public bool CanWrite
        {
            get { return canWrite; }
        }

        /// <summary>
        /// The Type of object this property accessor was
        /// created for.
        /// </summary>
        public Type TargetType
        {
            get { return targetType; }
        }

        /// <summary>
        /// The Type of the Property being accessed.
        /// </summary>
        public Type PropertyType
        {
            get { return propertyType; }
        }

        #region IPropertyAccessor Members

        /// <summary>
        /// Gets the property value from the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns>Property value.</returns>
        public object Get(object target)
        {
            if (canRead)
            {
                if (emittedPropertyAccessor == null)
                {
                    Init();
                }

                return emittedPropertyAccessor.Get(target);
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format("Property \"{0}\" does not have a get method.",
                                  propertyName));
            }
        }

        /// <summary>
        /// Sets the property for the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <param name="value">Value to set.</param>
        public void Set(object target, object value)
        {
            if (canWrite)
            {
                if (emittedPropertyAccessor == null)
                {
                    Init();
                }

                //
                // Set the property value
                //
                emittedPropertyAccessor.Set(target, value);
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format("Property \"{0}\" does not have a set method.",
                                  propertyName));
            }
        }

        #endregion

        /// <summary>
        /// This method creates a new assembly containing
        /// the Type that will provide dynamic access.
        /// </summary>
        void Init()
        {
            // Create the assembly and an instance of the 
            // property accessor class.
            Assembly assembly = EmitAssembly();

            emittedPropertyAccessor =
                assembly.CreateInstance("Property") as IPropertyAccessor;

            if (emittedPropertyAccessor == null)
            {
                throw new Exception("Unable to create property accessor.");
            }
        }

        /// <summary>
        /// Create an assembly that will provide the get and set methods.
        /// </summary>
        Assembly EmitAssembly()
        {
            //
            // Create an assembly name
            //
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = "GenericPropertyAccessorAssembly";

            //
            // Create a new assembly with one module
            //
            AssemblyBuilder newAssembly =
                Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder newModule = newAssembly.DefineDynamicModule("Module");

            //		
            //  Define a public class named "Property" in the assembly.
            //			
            TypeBuilder myType =
                newModule.DefineType("Property", TypeAttributes.Public | TypeAttributes.Sealed);

            //
            // Mark the class as implementing IPropertyAccessor. 
            //
            myType.AddInterfaceImplementation(typeof (IPropertyAccessor));

            // Add a constructor
            ConstructorBuilder constructor =
                myType.DefineDefaultConstructor(MethodAttributes.Public);


            //
            // Define a method for the get operation. 
            //
            Type[] getParamTypes = new Type[] {typeof (object)};
            Type getReturnType = typeof (object);
            MethodBuilder getMethod =
                myType.DefineMethod("Get",
                                    MethodAttributes.Public | MethodAttributes.Virtual,
                                    getReturnType,
                                    getParamTypes);


            //
            // From the method, get an ILGenerator. This is used to
            // emit the IL that we want.
            //
            ILGenerator getIL = getMethod.GetILGenerator();


            //
            // Emit the IL. 
            //
            MethodInfo targetGetMethod = targetType.GetMethod("get_" + propertyName);

            if (targetGetMethod != null)
            {
                getIL.DeclareLocal(typeof (object));
                getIL.Emit(OpCodes.Ldarg_1); //Load the first argument

                //Cast to the source type
                getIL.Emit(OpCodes.Castclass, targetType);
                getIL.EmitCall(OpCodes.Call, targetGetMethod, null);
                if (targetGetMethod.ReturnType.IsValueType)
                {
                    getIL.Emit(OpCodes.Box, targetGetMethod.ReturnType);
                    //Box if necessary
                }

                //(target object)
                getIL.Emit(OpCodes.Stloc_0); //Store it
                getIL.Emit(OpCodes.Ldloc_0);
            }
            else
            {
                getIL.ThrowException(typeof (MissingMethodException));
            }

            getIL.Emit(OpCodes.Ret);


            //
            // Define a method for the set operation.
            //

            Type[] setParamTypes = new Type[] {typeof (object), typeof (object)};
            Type setReturnType = null;
            MethodBuilder setMethod =
                myType.DefineMethod("Set",
                                    MethodAttributes.Public | MethodAttributes.Virtual,
                                    setReturnType,
                                    setParamTypes);

            //
            // From the method, get an ILGenerator. This is used to
            // emit the IL that we want.
            //
            ILGenerator setIL = setMethod.GetILGenerator();
            //
            // Emit the IL. 
            //

            MethodInfo targetSetMethod = targetType.GetMethod("set_" + propertyName);
            if (targetSetMethod != null)
            {
                Type paramType = targetSetMethod.GetParameters()[0].ParameterType;
                setIL.DeclareLocal(paramType);
                setIL.Emit(OpCodes.Ldarg_1); //Load the first argument 
                //(target object)
                //Cast to the source type
                setIL.Emit(OpCodes.Castclass, targetType);
                setIL.Emit(OpCodes.Ldarg_2); //Load the second argument 
                //(value object)
                if (paramType.IsValueType)
                {
                    setIL.Emit(OpCodes.Unbox, paramType); //Unbox it 
                    if (mTypeHash[paramType] != null) //and load
                    {
                        OpCode load = (OpCode) mTypeHash[paramType];
                        setIL.Emit(load);
                    }
                    else
                    {
                        setIL.Emit(OpCodes.Ldobj, paramType);
                    }
                }
                else
                {
                    setIL.Emit(OpCodes.Castclass, paramType); //Cast class
                }

                setIL.EmitCall(OpCodes.Callvirt,
                               targetSetMethod, null); //Set the property value
            }
            else
            {
                setIL.ThrowException(typeof (MissingMethodException));
            }

            setIL.Emit(OpCodes.Ret);

            //
            // Load the type
            //
            myType.CreateType();

            return newAssembly;
        }
    }
}