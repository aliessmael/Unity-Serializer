/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test5_CustomClass_Serializer : Serializer
	{
		public Test5_CustomClass_Serializer()
		{
			SerializerOf = typeof(Test5.CustomClass);
			
			AddField(typeof(System.Boolean), "member0", 1);
			AddField(typeof(System.Boolean), "member1", 2);
			AddField(typeof(System.Byte), "member2", 3);
			AddField(typeof(System.SByte), "member3", 4);
			AddField(typeof(System.Char), "member4", 5);
			AddField(typeof(System.Int16), "member5", 6);
			AddField(typeof(System.UInt16), "member6", 7);
			AddField(typeof(System.Int32), "member7", 8);
			AddField(typeof(System.UInt32), "member8", 9);
			AddField(typeof(System.Int64), "member9", 10);
			AddField(typeof(System.UInt64), "member10", 11);
			AddField(typeof(System.Single), "member11", 12);
			AddField(typeof(System.Double), "member12", 13);
			AddField(typeof(System.Decimal), "member13", 14);
			AddField(typeof(System.String), "member14", 15);
			AddField(typeof(System.String), "member15", 16);
			AddField(typeof(System.Type), "member16", 17);
			AddField(typeof(System.Type), "member17", 18);
			AddField(typeof(System.DateTime), "member18", 19);
			AddField(typeof(UnityEngine.Color), "member19", 20);
			AddField(typeof(UnityEngine.Color), "member20", 21);
			AddField(typeof(UnityEngine.Vector2), "member21", 22);
			AddField(typeof(UnityEngine.Vector2), "member22", 23);
			AddField(typeof(UnityEngine.Vector3), "member23", 24);
			AddField(typeof(UnityEngine.Vector3), "member24", 25);
			AddField(typeof(UnityEngine.Vector4), "member25", 26);
			AddField(typeof(UnityEngine.Vector4), "member26", 27);
			AddField(typeof(UnityEngine.Quaternion), "member27", 28);
			AddField(typeof(UnityEngine.Quaternion), "member28", 29);
			AddField(typeof(UnityEngine.Matrix4x4), "member29", 30);
			AddField(typeof(UnityEngine.Matrix4x4), "member30", 31);
			AddField(typeof(System.Int32[]), "member31", 32);
			AddField(typeof(System.Int32[]), "member32", 33);
			AddField(typeof(List<System.Int32>), "member33", 34);
			AddField(typeof(List<System.Int32>), "member34", 35);
			AddField(typeof(Dictionary<System.Int32,System.Int32>), "member35", 36);
			AddField(typeof(Dictionary<System.Int32,System.Int32>), "member36", 37);
			AddField(typeof(System.Object), "member37", 38);
			AddField(typeof(System.Object), "member38", 39);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest5_CustomClass( (Test5.CustomClass)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest5_CustomClass();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest5_CustomClass( Test5.CustomClass value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == null )
				return 0;
			
			int dataSize = 0;
			
			if( slot != -1 )
			{
				dataSize += WriteSlotInfo( slot );
			}
			dataSize += WriteSystem_Boolean(value.member0, true, 1);
			dataSize += WriteSystem_Boolean(value.member1, true, 2);
			dataSize += WriteSystem_Byte(value.member2, true, 3);
			dataSize += WriteSystem_SByte(value.member3, true, 4);
			dataSize += WriteSystem_Char(value.member4, true, 5);
			dataSize += WriteSystem_Int16(value.member5, true, 6);
			dataSize += WriteSystem_UInt16(value.member6, true, 7);
			dataSize += WriteSystem_Int32(value.member7, true, 8);
			dataSize += WriteSystem_UInt32(value.member8, true, 9);
			dataSize += WriteSystem_Int64(value.member9, true, 10);
			dataSize += WriteSystem_UInt64(value.member10, true, 11);
			dataSize += WriteSystem_Single(value.member11, true, 12);
			dataSize += WriteSystem_Double(value.member12, true, 13);
			dataSize += WriteSystem_Decimal(value.member13, true, 14);
			dataSize += WriteSystem_String(value.member14, true, 15);
			dataSize += WriteSystem_String(value.member15, true, 16);
			dataSize += WriteSystem_Type(value.member16, true, 17);
			dataSize += WriteSystem_Type(value.member17, true, 18);
			dataSize += WriteSystem_DateTime(value.member18, true, 19);
			dataSize += WriteUnityEngine_Color(value.member19, true, 20);
			dataSize += WriteUnityEngine_Color(value.member20, true, 21);
			dataSize += WriteUnityEngine_Vector2(value.member21, true, 22);
			dataSize += WriteUnityEngine_Vector2(value.member22, true, 23);
			dataSize += WriteUnityEngine_Vector3(value.member23, true, 24);
			dataSize += WriteUnityEngine_Vector3(value.member24, true, 25);
			dataSize += WriteUnityEngine_Vector4(value.member25, true, 26);
			dataSize += WriteUnityEngine_Vector4(value.member26, true, 27);
			dataSize += WriteUnityEngine_Quaternion(value.member27, true, 28);
			dataSize += WriteUnityEngine_Quaternion(value.member28, true, 29);
			dataSize += WriteUnityEngine_Matrix4x4(value.member29, true, 30);
			dataSize += WriteUnityEngine_Matrix4x4(value.member30, true, 31);
			dataSize += WriteArray( value.member31, true, 32);
			dataSize += WriteArray( value.member32, true, 33);
			dataSize += WriteList( value.member33, true, 34);
			dataSize += WriteList( value.member34, true, 35);
			dataSize += WriteDictionary( value.member35, true, 36 );
			dataSize += WriteDictionary( value.member36, true, 37 );
			dataSize += WriteSystem_Object(value.member38, true, 39);
			dataSize += WriteSystem_UInt32(0u);
			return dataSize;
		}
		
		public Test5.CustomClass ReadTest5_CustomClass( uint subType = 0 )
		{
			if( subType == 0 )
			{
				Test5.CustomClass value = new Test5.CustomClass();
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
						value.member0 = ReadSystem_Boolean( );
						continue;
					}
					if (fieldId == 2)
					{
						value.member1 = ReadSystem_Boolean( );
						continue;
					}
					if (fieldId == 3)
					{
						value.member2 = ReadSystem_Byte( );
						continue;
					}
					if (fieldId == 4)
					{
						value.member3 = ReadSystem_SByte( );
						continue;
					}
					if (fieldId == 5)
					{
						value.member4 = ReadSystem_Char( );
						continue;
					}
					if (fieldId == 6)
					{
						value.member5 = ReadSystem_Int16( );
						continue;
					}
					if (fieldId == 7)
					{
						value.member6 = ReadSystem_UInt16( );
						continue;
					}
					if (fieldId == 8)
					{
						value.member7 = ReadSystem_Int32( );
						continue;
					}
					if (fieldId == 9)
					{
						value.member8 = ReadSystem_UInt32( );
						continue;
					}
					if (fieldId == 10)
					{
						value.member9 = ReadSystem_Int64( );
						continue;
					}
					if (fieldId == 11)
					{
						value.member10 = ReadSystem_UInt64( );
						continue;
					}
					if (fieldId == 12)
					{
						value.member11 = ReadSystem_Single( );
						continue;
					}
					if (fieldId == 13)
					{
						value.member12 = ReadSystem_Double( );
						continue;
					}
					if (fieldId == 14)
					{
						value.member13 = ReadSystem_Decimal( );
						continue;
					}
					if (fieldId == 15)
					{
						value.member14 = ReadSystem_String( );
						continue;
					}
					if (fieldId == 16)
					{
						value.member15 = ReadSystem_String( );
						continue;
					}
					if (fieldId == 17)
					{
						value.member16 = ReadSystem_Type( );
						continue;
					}
					if (fieldId == 18)
					{
						value.member17 = ReadSystem_Type( );
						continue;
					}
					if (fieldId == 19)
					{
						value.member18 = ReadSystem_DateTime( );
						continue;
					}
					if (fieldId == 20)
					{
						value.member19 = ReadUnityEngine_Color( typeId );
						continue;
					}
					if (fieldId == 21)
					{
						value.member20 = ReadUnityEngine_Color( typeId );
						continue;
					}
					if (fieldId == 22)
					{
						value.member21 = ReadUnityEngine_Vector2( typeId );
						continue;
					}
					if (fieldId == 23)
					{
						value.member22 = ReadUnityEngine_Vector2( typeId );
						continue;
					}
					if (fieldId == 24)
					{
						value.member23 = ReadUnityEngine_Vector3( typeId );
						continue;
					}
					if (fieldId == 25)
					{
						value.member24 = ReadUnityEngine_Vector3( typeId );
						continue;
					}
					if (fieldId == 26)
					{
						value.member25 = ReadUnityEngine_Vector4( typeId );
						continue;
					}
					if (fieldId == 27)
					{
						value.member26 = ReadUnityEngine_Vector4( typeId );
						continue;
					}
					if (fieldId == 28)
					{
						value.member27 = ReadUnityEngine_Quaternion( typeId );
						continue;
					}
					if (fieldId == 29)
					{
						value.member28 = ReadUnityEngine_Quaternion( typeId );
						continue;
					}
					if (fieldId == 30)
					{
						value.member29 = ReadUnityEngine_Matrix4x4( typeId );
						continue;
					}
					if (fieldId == 31)
					{
						value.member30 = ReadUnityEngine_Matrix4x4( typeId );
						continue;
					}
					if (fieldId == 32)
					{
						value.member31 = (System.Int32[])ReadArray(typeof(System.Int32[]) );
						continue;
					}
					if (fieldId == 33)
					{
						value.member32 = (System.Int32[])ReadArray(typeof(System.Int32[]) );
						continue;
					}
					if (fieldId == 34)
					{
						value.member33 = (List<System.Int32>)ReadList(typeof(List<System.Int32>) );
						continue;
					}
					if (fieldId == 35)
					{
						value.member34 = (List<System.Int32>)ReadList(typeof(List<System.Int32>) );
						continue;
					}
					if (fieldId == 36)
					{
						value.member35 = (Dictionary<System.Int32, System.Int32>)ReadDictionary(typeof(Dictionary<System.Int32, System.Int32>) );
						continue;
					}
					if (fieldId == 37)
					{
						value.member36 = (Dictionary<System.Int32, System.Int32>)ReadDictionary(typeof(Dictionary<System.Int32, System.Int32>) );
						continue;
					}
					if (fieldId == 39)
					{
						value.member38 = ReadSystem_Object( );
						continue;
					}
				}
				return value;
			}
			throw new System.Exception("Can not ReadTest5_CustomClass");
		}
		
	}
}
