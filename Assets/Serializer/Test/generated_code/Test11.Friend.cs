/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test11_Friend_Serializer : Serializer
	{
		public Test11_Friend_Serializer()
		{
			SerializerOf = typeof(Test11.Friend);
			
			AddField(typeof(System.String), "email", 1);
			AddField(typeof(System.String), "password", 2);
			AddField(typeof(System.String), "name", 3);
			AddField(typeof(System.Int32), "age", 4);
			AddField(typeof(System.String), "phone", 5);
			AddField(typeof(System.String), "address", 6);
			AddField(typeof(System.String), "address2", 7);
			AddField(typeof(System.Boolean), "isMale", 8);
			AddField(typeof(Test11.PersonTag), "tag", 9);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest11_Friend( (Test11.Friend)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest11_Friend();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest11_Friend( Test11.Friend value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == null )
				return 0;
			
			int dataSize = 0;
			
			if( slot != -1 )
			{
				dataSize += WriteSlotInfo( slot );
			}
			dataSize += WriteSystem_String(value.email, true, 1);
			dataSize += WriteSystem_String(value.password, true, 2);
			dataSize += WriteSystem_String(value.name, true, 3);
			dataSize += WriteSystem_Int32(value.age, true, 4);
			dataSize += WriteSystem_String(value.phone, true, 5);
			dataSize += WriteSystem_String(value.address, true, 6);
			dataSize += WriteSystem_String(value.address2, true, 7);
			dataSize += WriteSystem_Boolean(value.isMale, true, 8);
			dataSize += WriteTest11_PersonTag(value.tag, true, 9);
			dataSize += WriteSystem_UInt32(0u);
			return dataSize;
		}
		
		public Test11.Friend ReadTest11_Friend( uint subType = 0 )
		{
			if( subType == 0 )
			{
				Test11.Friend value = new Test11.Friend();
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
						value.email = ReadSystem_String( );
						continue;
					}
					if (fieldId == 2)
					{
						value.password = ReadSystem_String( );
						continue;
					}
					if (fieldId == 3)
					{
						value.name = ReadSystem_String( );
						continue;
					}
					if (fieldId == 4)
					{
						value.age = ReadSystem_Int32( );
						continue;
					}
					if (fieldId == 5)
					{
						value.phone = ReadSystem_String( );
						continue;
					}
					if (fieldId == 6)
					{
						value.address = ReadSystem_String( );
						continue;
					}
					if (fieldId == 7)
					{
						value.address2 = ReadSystem_String( );
						continue;
					}
					if (fieldId == 8)
					{
						value.isMale = ReadSystem_Boolean( );
						continue;
					}
					if (fieldId == 9)
					{
						value.tag = ReadTest11_PersonTag( typeId );
						continue;
					}
				}
				return value;
			}
			throw new System.Exception("Can not ReadTest11_Friend");
		}
		
	}
}
