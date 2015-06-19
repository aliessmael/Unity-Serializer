/*
* Generated from ALI-PC at 6/19/2015 11:40:28 AM
* */

namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test12_TestClass_Serializer : Serializer
	{
		public Test12_TestClass_Serializer()
		{
			SerializerOf = typeof(Test12.TestClass);
			
			AddField(typeof(List<UnityEngine.Vector3>), "vectors", 1);
			AddField(typeof(List<UnityEngine.Quaternion>), "quaternions", 2);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest12_TestClass( (Test12.TestClass)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest12_TestClass();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest12_TestClass( Test12.TestClass value, bool dontWriteDefault = false, int slot = -1)
		{
			if ( dontWriteDefault && value == null )
				return 0;
			
			int dataSize = 0;
			
			if( slot != -1 )
			{
				dataSize += WriteSlotInfo( slot );
			}
			dataSize += WriteList( value.vectors, true, 1);
			dataSize += WriteList( value.quaternions, true, 2);
			dataSize += WriteSystem_UInt32(0u);
			return dataSize;
		}
		
		public Test12.TestClass ReadTest12_TestClass( uint subType = 0 )
		{
			if( subType == 0 )
			{
				Test12.TestClass value = new Test12.TestClass();
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
						value.vectors = (List<UnityEngine.Vector3>)ReadList(typeof(List<UnityEngine.Vector3>) );
						continue;
					}
					if (fieldId == 2)
					{
						value.quaternions = (List<UnityEngine.Quaternion>)ReadList(typeof(List<UnityEngine.Quaternion>) );
						continue;
					}
				}
				return value;
			}
			throw new System.Exception("Can not ReadTest12_TestClass");
		}
		
	}
}
