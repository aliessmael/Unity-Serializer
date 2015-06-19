using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_MonoType_Serializer : Serializer {

		public System_MonoType_Serializer()
		{
			System.Type t = typeof(int);//System.MonoType is not accessable, I access it by trick
			SerializerOf = t.GetType();
			ManualChange = true;
			CanHasSubtype = false;
		}

		public override object Read( SerializerStream stream, System.Type _type )
		{
			return stream.ReadSystem_Type();
			
		}
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteSystem_Type( (System.Type)value);

		}
		
	}

}
