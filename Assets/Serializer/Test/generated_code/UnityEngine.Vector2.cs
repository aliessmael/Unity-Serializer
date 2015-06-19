/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class UnityEngine_Vector2_Serializer : Serializer
	{
		static public UnityEngine.Vector2 defaultValue = new UnityEngine.Vector2();
		public UnityEngine_Vector2_Serializer()
		{
			SerializerOf = typeof(UnityEngine.Vector2);
			
			AddField(typeof(System.Single), "x", 1);
			AddField(typeof(System.Single), "y", 2);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteUnityEngine_Vector2( (UnityEngine.Vector2)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadUnityEngine_Vector2();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteUnityEngine_Vector2( UnityEngine.Vector2 value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == UnityEngine_Vector2_Serializer.defaultValue)
				return 0;
			
			int dataSize = 0;
			
			if( slot != -1 )
			{
				dataSize += WriteSlotInfo( slot );
			}
			dataSize += WriteSystem_Single(value.x, true, 1);
			dataSize += WriteSystem_Single(value.y, true, 2);
			dataSize += WriteSystem_UInt32(0u);
			return dataSize;
		}
		
		public UnityEngine.Vector2 ReadUnityEngine_Vector2( uint subType = 0 )
		{
			if( subType == 0 )
			{
				UnityEngine.Vector2 value = new UnityEngine.Vector2();
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
						value.x = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 2)
					{
						value.y = ReadSystem_Single( );
						continue;
					}
				}
				return value;
			}
			throw new System.Exception("Can not ReadUnityEngine_Vector2");
		}
		
	}
}
