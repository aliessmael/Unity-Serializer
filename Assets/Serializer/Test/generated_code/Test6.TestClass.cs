/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test6_TestClass_Serializer : Serializer
	{
		public Test6_TestClass_Serializer()
		{
			SerializerOf = typeof(Test6.TestClass);
			
			AddField(typeof(Test6.Person), "bestFriend", 1);
			AddField(typeof(Test6.Person), "worsFriend", 2);
			AddField(typeof(List<Test6.Person>), "friends", 3);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest6_TestClass( (Test6.TestClass)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest6_TestClass();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest6_TestClass( Test6.TestClass value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == null )
				return 0;
			
			int dataSize = 0;
			
			if( slot != -1 )
			{
				dataSize += WriteSlotInfo( slot );
			}
			dataSize += WriteTest6_Person(value.bestFriend, true, 1);
			dataSize += WriteTest6_Person(value.worsFriend, true, 2);
			dataSize += WriteList( value.friends, true, 3);
			dataSize += WriteSystem_UInt32(0u);
			return dataSize;
		}
		
		public Test6.TestClass ReadTest6_TestClass( uint subType = 0 )
		{
			if( subType == 0 )
			{
				Test6.TestClass value = new Test6.TestClass();
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
						value.bestFriend = ReadTest6_Person( typeId );
						continue;
					}
					if (fieldId == 2)
					{
						value.worsFriend = ReadTest6_Person( typeId );
						continue;
					}
					if (fieldId == 3)
					{
						value.friends = (List<Test6.Person>)ReadList(typeof(List<Test6.Person>) );
						continue;
					}
				}
				return value;
			}
			throw new System.Exception("Can not ReadTest6_TestClass");
		}
		
	}
}
