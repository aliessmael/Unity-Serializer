using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Object_Serializer : Serializer {

		public System_Object_Serializer()
		{
			SerializerOf = typeof(System.Object);
			ManualChange = true;
			CanHasSubtype = false;
		}

		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_Object();
		}
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteSystem_Object( value );
		}
	}


	public partial class SerializerStream
	{
		public int WriteSystem_Object( System.Object value, bool dontWriteDefault = false , int slot = -1)
		{
			if( dontWriteDefault && value == null )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			return dataSize;
		}
		
		public System.Object ReadSystem_Object()
		{
			return null;
		}
	}
}
