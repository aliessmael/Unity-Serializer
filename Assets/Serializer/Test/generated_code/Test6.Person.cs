/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test6_Person_Serializer : Serializer
	{
		public Test6_Person_Serializer()
		{
			SerializerOf = typeof(Test6.Person);
			AddSubType(typeof(Test6.Friend), 1u);
			
			AddField(typeof(System.String), "name", 1);
			AddField(typeof(System.Int32), "age", 2);
			AddField(typeof(System.String), "phone", 3);
			AddField(typeof(System.String), "address", 4);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest6_Person( (Test6.Person)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest6_Person();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest6_Person( Test6.Person value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == null )
				return 0;
			
			int dataSize = 0;
			
			if( value.GetType() == typeof( Test6.Person ))
			{
				if( slot != -1 )
				{
					dataSize += WriteSlotInfo( slot );
				}
				dataSize += WriteSystem_String(value.name, true, 1);
				dataSize += WriteSystem_Int32(value.age, true, 2);
				dataSize += WriteSystem_String(value.phone, true, 3);
				dataSize += WriteSystem_String(value.address, true, 4);
				dataSize += WriteSystem_UInt32(0u);
				return dataSize;
			}
			Test6.Friend sub0 = value as Test6.Friend;
			if (sub0 != null)
			{
				dataSize += WriteSlotInfo( slot, 1 );
				dataSize += WriteTest6_Friend(sub0);
				return dataSize;
			}
			throw new System.Exception("Can not find serializer for " + value.GetType());
		}
		
		public Test6.Person ReadTest6_Person( uint subType = 0 )
		{
			if( subType == 0 )
			{
				Test6.Person value = new Test6.Person();
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
				}
				return value;
			}
			if (subType == 1)
			{
				return  ReadTest6_Friend( 0 );
			}
			throw new System.Exception("Can not ReadTest6_Person");
		}
		
	}
}
