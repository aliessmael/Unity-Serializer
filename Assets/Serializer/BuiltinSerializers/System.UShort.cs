using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_UShort_Serializer : Serializer {

		public System_UShort_Serializer()
		{
			SerializerOf = typeof(ushort);
			ManualChange = true;
			CanHasSubtype = false;
		}

		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_UInt16();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			ushort u = (ushort)obj;
			return stream.WriteSystem_UInt16(  u );
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_UInt16( ushort value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == 0 )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			dataSize += WriteSystem_UInt32(value);
			return dataSize;
		}
		public ushort ReadSystem_UInt16()
		{
			return (ushort)ReadSystem_UInt32();
		}
	}
}

