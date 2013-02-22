
#region Imports

using System;

#endregion

namespace Helpers
{
    public static class Enums
    {
		public static T ToEnum<T>( this string value ) 
		{
			try {
				return (T)Enum.Parse(typeof(T), value);

			} catch( Exception ex ) {
				throw new ApplicationException( string.Format( "Unable to convert value to enumeration [ Type = {0}, Value = {1} ]",
					typeof( T ), value ), ex );
			}
		}

		public static T ToEnum<T>( this string value, T defaultValue ) 
		{
			try {
				return (T)Enum.Parse(typeof(T), value);

			} catch {
				return defaultValue;
			}
		}
    }
}
