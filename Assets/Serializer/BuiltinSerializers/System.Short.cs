using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Short_Serializer : Serializer {

		public System_Short_Serializer()
		{
			SerializerOf = typeof(short);
			ManualChange = true;
			CanHasSubtype = false;
		}

		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_Int16();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			short s = (short)obj;
			return stream.WriteSystem_Int16( s );
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_Int16( short value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == 0 )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			dataSize += WriteSystem_Int32(value);
			return dataSize ;
		}
		public short ReadSystem_Int16( )
		{
			return (short)ReadSystem_Int32();
		}
	}
}
