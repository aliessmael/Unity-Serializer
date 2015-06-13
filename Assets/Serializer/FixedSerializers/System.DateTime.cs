using System.IO;
using System.Collections;

namespace cloudsoft
{
	public class System_DateTime_Serializer : Serializer {

		public static System.DateTime defaultValue = new System.DateTime();
		public System_DateTime_Serializer()
		{
			SerializerOf = typeof(System.DateTime);
			ManualChange = true;
		}


		public override object Read( SerializerStream stream, System.Type type )
		{
			double d = stream.ReadSystem_Double();
			return System.DateTime.FromOADate( d );
			
		}
		public override int Write( SerializerStream stream, object obj )
		{
			System.DateTime d = (System.DateTime)obj;
			return stream.WriteSystem_Double(  d.ToOADate());
		}
		
	}
	

	public partial class SerializerStream
	{

		public int WriteSystem_DateTime(System.DateTime value)
		{
			return WriteSystem_Double(value.ToOADate());
		}
		public System.DateTime ReadSystem_DateTime()
		{
			double d = ReadSystem_Double();
			return System.DateTime.FromOADate(d);
		}
	}
}

