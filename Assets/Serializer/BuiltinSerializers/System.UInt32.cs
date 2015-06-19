using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_UInt32_Serializer : Serializer {
		
		public System_UInt32_Serializer()
		{
			SerializerOf = typeof(uint);
			ManualChange = true;
			CanHasSubtype = false;
		}

		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_UInt32();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			uint u = (uint)obj;
			return stream.WriteSystem_UInt32( u );
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_UInt32(uint value, bool dontWriteDefault = false ,int slot=-1)
		{
			if( dontWriteDefault && value == 0 )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			//write variant 
			do
			{
				uint byteVal = value & 0x7f;
				value >>= 7;
				
				if (value != 0)
				{
					byteVal |= 0x80;
				}
				
				buffer[pos++] = (byte)byteVal;
				dataSize++;
				
			} while (value != 0);
			
			return dataSize;
		}
		public uint ReadSystem_UInt32()
		{
			//read variant
			uint result = 0;
			int shift = 0;
			uint byteValue;
			do
			{
				byteValue = buffer[pos++];
				uint tmp = byteValue & 0x7f;
				result |= tmp << shift;
				if (shift > 32)
				{
					throw new System.ArgumentOutOfRangeException("bytes", "Byte array is too large.");
				}
				shift += 7;
			}
			while( ( byteValue & 0x80 ) != 0 );
			
			return result;
		}
	}
}

