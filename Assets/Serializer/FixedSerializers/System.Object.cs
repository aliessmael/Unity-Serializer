using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Object_Serializer : Serializer {

		public System_Object_Serializer()
		{
			SerializerOf = typeof(System.Object);
			ManualChange = true;
		}

		public override object Read( SerializerStream stream, System.Type type )
		{
			return null;
		}
		public override int Write( SerializerStream stream, object obj )
		{
			return 0 ;
		}
	}


	public partial class SerializerStream
	{
		public int WriteSystem_Object( System.Object value)
		{
			return 0 ;
		}
		
		public System.Object ReadSystem_Object( )
		{
			return null;
		}
	}
}
