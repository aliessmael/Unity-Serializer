using UnityEngine;
using System.Collections;
using cloudsoft ;

public class Test9: Test {

	Texture2D a = null ;
	Texture2D b = null ;
	override public void DoTest()
	{
		Description = "Test 9 Succeed : Serialize Deserialize UnityEngine.Texture2D by using proxy, check Texture2DProxy class" ;
		Succeed = false ;

		a = Resources.Load<Texture2D>("serializer");
		float t = Time.realtimeSinceStartup ;
		byte[] d = SerializerSystem.Serialize( a );
		DataSize = d.Length ;
		SerializeTime = (Time.realtimeSinceStartup-t);
		t = Time.realtimeSinceStartup ;

		b = SerializerSystem.Deserialize<Texture2D>( d );
		DeserializeTime = (Time.realtimeSinceStartup-t);
		//tests[8].Data = b ;
		Succeed = true ;
	}
	
	override public void OnGUI()
	{
		base.OnGUI();
		if( Succeed )
		{
			GUILayout.Box( new GUIContent( " Serialized image" ,a ) );
			GUILayout.Box( new GUIContent( " Deserialized image" ,b ) );

		}

	}
}
