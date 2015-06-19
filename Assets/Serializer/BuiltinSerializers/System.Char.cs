using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Char_Serializer : Serializer {

		public static char defaultValue = new char();
		public System_Char_Serializer()
		{
			SerializerOf = typeof(char);
			ManualChange = true;
			CanHasSubtype = false;
		}


		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_Char();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			char c = (char)obj;
			stream.WriteSystem_Char(c);
			return sizeof(char) ;
		}
	}
	public partial class SerializerStream
	{
		public int WriteSystem_Char( char value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == System_Char_Serializer.defaultValue )
				return 0;

			int dataSize = 0;
			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			dataSize += WriteSystem_Int16((short)value);
			return dataSize;
		}
		public  char ReadSystem_Char( )
		{
			return (char)ReadSystem_Int16();
		}
	}
}


