/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test11_TestClass_Serializer : Serializer
	{
		public Test11_TestClass_Serializer()
		{
			SerializerOf = typeof(Test11.TestClass);
			
			AddField(typeof(Test11.Person), "bestFriend", 1);
			AddField(typeof(Test11.Person), "worsFriend", 2);
			AddField(typeof(List<Test11.Person>), "friends", 3);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest11_TestClass( (Test11.TestClass)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest11_TestClass();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest11_TestClass( Test11.TestClass value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == null )
				return 0;
			
			int dataSize = 0;
			
			if( slot != -1 )
			{
				dataSize += WriteSlotInfo( slot );
			}
			dataSize += WriteTest11_Person(value.bestFriend, true, 1);
			dataSize += WriteTest11_Person(value.worsFriend, true, 2);
			dataSize += WriteList( value.friends, true, 3);
			dataSize += WriteSystem_UInt32(0u);
			return dataSize;
		}
		
		public Test11.TestClass ReadTest11_TestClass( uint subType = 0 )
		{
			if( subType == 0 )
			{
				Test11.TestClass value = new Test11.TestClass();
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
						value.bestFriend = ReadTest11_Person( typeId );
						continue;
					}
					if (fieldId == 2)
					{
						value.worsFriend = ReadTest11_Person( typeId );
						continue;
					}
					if (fieldId == 3)
					{
						value.friends = (List<Test11.Person>)ReadList(typeof(List<Test11.Person>) );
						continue;
					}
				}
				return value;
			}
			throw new System.Exception("Can not ReadTest11_TestClass");
		}
		
	}
}
