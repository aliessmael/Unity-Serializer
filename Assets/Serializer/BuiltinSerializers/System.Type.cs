using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Type_Serializer : Serializer {

		public System_Type_Serializer()
		{
			SerializerOf = typeof(System.Type);
			ManualChange = true;
			CanHasSubtype = false;
		}

		public override object Read( SerializerStream stream, System.Type _type )
		{
			return stream.ReadSystem_Type();
			
		}
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteSystem_Type( (System.Type)value);
		}
		
	}

	public partial class SerializerStream
	{

		public int WriteSystem_Type(System.Type value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == null )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			string str = value.ToString();
			if( value.IsArray )
				str = typeof(System.Array).ToString();
			else if( value.IsGenericType )
			{
				str = value.GetGenericTypeDefinition().ToString();
				int index = str.IndexOf("[");
				str = str.Substring(0,index);
			}
			if( string.IsNullOrEmpty( str ) )
				return dataSize + WriteSystem_String( "" ) ;
			else
				return dataSize + WriteSystem_String(  str ) ;
		}

		public System.Type ReadSystem_Type()
		{
			string typeStr = ReadSystem_String();
			if( string.IsNullOrEmpty( typeStr ))
				return null ;
			
			var type = System.Type.GetType(typeStr);
			if (type != null) 
				return type;
			foreach (var a in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				type = a.GetType(typeStr);
				if (type != null)
					return type;
			}
			return null ;
		}
	}
}
