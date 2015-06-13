using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Boolean_Serializer : Serializer {

		public System_Boolean_Serializer()
		{
			SerializerOf = typeof(bool);
			ManualChange = true;
		}


		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_Boolean();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			bool b = (bool)obj;
			return stream.WriteSystem_Boolean( b );
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_Boolean(   bool value)
		{
			int size = WriteSystem_UInt32(value ? (uint)1 : (uint)0);
			return size;
		}
		
		public  bool ReadSystem_Boolean( )
		{
			switch (ReadSystem_UInt32())
			{
			case 0:
				return false;
				
			case 1:
				return true;
			}
			throw new System.Exception("Unexpected boolean value");
		}
	}
}


