using UnityEngine;
using System.Collections;
using System.IO;
using cloudsoft ;

namespace cloudsoft
{
	public class UnityEngine_Texture2D_Serializer : Serializer {

		public UnityEngine_Texture2D_Serializer()
		{
			SerializerOf = typeof(Texture2D);
			ManualChange = true;
			CanHasSubtype = false;
		}


		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadUnityEngine_Texture2D()  ;
		}
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteUnityEngine_Texture2D( (Texture2D)value);
		}
	}

	public partial class SerializerStream
	{
		public int WriteUnityEngine_Texture2D(   UnityEngine.Texture2D value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == null )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			Texture2D t = (Texture2D)value;
			if( t == null ){
				return WriteSystem_UInt32((uint)0);
			}

			byte[] data = t.EncodeToPNG();
			dataSize += WriteBytes(data );
			
			return dataSize ;
		}
		public UnityEngine.Texture2D ReadUnityEngine_Texture2D()
		{
			Texture2D t = new Texture2D(1,1);
			byte[] data = ReadBytes();
			if( data != null ){
				t.LoadImage( data );
			}
			return t ;
		}
	}
}

