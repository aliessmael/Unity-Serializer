using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;

public class SerializerTest : MonoBehaviour {

	/*class Test
	{
		public object Data ;
	}*/


	/**/

	Test[] tests ;
	int currentTest = -1 ;
	// Use this for initialization

	


	void Start () {
	
		try
		{
			tests = new Test[20];
			tests[0] 	= new Test1();
			tests[1] 	= new Test2();
			tests[2] 	= new Test3();
			tests[3] 	= new Test4();
			tests[4] 	= new Test5();
			tests[5] 	= new Test6();
			tests[6] 	= new Test7();
			tests[7] 	= new Test8();
			tests[8] 	= new Test9();
			tests[9] 	= new Test10();
			tests[10] 	= new Test11();
			tests[11] 	= new Test12();
			tests[12] 	= new Test13();
			tests[13] 	= new Test14();
			tests[14] 	= new Test15();
			tests[15] 	= new Test16();
			tests[16] 	= new Test17();
			tests[17] 	= new Test18();
			tests[18] 	= new Test19();
			tests[19] 	= new Test20();
		}
		catch( System.Exception e )
		{
			Debug.LogException( e );
		}

	}
	
	bool allTestDone = false ;
	int succeed = 0;
	float totalTime = 0 ;
	void OnGUI()
	{
		if( tests == null )
			return;
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();


		if( currentTest == -1 )
		{
			if( GUILayout.Button("Start Test step by step" ,GUILayout.Width(200) , GUILayout.Height( 50 ) ) )
			{
				currentTest = 0  ;
				tests[ currentTest ].DoTest();
			}
			if( GUILayout.Button("Do All Test" ,GUILayout.Width(200) , GUILayout.Height( 50 ) ))
			{
				succeed = 0 ;
				float t = Time.realtimeSinceStartup ;
				for( int i = 0 ; i < tests.Length ; i++ )
				{
					try{
						tests[i].DoTest();
					}
					catch( System.Exception e )
					{
						Debug.LogException( e );
					}
					if( tests[i].Succeed )
						succeed ++ ;
				}
				totalTime = Time.realtimeSinceStartup - t ;
				allTestDone = true ;
			}
			if( allTestDone )
			{
				GUILayout.Label( "Succeed Tests " + succeed +" / " + tests.Length );
				GUILayout.Label( "Total Time " + totalTime );
			}
		}
		else if( currentTest == 0 )
		{
			tests[ currentTest ].OnGUI();
			if( GUILayout.Button("Do Next Test" ,GUILayout.Width(200) , GUILayout.Height( 50 )) )
			{
				currentTest++ ;
				if(currentTest < tests.Length ) 
					tests[ currentTest ].DoTest();
				else 
					currentTest = -1 ;
			}
		}
		else if( currentTest == tests.Length - 1 )
		{
			tests[ currentTest ].OnGUI();
			if( GUILayout.Button("Do Previous Test" ,GUILayout.Width(200) , GUILayout.Height( 50 )) )
			{
				currentTest-- ;
				if(currentTest>=0) 
					tests[ currentTest ].DoTest();
				else 
					currentTest = tests.Length-1 ;
			}
		}
		else
		{
			tests[ currentTest ].OnGUI();
			GUILayout.BeginHorizontal();
			if( GUILayout.Button("Do Previous Test" ,GUILayout.Width(200) , GUILayout.Height( 50 )) )
			{
				currentTest-- ;
				if(currentTest>=0) 
					tests[ currentTest ].DoTest();
				else 
					currentTest = tests.Length-1 ;
			}
			if( GUILayout.Button("Do Next Test" ,GUILayout.Width(200) , GUILayout.Height( 50 )) )
			{
				currentTest++ ;
				if(currentTest<tests.Length) 
					tests[ currentTest ].DoTest();
				else 
					currentTest = -1 ;
			}
			GUILayout.EndHorizontal();
		}

	}

}
