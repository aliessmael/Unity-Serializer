using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_String_Serializer : Serializer {

		public System_String_Serializer()
		{
			SerializerOf = typeof(string);
			ManualChange = true;
			CanHasSubtype = false;
		}

		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadSystem_String();
		}
		public override int Write( SerializerStream stream, object obj )
		{
			string s = (string)obj;
			return stream.WriteSystem_String(  s );
		}
	}

	public partial class SerializerStream
	{
		public int WriteSystem_String(string value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == null )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);


			if (string.IsNullOrEmpty(value))
			{
				dataSize = WriteSystem_UInt32((uint)0);
				return dataSize;
			}
			else
			{
				dataSize = WriteSystem_UInt32((uint)value.Length);
				int byteCount = System.Text.Encoding.ASCII.GetBytes(value,0,value.Length,buffer,pos);
				dataSize += byteCount;
				pos += byteCount;
				
				return dataSize;
			}
			
		}
		public string ReadSystem_String()
		{
			int size = (int)ReadSystem_UInt32();
			if (size == 0)
				return "";
			
			string value = System.Text.Encoding.ASCII.GetString(buffer,pos,size);
			pos += size;
			return value;
		}
	}
}
