/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test11_Person_Serializer : Serializer
	{
		public Test11_Person_Serializer()
		{
			SerializerOf = typeof(Test11.Person);
			AddSubType(typeof(Test11.Friend), 1u);
			
			AddField(typeof(System.String), "name", 1);
			AddField(typeof(System.Int32), "age", 2);
			AddField(typeof(System.String), "phone", 3);
			AddField(typeof(System.String), "address", 4);
			AddField(typeof(System.String), "address2", 5);
			AddField(typeof(System.Boolean), "isMale", 6);
			AddField(typeof(Test11.PersonTag), "tag", 7);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest11_Person( (Test11.Person)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest11_Person();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest11_Person( Test11.Person value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == null )
				return 0;
			
			int dataSize = 0;
			
			if( value.GetType() == typeof( Test11.Person ))
			{
				if( slot != -1 )
				{
					dataSize += WriteSlotInfo( slot );
				}
				dataSize += WriteSystem_String(value.name, true, 1);
				dataSize += WriteSystem_Int32(value.age, true, 2);
				dataSize += WriteSystem_String(value.phone, true, 3);
				dataSize += WriteSystem_String(value.address, true, 4);
				dataSize += WriteSystem_String(value.address2, true, 5);
				dataSize += WriteSystem_Boolean(value.isMale, true, 6);
				dataSize += WriteTest11_PersonTag(value.tag, true, 7);
				dataSize += WriteSystem_UInt32(0u);
				return dataSize;
			}
			Test11.Friend sub0 = value as Test11.Friend;
			if (sub0 != null)
			{
				dataSize += WriteSlotInfo( slot, 1 );
				dataSize += WriteTest11_Friend(sub0);
				return dataSize;
			}
			throw new System.Exception("Can not find serializer for " + value.GetType());
		}
		
		public Test11.Person ReadTest11_Person( uint subType = 0 )
		{
			if( subType == 0 )
			{
				Test11.Person value = new Test11.Person();
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
						value.name = ReadSystem_String( );
						continue;
					}
					if (fieldId == 2)
					{
						value.age = ReadSystem_Int32( );
						continue;
					}
					if (fieldId == 3)
					{
						value.phone = ReadSystem_String( );
						continue;
					}
					if (fieldId == 4)
					{
						value.address = ReadSystem_String( );
						continue;
					}
					if (fieldId == 5)
					{
						value.address2 = ReadSystem_String( );
						continue;
					}
					if (fieldId == 6)
					{
						value.isMale = ReadSystem_Boolean( );
						continue;
					}
					if (fieldId == 7)
					{
						value.tag = ReadTest11_PersonTag( typeId );
						continue;
					}
				}
				return value;
			}
			if (subType == 1)
			{
				return  ReadTest11_Friend( 0 );
			}
			throw new System.Exception("Can not ReadTest11_Person");
		}
		
	}
}
