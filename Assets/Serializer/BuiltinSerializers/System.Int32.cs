using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Int32_Serializer : Serializer{
		
		public System_Int32_Serializer()
		{
			SerializerOf = typeof(int);
			ManualChange = true;
			CanHasSubtype = false;
		}


		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_Int32();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			int i = (int)obj;
			return stream.WriteSystem_Int32( i );
		}
	}
	public partial class SerializerStream
	{
		public int WriteSystem_Int32(int value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == 0 )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			uint v = (uint)((value << 1) ^ (value >> 0x1f));
			dataSize += WriteSystem_UInt32(v);
			return dataSize ;
		}
		public int ReadSystem_Int32( )
		{
			uint value = ReadSystem_UInt32();
			int res = (int) value;
			return (-(res & 1) ^ ((res >> 1) & 0x7fffffff));
			
		}
	}
}
