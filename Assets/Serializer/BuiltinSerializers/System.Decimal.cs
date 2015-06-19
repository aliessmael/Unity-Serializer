using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Decimal_Serializer : Serializer {

		public static decimal defaultValue = new decimal();
		public System_Decimal_Serializer()
		{
			SerializerOf = typeof(decimal);
			ManualChange = true;
			CanHasSubtype = false;
		}


		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_Decimal();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			decimal d = (decimal)obj;
			int size = stream.WriteSystem_Decimal( d );
			return size ;
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_Decimal( decimal value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == System_Decimal_Serializer.defaultValue )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			int[] parts = System.Decimal.GetBits(value);
			bool sign = (parts[3] & 0x80000000) != 0;
			byte scale = (byte) ((parts[3] >> 16) & 0x7F); 
			

			dataSize += WriteSystem_UInt32((uint)parts[0]);
			dataSize += WriteSystem_UInt32((uint)parts[1]);
			dataSize += WriteSystem_UInt32((uint)parts[2]);
			dataSize += this.WriteSystem_Boolean(sign);
			dataSize += WriteSystem_Byte(scale);
			return dataSize ;
		}
		public decimal ReadSystem_Decimal()
		{
			int p1 = (int)ReadSystem_UInt32();
			int p2 = (int)ReadSystem_UInt32();
			int p3 = (int)ReadSystem_UInt32();
			bool sign = this.ReadSystem_Boolean();
			byte scale = ReadSystem_Byte();
			return new decimal(p1, p2, p3, sign, scale);
		}
	}
}
