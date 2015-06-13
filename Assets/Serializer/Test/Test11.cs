using UnityEngine;
using System.Collections;
using cloudsoft ;

public class Test11: Test {

	override public void DoTest()
	{
		return;
		/*Description = "Test 11 Succeed : Serialize + Deserialize (SerializerSettings)";
		Succeed = false ;
		Serializers a = Serializers.Local ;
		float t = Time.realtimeSinceStartup ;
		byte[] d = Serializers.Serialize<Serializers>( a );
		DataSize = d.Length ;
		SerializeTime = (Time.realtimeSinceStartup-t);
		t = Time.realtimeSinceStartup ;

		Serializers b = Serializers.Deserialize<Serializers>( d );
		DeserializeTime = (Time.realtimeSinceStartup-t);

		for( int i = 0 ; i < a.serializers.Count ; i++  )
		{
			if( a.serializers.Count != b.serializers.Count )
			{
				Error = "count is not match";
				return ;
			}
			if( a.serializers[ i].SerializerOf != b.serializers[ i ].SerializerOf )
			{
				Error = "type is not match";
				return ;
			}
			if( a.serializers[ i].lastVersion != b.serializers[ i ].lastVersion )
			{
				Error = "version is not match";
				return ;
			}
			if( a.serializers[ i].holder != b.serializers[ i ].holder )
			{
				Error = "proxy is not match";
				return ;
			}
		}
		Succeed = true ;*/
		//tests[10].Data = "succeed , size is " + ((float)d.Length /1024) + " kb" ;
	}
}
