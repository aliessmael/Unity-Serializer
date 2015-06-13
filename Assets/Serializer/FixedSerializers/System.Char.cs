using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Char_Serializer : Serializer {
		
		public System_Char_Serializer()
		{
			SerializerOf = typeof(char);
			ManualChange = true;
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
		public int WriteSystem_Char( char value )
		{
			int size = WriteSystem_Int16((short)value);
			return size;
		}
		public  char ReadSystem_Char()
		{
			return (char)ReadSystem_Int16();
		}
	}
}


