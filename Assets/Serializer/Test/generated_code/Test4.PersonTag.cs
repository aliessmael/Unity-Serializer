namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test4_PersonTag : Serializer
	{
		public Test4_PersonTag()
		{
			SerializerOf = typeof(Test4.PersonTag);
			
			AddEnumField("none", 1);
			AddEnumField("tag1", 2);
			AddEnumField("tag2", 3);
			AddEnumField("tag3", 4);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest4_PersonTag( (Test4.PersonTag)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest4_PersonTag();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest4_PersonTag( Test4.PersonTag value)
		{
			int dataSize = 0;
			if ((value == Test4.PersonTag.none ))
			{
				dataSize += WriteSystem_UInt32(1);
				return dataSize;
			}
			if ((value == Test4.PersonTag.tag1 ))
			{
				dataSize += WriteSystem_UInt32(2);
				return dataSize;
			}
			if ((value == Test4.PersonTag.tag2 ))
			{
				dataSize += WriteSystem_UInt32(3);
				return dataSize;
			}
			if ((value == Test4.PersonTag.tag3 ))
			{
				dataSize += WriteSystem_UInt32(4);
				return dataSize;
			}
			throw new System.Exception(" Can not find index of " + value );
		}
		
		public Test4.PersonTag ReadTest4_PersonTag( )
		{
			uint enumId = ReadSystem_UInt32();
			if (enumId == 1)
			{
				return Test4.PersonTag.none;
			}
			if (enumId == 2)
			{
				return Test4.PersonTag.tag1;
			}
			if (enumId == 3)
			{
				return Test4.PersonTag.tag2;
			}
			if (enumId == 4)
			{
				return Test4.PersonTag.tag3;
			}
			throw new System.Exception("Can not find enum of index " + enumId);
		}
		
	}
}
