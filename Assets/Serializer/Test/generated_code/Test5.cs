namespace cloudsoft
{
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	public class Test5_Serializer : Serializer
	{
		public Test5_Serializer()
		{
			SerializerOf = typeof(Test5);
			
			AddField(typeof(System.Boolean), "Succeed", 1);
			AddField(typeof(System.Single), "DataSize", 2);
			AddField(typeof(System.Single), "SerializeTime", 3);
			AddField(typeof(System.Single), "DeserializeTime", 4);
			AddField(typeof(System.String), "Description", 5);
			AddField(typeof(System.String), "Error", 6);
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteTest5( (Test5)value);
		}
		
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadTest5();
		}
		
	}
	
	public partial class SerializerStream
	{
		public int WriteTest5( Test5 value)
		{
			int dataSize = 0;
			if ( value.Succeed != false)
			{
				dataSize += WriteFieldInfo(1);
				dataSize += WriteSystem_Boolean(value.Succeed);
			}
			if ( value.DataSize != 0)
			{
				dataSize += WriteFieldInfo(2);
				dataSize += WriteSystem_Single(value.DataSize);
			}
			if ( value.SerializeTime != 0)
			{
				dataSize += WriteFieldInfo(3);
				dataSize += WriteSystem_Single(value.SerializeTime);
			}
			if ( value.DeserializeTime != 0)
			{
				dataSize += WriteFieldInfo(4);
				dataSize += WriteSystem_Single(value.DeserializeTime);
			}
			if ( !string.IsNullOrEmpty( value.Description))
			{
				dataSize += WriteFieldInfo(5);
				dataSize += WriteSystem_String(value.Description);
			}
			if ( !string.IsNullOrEmpty( value.Error))
			{
				dataSize += WriteFieldInfo(6);
				dataSize += WriteSystem_String(value.Error);
			}
			dataSize += WriteSystem_UInt32(0u);
			return dataSize;
		}
		
		public Test5 ReadTest5( )
		{
			Test5 value = new Test5();
			int fieldId = 1;
			uint typeId = 1;
			for ( ;  ;  )
			{
				ReadFieldInfo(out fieldId, out typeId);
				if (fieldId == 0)
				{
					break;
				}
				if (fieldId == 1) // Succeed
				{
					if (typeId == 0)
					{
						value.Succeed = ReadSystem_Boolean();
						continue;
					}
				}
				if (fieldId == 2) // DataSize
				{
					if (typeId == 0)
					{
						value.DataSize = ReadSystem_Single();
						continue;
					}
				}
				if (fieldId == 3) // SerializeTime
				{
					if (typeId == 0)
					{
						value.SerializeTime = ReadSystem_Single();
						continue;
					}
				}
				if (fieldId == 4) // DeserializeTime
				{
					if (typeId == 0)
					{
						value.DeserializeTime = ReadSystem_Single();
						continue;
					}
				}
				if (fieldId == 5) // Description
				{
					if (typeId == 0)
					{
						value.Description = ReadSystem_String();
						continue;
					}
				}
				if (fieldId == 6) // Error
				{
					if (typeId == 0)
					{
						value.Error = ReadSystem_String();
						continue;
					}
				}
			}
			return value;
		}
		
	}
}
