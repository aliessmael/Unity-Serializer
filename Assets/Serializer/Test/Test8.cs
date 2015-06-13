using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;

public class Test8: Test {

	override public void DoTest()
	{
		Description = "Test 8 Succeed : Serialize + Deserilaize Dictionary of built-in type ( Dictionary<ushort, string> with 10 items)" ;
		Succeed = false ;

		Dictionary<ushort, string> a = new Dictionary<ushort, string>();
		for( ushort i = 0 ; i < 10 ; i ++ )
			a[i] = "testvalue"+i ;
		a[10] = null ;

		float t = Time.realtimeSinceStartup ;
		byte[] d = SerializerSystem.Serialize( a );
		DataSize = d.Length ;
		SerializeTime = (Time.realtimeSinceStartup-t);
		t = Time.realtimeSinceStartup ;
		Dictionary<ushort, string> b = SerializerSystem.Deserialize<Dictionary<ushort, string>>( d );
		DeserializeTime = (Time.realtimeSinceStartup-t);

		foreach( KeyValuePair<ushort,string> pair in a )
		{
			if( !b.ContainsKey( pair.Key ))
				return ;
			if( string.IsNullOrEmpty( b[ pair.Key ]) && string.IsNullOrEmpty( pair.Value ))
				continue ;
			if( !b[ pair.Key ].Equals( pair.Value ) )
				return ;
		}
		
		
		
		Succeed = true ;
	}

}
