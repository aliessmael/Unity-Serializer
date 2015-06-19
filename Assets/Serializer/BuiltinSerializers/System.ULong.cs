using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_ULong_Serializer : Serializer {

		public System_ULong_Serializer()
		{
			SerializerOf = typeof(ulong);
			ManualChange = true;
			CanHasSubtype = false;
		}

		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_UInt64();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			ulong u = (ulong)obj;
			return stream.WriteSystem_UInt64(  u );
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_UInt64(ulong value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == 0 )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			do
			{
				ulong byteVal = value & 0x7f;
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
		public ulong ReadSystem_UInt64()
		{
			ulong result = 0;
			int shift = 0;
			ulong byteValue;
			do
			{
				byteValue = buffer[pos++];
				ulong tmp = byteValue & 0x7f;
				result |= tmp << shift;
				if (shift > 64)
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

