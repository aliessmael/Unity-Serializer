using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Single_Serializer : Serializer {

		public System_Single_Serializer()
		{
			SerializerOf = typeof(float);
			ManualChange = true;
		}


		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_Single();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			float f = (float)obj;
			return stream.WriteSystem_Single( f);
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_Single( float value )
		{
			unsafe
			{
				int v = *(int*)(&value);
				
				fixed (byte* ptr = &buffer[pos])
				{
					*(int*)ptr = v;
				}
			}
			pos += 4 ;
			return 4;
		}
		public float ReadSystem_Single( )
		{
			float result = System.BitConverter.ToSingle(buffer,pos);
			pos+=4;
			return result;
		}
	}
}
