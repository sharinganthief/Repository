
#region Imports



#endregion

namespace Helpers 
{
	public static class Serialization
	{
		public enum SerializationMode
		{
			String,
			XML
		}

        //public static string Serialize<T>( T objectToSerialize )
        //{
        //    return Serialize( objectToSerialize, SerializationMode.String );
        //}
        //public static string Serialize<T>( T objectToSerialize, SerializationMode mode )
        //{
        //    switch( mode )
        //    {
        //        case SerializationMode.XML:
        //            return XmlSerializer.SerializeToString( objectToSerialize );

        //        default:
        //            return new TypeSerializer<T>().SerializeToString( objectToSerialize );
        //    }
        //}
 
        //public static T Deserialize<T>( string serializedObject )
        //{       
        //    return Deserialize<T>( serializedObject, SerializationMode.String );
        //}
        //public static T Deserialize<T>( string serializedObject, SerializationMode mode )
        //{       
        //    switch( mode )
        //    {
        //        case SerializationMode.XML:
        //            return XmlSerializer.DeserializeFromString<T>( serializedObject );

        //        default:
        //            return new TypeSerializer<T>().DeserializeFromString( serializedObject );
        //    }
        //}
	}
}
