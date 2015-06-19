using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_SByte_Serializer : Serializer {
		
		public System_SByte_Serializer()
		{
			SerializerOf = typeof(sbyte);
			ManualChange = true;
			CanHasSubtype = false;
		}

		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_SByte();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			sbyte s = (sbyte)obj;
			stream.WriteSystem_SByte( s );
			return 1 ;
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_SByte( sbyte value, bool dontWriteDefault = false , int slot = -1)
		{
			if( dontWriteDefault && value == 0 )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			buffer[pos++] = (byte)value ;
			return dataSize + 1 ;
		}
		public sbyte ReadSystem_SByte( )
		{
			return (sbyte)buffer[pos++];
		}
	}
}

