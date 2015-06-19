using UnityEngine;
using System.Collections;
using cloudsoft ;

public class Test10: SerializerTestUnit {

	public Test10()
	{
		Id = 10;
	}

	public class TestClass
	{
		public Texture2D faceImage ;
	}
	TestClass a = null ;
	TestClass b = null ;
	override public void DoTest()
	{
		Description = "Test 10 Succeed : Serialize + Deserilaize custom class contains UnityEngine.Texture2D";
		Succeed = false ;

		a = new TestClass();
		a.faceImage = Resources.Load<Texture2D>("serializer");
		float t = Time.realtimeSinceStartup ;
		byte[] d =  SerializerSystem.Serialize( a );
		DataSize = d.Length ;
		SerializeTime = (Time.realtimeSinceStartup-t);
		t = Time.realtimeSinceStartup ;

		b = SerializerSystem.Deserialize<TestClass>( d );
		DeserializeTime = (Time.realtimeSinceStartup-t);
		Succeed = true ;
		//tests[9].Data = b.faceImage ;
	}
	
	override public void OnGUI()
	{
		base.OnGUI();
		if( Succeed )
		{
			GUILayout.Box( new GUIContent( " Serialized image" ,a.faceImage ) );
			GUILayout.Box( new GUIContent( " Deserialized image" ,b.faceImage ) );

		}
	}
}
