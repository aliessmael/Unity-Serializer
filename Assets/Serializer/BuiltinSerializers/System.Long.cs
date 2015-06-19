using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Long_Serializer : Serializer {

		public System_Long_Serializer()
		{
			SerializerOf = typeof(long);
			ManualChange = true;
			CanHasSubtype = false;
		}

		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_Int64();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			long l = (long)obj;
			return stream.WriteSystem_Int64(  l );
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_Int64(long value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == 0 )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			ulong v = (ulong)((value << 1) ^ (value >> 0x3f));
			dataSize += WriteSystem_UInt64(v);
			return dataSize ;
		}
		public long ReadSystem_Int64()
		{
			ulong value = ReadSystem_UInt64();
			long res = (long) value;
			return (-(res & 1L) ^ ((res >> 1) & 0x7fffffffffffffffL));
		}
	}
}