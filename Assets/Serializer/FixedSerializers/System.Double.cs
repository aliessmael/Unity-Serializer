using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Double_Serializer : Serializer {

		public System_Double_Serializer()
		{
			SerializerOf = typeof(double);
			ManualChange = true;
		}
	

		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_Double();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			double d = (double)obj;
			return stream.WriteSystem_Double(d);
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_Double( double value )
		{ 
			unsafe 
			{
				long v = *(long*)(&value);
				fixed (byte* ptr = &buffer[pos])
				{
					*(long*)ptr = v;
				}
			}
			pos += 8 ;
			return 8;
		}
		public double ReadSystem_Double()
		{
			double result = System.BitConverter.ToDouble(buffer,pos);
			pos+=8;
			return result;
		}
	}
}