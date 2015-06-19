using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;
using System.Reflection;
using System;

public class SerializerTest : MonoBehaviour {

	/*class Test
	{
		public object Data ;
	}*/


	/**/

	List<SerializerTestUnit> tests = new List<SerializerTestUnit>();
	int currentTest = -1 ;
	// Use this for initialization

	


	void Start () {
	
		try
		{
			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type t in a.GetTypes())
				{
					if( t.IsSubclassOf( typeof(SerializerTestUnit)))
					{
						SerializerTestUnit r = (SerializerTestUnit)Activator.CreateInstance( t );

						tests.Add( r );
					}
				}
				
			}

			tests.Sort((a,b) => 
			              { 
				return a.Id.CompareTo(b.Id);
			} );
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
			GUILayout.BeginHorizontal();
			if( GUILayout.Button("Do All Test" ,GUILayout.Width(200) , GUILayout.Height( 50 ) ))
			{
				succeed = 0 ;
				float t = Time.realtimeSinceStartup ;
				for( int i = 0 ; i < tests.Count ; i++ )
				{
					try{
						tests[i].DoTest();
					}
					catch( System.Exception e )
					{
						Debug.LogException( e );
						Debug.LogError("Test " + i + " Failed");
					}
					if( tests[i].Succeed )
						succeed ++ ;
				}
				totalTime = Time.realtimeSinceStartup - t ;
				allTestDone = true ;
			}
			if( GUILayout.Button("Start Test step by step" ,GUILayout.Width(200) , GUILayout.Height( 50 ) ) )
			{
				currentTest = 0  ;
				tests[ currentTest ].DoTest();
			}

			GUILayout.EndHorizontal();
			if( allTestDone )
			{
				GUILayout.Label( "Succeed Tests " + succeed +" / " + tests.Count );
				GUILayout.Label( "Total Time " + totalTime );
			}
		}
		else if( currentTest == 0 )
		{
			GUILayout.BeginHorizontal();
			if( GUILayout.Button("Home" ,GUILayout.Width(200) , GUILayout.Height( 50 )) )
			{
				currentTest = -1;
			}
			if( GUILayout.Button("Do Next Test" ,GUILayout.Width(200) , GUILayout.Height( 50 )) )
			{
				currentTest++ ;
				if(currentTest < tests.Count ) 
					tests[ currentTest ].DoTest();
				else 
					currentTest = -1 ;
			}
			GUILayout.EndHorizontal();
			if( currentTest >= 0 )
				tests[ currentTest ].OnGUI();
		}
		else if( currentTest == tests.Count - 1 )
		{
			GUILayout.BeginHorizontal();
			if( GUILayout.Button("Do Previous Test" ,GUILayout.Width(200) , GUILayout.Height( 50 )) )
			{
				currentTest-- ;
				if(currentTest>=0) 
					tests[ currentTest ].DoTest();
				else 
					currentTest = tests.Count-1 ;
			}
			if( GUILayout.Button("Home" ,GUILayout.Width(200) , GUILayout.Height( 50 )) )
			{
				currentTest = -1;
			}
			GUILayout.EndHorizontal();
			if( currentTest >= 0)
				tests[ currentTest ].OnGUI();
		}
		else
		{
			GUILayout.BeginHorizontal();
			if( GUILayout.Button("Do Previous Test" ,GUILayout.Width(200) , GUILayout.Height( 50 )) )
			{
				currentTest-- ;
				if(currentTest>=0) 
					tests[ currentTest ].DoTest();
				else 
					currentTest = tests.Count-1 ;
			}
			if( GUILayout.Button("Do Next Test" ,GUILayout.Width(200) , GUILayout.Height( 50 )) )
			{
				currentTest++ ;
				if(currentTest<tests.Count) 
					tests[ currentTest ].DoTest();
				else 
					currentTest = -1 ;
			}
			GUILayout.EndHorizontal();
			tests[ currentTest ].OnGUI();
		}

	}

}
