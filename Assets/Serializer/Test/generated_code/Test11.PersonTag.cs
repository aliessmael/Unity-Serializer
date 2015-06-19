/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test11_PersonTag_Serializer : Serializer
	{
		public Test11_PersonTag_Serializer()
		{
			SerializerOf = typeof(Test11.PersonTag);
			
			AddEnumField("none", 1);
			AddEnumField("tag1", 2);
			AddEnumField("tag2", 3);
			AddEnumField("tag3", 4);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest11_PersonTag( (Test11.PersonTag)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest11_PersonTag();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest11_PersonTag( Test11.PersonTag value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == Test11.PersonTag.none)
				return 0;
			
			int dataSize = 0;
			
			if( slot != -1 )
			{
				dataSize += WriteSlotInfo( slot );
			}
			if ((value == Test11.PersonTag.none ))
			{
				dataSize += WriteSystem_UInt32(1);
				return dataSize;
			}
			if ((value == Test11.PersonTag.tag1 ))
			{
				dataSize += WriteSystem_UInt32(2);
				return dataSize;
			}
			if ((value == Test11.PersonTag.tag2 ))
			{
				dataSize += WriteSystem_UInt32(3);
				return dataSize;
			}
			if ((value == Test11.PersonTag.tag3 ))
			{
				dataSize += WriteSystem_UInt32(4);
				return dataSize;
			}
			throw new System.Exception(" Can not find index of " + value );
		}
		
		public Test11.PersonTag ReadTest11_PersonTag( uint subType = 0 )
		{
			uint enumId = ReadSystem_UInt32();
			if (enumId == 1)
			{
				return Test11.PersonTag.none;
			}
			if (enumId == 2)
			{
				return Test11.PersonTag.tag1;
			}
			if (enumId == 3)
			{
				return Test11.PersonTag.tag2;
			}
			if (enumId == 4)
			{
				return Test11.PersonTag.tag3;
			}
			throw new System.Exception("Can not find enum of index " + enumId);
		}
		
	}
}
