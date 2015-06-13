using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_SByte_Serializer : Serializer {
		
		public System_SByte_Serializer()
		{
			SerializerOf = typeof(sbyte);
			ManualChange = true;
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
		public int WriteSystem_SByte( sbyte value )
		{
			buffer[pos++] = (byte)value ;
			return 1 ;
		}
		public sbyte ReadSystem_SByte()
		{
			return (sbyte)buffer[pos++];
		}
	}
}

