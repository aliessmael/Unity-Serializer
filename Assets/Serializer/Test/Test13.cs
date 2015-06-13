using UnityEngine;
using System.Collections;
using cloudsoft ;

public class Test13: Test {

	override public void DoTest()
	{
		Description = "Test 13 Succeed" ;
		Succeed = false ;

		/*float t = Time.realtimeSinceStartup ;
		DataSize = d.Length ;
		SerializeTime = (Time.realtimeSinceStartup-t);
		t = Time.realtimeSinceStartup ;
		DeserializeTime = (Time.realtimeSinceStartup-t);*/
		Succeed = true ;
	}

}
