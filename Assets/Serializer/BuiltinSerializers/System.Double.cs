using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_Double_Serializer : Serializer {

		public System_Double_Serializer()
		{
			SerializerOf = typeof(double);
			ManualChange = true;
			CanHasSubtype = false;
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
		public int WriteSystem_Double( double value, bool dontWriteDefault = false , int slot=-1)
		{ 
			if( dontWriteDefault && value == 0 )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			unsafe 
			{
				long v = *(long*)(&value);
				fixed (byte* ptr = &buffer[pos])
				{
					*(long*)ptr = v;
				}
			}
			pos += 8 ;
			return dataSize + 8;
		}
		public double ReadSystem_Double()
		{
			double result = System.BitConverter.ToDouble(buffer,pos);
			pos+=8;
			return result;
		}
	}
}