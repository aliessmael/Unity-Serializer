using UnityEngine;
using System.Collections;
using cloudsoft ;

public class Test17: Test {

	override public void DoTest()
	{
		Description = "Test 17 Succeed" ;
		Succeed = false ;

		/*float t = Time.realtimeSinceStartup ;
		DataSize = d.Length ;
		SerializeTime = (Time.realtimeSinceStartup-t);
		t = Time.realtimeSinceStartup ;
		DeserializeTime = (Time.realtimeSinceStartup-t);*/
		Succeed = true ;
	}

}
