/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test10_TestClass_Serializer : Serializer
	{
		public Test10_TestClass_Serializer()
		{
			SerializerOf = typeof(Test10.TestClass);
			
			AddField(typeof(UnityEngine.Texture2D), "faceImage", 1);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest10_TestClass( (Test10.TestClass)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest10_TestClass();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest10_TestClass( Test10.TestClass value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == null )
				return 0;
			
			int dataSize = 0;
			
			if( slot != -1 )
			{
				dataSize += WriteSlotInfo( slot );
			}
			dataSize += WriteUnityEngine_Texture2D(value.faceImage, true, 1);
			dataSize += WriteSystem_UInt32(0u);
			return dataSize;
		}
		
		public Test10.TestClass ReadTest10_TestClass( uint subType = 0 )
		{
			if( subType == 0 )
			{
				Test10.TestClass value = new Test10.TestClass();
				int fieldId = 1;
				uint typeId = 1;
				while( true )
				{
					ReadSlotInfo(out fieldId, out typeId);
					if (fieldId == 0)
					{
						break;
					}
					if (fieldId == 1)
					{
						value.faceImage = ReadUnityEngine_Texture2D( );
						continue;
					}
				}
				return value;
			}
			throw new System.Exception("Can not ReadTest10_TestClass");
		}
		
	}
}
