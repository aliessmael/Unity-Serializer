/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class UnityEngine_Matrix4x4_Serializer : Serializer
	{
		static public UnityEngine.Matrix4x4 defaultValue = new UnityEngine.Matrix4x4();
		public UnityEngine_Matrix4x4_Serializer()
		{
			SerializerOf = typeof(UnityEngine.Matrix4x4);
			
			AddField(typeof(System.Single), "m00", 1);
			AddField(typeof(System.Single), "m10", 2);
			AddField(typeof(System.Single), "m20", 3);
			AddField(typeof(System.Single), "m30", 4);
			AddField(typeof(System.Single), "m01", 5);
			AddField(typeof(System.Single), "m11", 6);
			AddField(typeof(System.Single), "m21", 7);
			AddField(typeof(System.Single), "m31", 8);
			AddField(typeof(System.Single), "m02", 9);
			AddField(typeof(System.Single), "m12", 10);
			AddField(typeof(System.Single), "m22", 11);
			AddField(typeof(System.Single), "m32", 12);
			AddField(typeof(System.Single), "m03", 13);
			AddField(typeof(System.Single), "m13", 14);
			AddField(typeof(System.Single), "m23", 15);
			AddField(typeof(System.Single), "m33", 16);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteUnityEngine_Matrix4x4( (UnityEngine.Matrix4x4)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadUnityEngine_Matrix4x4();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteUnityEngine_Matrix4x4( UnityEngine.Matrix4x4 value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == UnityEngine_Matrix4x4_Serializer.defaultValue)
				return 0;
			
			int dataSize = 0;
			
			if( slot != -1 )
			{
				dataSize += WriteSlotInfo( slot );
			}
			dataSize += WriteSystem_Single(value.m00, true, 1);
			dataSize += WriteSystem_Single(value.m10, true, 2);
			dataSize += WriteSystem_Single(value.m20, true, 3);
			dataSize += WriteSystem_Single(value.m30, true, 4);
			dataSize += WriteSystem_Single(value.m01, true, 5);
			dataSize += WriteSystem_Single(value.m11, true, 6);
			dataSize += WriteSystem_Single(value.m21, true, 7);
			dataSize += WriteSystem_Single(value.m31, true, 8);
			dataSize += WriteSystem_Single(value.m02, true, 9);
			dataSize += WriteSystem_Single(value.m12, true, 10);
			dataSize += WriteSystem_Single(value.m22, true, 11);
			dataSize += WriteSystem_Single(value.m32, true, 12);
			dataSize += WriteSystem_Single(value.m03, true, 13);
			dataSize += WriteSystem_Single(value.m13, true, 14);
			dataSize += WriteSystem_Single(value.m23, true, 15);
			dataSize += WriteSystem_Single(value.m33, true, 16);
			dataSize += WriteSystem_UInt32(0u);
			return dataSize;
		}
		
		public UnityEngine.Matrix4x4 ReadUnityEngine_Matrix4x4( uint subType = 0 )
		{
			if( subType == 0 )
			{
				UnityEngine.Matrix4x4 value = new UnityEngine.Matrix4x4();
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
						value.m00 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 2)
					{
						value.m10 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 3)
					{
						value.m20 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 4)
					{
						value.m30 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 5)
					{
						value.m01 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 6)
					{
						value.m11 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 7)
					{
						value.m21 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 8)
					{
						value.m31 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 9)
					{
						value.m02 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 10)
					{
						value.m12 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 11)
					{
						value.m22 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 12)
					{
						value.m32 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 13)
					{
						value.m03 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 14)
					{
						value.m13 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 15)
					{
						value.m23 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 16)
					{
						value.m33 = ReadSystem_Single( );
						continue;
					}
				}
				return value;
			}
			throw new System.Exception("Can not ReadUnityEngine_Matrix4x4");
		}
		
	}
}
