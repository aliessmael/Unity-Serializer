using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;

public class Test12: Test {
	
	public class TestClass
	{
		public List<Vector3> vectors ;
		public List<Quaternion> quaternions ;
	}

	override public void DoTest()
	{
		Description = "Test 12 Succeed" ;
		Succeed = false ;

		TestClass a = new TestClass();
		a.vectors = new List<Vector3>();
		a.quaternions = new List<Quaternion>();

		for( int i = 0 ; i < 100000 ; i++ )
		{
			a.vectors.Add( new Vector3( Random.Range(1,1000) , Random.Range(1,1000) , Random.Range(1,1000) ));
			a.quaternions.Add( new Quaternion( Random.Range(1,1000) , Random.Range(1,1000) , Random.Range(1,1000) ,Random.Range(1,1000) ));
		}

		float t = Time.realtimeSinceStartup ;
		byte[] d = SerializerSystem.Serialize( a );
		DataSize = d.Length ;
		SerializeTime = (Time.realtimeSinceStartup-t);
		t = Time.realtimeSinceStartup ;
		TestClass b = SerializerSystem.Deserialize<TestClass>( d );
		DeserializeTime = (Time.realtimeSinceStartup-t);

		if( a.vectors.Count != b.vectors.Count )
			return ;
		if( a.quaternions.Count != b.quaternions.Count )
			return ;
		for( int i = 0 ; i < 100000 ; i++ )
		{
			if( a.vectors[i].x != b.vectors[i].x )
				return ;
			if( a.vectors[i].y != b.vectors[i].y )
				return ;
			if( a.vectors[i].z != b.vectors[i].z )
				return ;

			if( a.quaternions[i].x != b.quaternions[i].x )
				return ;
			if( a.quaternions[i].y != b.quaternions[i].y )
				return ;
			if( a.quaternions[i].z != b.quaternions[i].z )
				return ;
			if( a.quaternions[i].w != b.quaternions[i].w )
				return ;
		}

		Succeed = true ;
	}

}
