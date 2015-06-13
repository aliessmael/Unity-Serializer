using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_MonoType_Serializer : Serializer {

		public System_MonoType_Serializer()
		{
			System.Type t = typeof(int);//System.MonoType is not accessable, I access it by trick
			SerializerOf = t.GetType();
			ManualChange = true;
		}

		public override object Read( SerializerStream stream, System.Type _type )
		{
			string typeStr = stream.ReadSystem_String();
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
		public override int Write( SerializerStream stream, object obj )
		{
			System.Type type = (System.Type)obj;
			if( type == null )
			{
				return stream.WriteSystem_String( "" ) ;
			}
			string str = type.ToString();
			if( type.IsArray )
				str = typeof(System.Array).ToString();
			else if( type.IsGenericType )
			{
				str = type.GetGenericTypeDefinition().ToString();
				int index = str.IndexOf("[");
				str = str.Substring(0,index);
			}
			if( string.IsNullOrEmpty( str ) )
				return stream.WriteSystem_String( "" ) ;
			else
				return stream.WriteSystem_String(  str ) ;

		}
		
	}

	public partial class SerializerStream
	{
	}
}
