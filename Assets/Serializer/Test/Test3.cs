using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;


public class Test3: Test {

	override public void DoTest()
	{
		Description = "Test 3 Succeed : Serialize + Deserialize List of all built-in types" ;
		Succeed = false ;

		List<ushort> a = new List<ushort>();
		a.Add( (ushort)Random.Range( 0 , 1000));
		a.Add( (ushort)Random.Range( 0 , 1000));
		a.Add( (ushort)Random.Range( 0 , 1000));
		float t = Time.realtimeSinceStartup ;
		byte[] d =  SerializerSystem.Serialize( a );
		DataSize = d.Length ;
		SerializeTime = ( Time.realtimeSinceStartup - t ) ;
		t = Time.realtimeSinceStartup ;
		List<ushort> b = SerializerSystem.Deserialize<List<ushort>>( d );
		DeserializeTime = (Time.realtimeSinceStartup-t);
		if( a.Count == b.Count )
		{
			for( int i = 0 ; i < a.Count ; i++ )
			{
				if( a[i] != b[i] )
				{
					return;
				}
			}
			
		}
		Succeed = true ;
	}

}
