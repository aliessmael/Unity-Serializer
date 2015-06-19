using UnityEngine;
using System.Collections;
using cloudsoft ;


public class TestClass
{
	public object value;
	public byte[] d ;
}

public class SerializerTestUnit {

	public int 		Id ;
	public bool  	Succeed ;
	public float 	DataSize ;
	public float 	SerializeTime ;
	public float 	DeserializeTime ;
	public string 	Description ;
	public string 	Error ;

	virtual public void DoTest()
	{
	}

	virtual public void OnGUI()
	{
		if( Succeed )
		{
			GUILayout.Label( "Succeed" + Description );
			GUILayout.Label( "Data Size " + DataSize + " Bytes");
			GUILayout.Label( "SerializeTime " + SerializeTime + " Seconds");
			GUILayout.Label( "DeserializeTime " + DeserializeTime + " Seconds" );
		}
		else 
		{
			GUILayout.Label( "Failed : " + Description );
			GUILayout.Label( Error );
		}

	}
}
