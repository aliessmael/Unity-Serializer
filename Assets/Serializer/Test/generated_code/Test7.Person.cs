/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test7_Person_Serializer : Serializer
	{
		public Test7_Person_Serializer()
		{
			SerializerOf = typeof(Test7.Person);
			AddSubType(typeof(Test7.Friend), 1u);
			
			AddField(typeof(System.String), "name", 1);
			AddField(typeof(System.Int32), "age", 2);
			AddField(typeof(System.String), "phone", 3);
			AddField(typeof(System.String), "address", 4);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest7_Person( (Test7.Person)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest7_Person();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest7_Person( Test7.Person value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == null )
				return 0;
			
			int dataSize = 0;
			
			if( value.GetType() == typeof( Test7.Person ))
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
			Test7.Friend sub0 = value as Test7.Friend;
			if (sub0 != null)
			{
				dataSize += WriteSlotInfo( slot, 1 );
				dataSize += WriteTest7_Friend(sub0);
				return dataSize;
			}
			throw new System.Exception("Can not find serializer for " + value.GetType());
		}
		
		public Test7.Person ReadTest7_Person( uint subType = 0 )
		{
			if( subType == 0 )
			{
				Test7.Person value = new Test7.Person();
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
				return  ReadTest7_Friend( 0 );
			}
			throw new System.Exception("Can not ReadTest7_Person");
		}
		
	}
}
