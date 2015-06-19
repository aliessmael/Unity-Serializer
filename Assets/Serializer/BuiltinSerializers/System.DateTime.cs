using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_DateTime_Serializer : Serializer {

		public static System.DateTime defaultValue = new System.DateTime();
		public System_DateTime_Serializer()
		{
			SerializerOf = typeof(System.DateTime);
			ManualChange = true;
			CanHasSubtype = false;
		}


		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_DateTime();
		}
		public override int Write( SerializerStream stream, object value )
		{
			System.DateTime d = (System.DateTime)value;
			return stream.WriteSystem_DateTime( d);
		}
		
	}
	

	public partial class SerializerStream
	{

		public int WriteSystem_DateTime(System.DateTime value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == System_DateTime_Serializer.defaultValue )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);


			dataSize += WriteSystem_UInt32( (uint)value.Year );
			dataSize += WriteSystem_UInt32( (uint)value.Month );
			dataSize += WriteSystem_UInt32( (uint)value.Day );
			dataSize += WriteSystem_UInt32( (uint)value.Hour );
			dataSize += WriteSystem_UInt32( (uint)value.Minute );
			dataSize += WriteSystem_UInt32( (uint)value.Second );
			dataSize += WriteSystem_UInt32( (uint)value.Millisecond );
			
			return dataSize ;
		}
		public System.DateTime ReadSystem_DateTime()
		{
			int year 	= (int)ReadSystem_UInt32();
			int month 	= (int)ReadSystem_UInt32();
			int day 	= (int)ReadSystem_UInt32();
			int hour 	= (int)ReadSystem_UInt32();
			int minutes = (int)ReadSystem_UInt32() ;
			int sec 	= (int)ReadSystem_UInt32() ;
			int milli 	= (int)ReadSystem_UInt32() ;
			System.DateTime d = new System.DateTime( year, month, day, hour, minutes, sec, milli );
			
			return d;
		}
	}
}

