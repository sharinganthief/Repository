
#region Imports

using System;
using System.Reflection;

#endregion

namespace Helpers 
{
	public static class Reflection 
	{
        public static TReturnType GetValue<T, TReturnType>(T objectToReflect, string propertyName)
        {
            //Type type = typeof(T);
            //PropertyInfo[] properties = type.GetProperties();
            PropertyInfo pi = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance) ??
                              typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (pi == null)
            {
                FieldInfo fi = typeof(T).GetField(propertyName, BindingFlags.Public | BindingFlags.Instance) ??
                               typeof(T).GetField(propertyName, BindingFlags.Public | BindingFlags.Static);
                if (fi == null)
                    return default(TReturnType);
                try
                {
                    return (TReturnType)fi.GetValue(null);
                }
                catch (Exception)
                {
                    return (TReturnType)fi.GetRawConstantValue();
                }
            }

            return (TReturnType)pi.GetValue(objectToReflect, null);
        }
    
        public static TReturnType GetValue<TReturnType>( Type staticTypeToReflect, string propertyName)
        {
            //Type type = staticTypeToReflect;
            //PropertyInfo[] properties = type.GetProperties();
            PropertyInfo pi = staticTypeToReflect.GetProperty(propertyName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance) ??
                              staticTypeToReflect.GetProperty(propertyName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (pi == null)
            {
                FieldInfo fi = staticTypeToReflect.GetField(propertyName, BindingFlags.Public | BindingFlags.Instance) ??
                               staticTypeToReflect.GetField(propertyName, BindingFlags.Public | BindingFlags.Static);
                if (fi == null)
                    return default(TReturnType);
                try
                {
                    return (TReturnType)fi.GetValue(null);
                }
                catch (Exception)
                {
                    return (TReturnType)fi.GetRawConstantValue();
                }
            }

            return (TReturnType)pi.GetValue(staticTypeToReflect, null);
        }
    }
}
