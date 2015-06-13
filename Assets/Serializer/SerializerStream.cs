using System;
using System.Collections;


namespace cloudsoft
{
	public partial class SerializerStream {

		byte[] buffer ;
		int    pos ;
		public SerializerStream( int capacity )
		{
			buffer = new byte[ capacity ];
		}
		public void Clear()
		{
			pos = 0;
		}
		public void Set( byte[] data )
		{
			Buffer.BlockCopy(data, 0, buffer, 0, data.Length);
		}
		public byte[] ToArray()
		{
			byte[] result = new byte[pos];
			Buffer.BlockCopy(buffer, 0, result, 0, pos);
			return result;
		}


        
	}
}
