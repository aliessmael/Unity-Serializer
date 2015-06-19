/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class UnityEngine_Color_Serializer : Serializer
	{
		static public UnityEngine.Color defaultValue = new UnityEngine.Color();
		public UnityEngine_Color_Serializer()
		{
			SerializerOf = typeof(UnityEngine.Color);
			
			AddField(typeof(System.Single), "r", 1);
			AddField(typeof(System.Single), "g", 2);
			AddField(typeof(System.Single), "b", 3);
			AddField(typeof(System.Single), "a", 4);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteUnityEngine_Color( (UnityEngine.Color)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadUnityEngine_Color();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteUnityEngine_Color( UnityEngine.Color value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == UnityEngine_Color_Serializer.defaultValue)
				return 0;
			
			int dataSize = 0;
			
			if( slot != -1 )
			{
				dataSize += WriteSlotInfo( slot );
			}
			dataSize += WriteSystem_Single(value.r, true, 1);
			dataSize += WriteSystem_Single(value.g, true, 2);
			dataSize += WriteSystem_Single(value.b, true, 3);
			dataSize += WriteSystem_Single(value.a, true, 4);
			dataSize += WriteSystem_UInt32(0u);
			return dataSize;
		}
		
		public UnityEngine.Color ReadUnityEngine_Color( uint subType = 0 )
		{
			if( subType == 0 )
			{
				UnityEngine.Color value = new UnityEngine.Color();
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
						value.r = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 2)
					{
						value.g = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 3)
					{
						value.b = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 4)
					{
						value.a = ReadSystem_Single( );
						continue;
					}
				}
				return value;
			}
			throw new System.Exception("Can not ReadUnityEngine_Color");
		}
		
	}
}
