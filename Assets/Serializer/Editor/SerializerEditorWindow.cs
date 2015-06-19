using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using cloudsoft;
using System.Linq;


public class SerializerEditorWindow : EditorWindow {

	public static SerializerEditorWindow Instance { get; private set; }
	public static bool IsOpen {
		get { return Instance != null; }
	}
	void OnEnable() {
		Instance = this;
	}

	SerializersEditor editor = new SerializersEditor();


	[MenuItem ("aliessmael/Serialize")]
	static void Init () {
		SerializerEditorWindow window = (SerializerEditorWindow)EditorWindow.GetWindow (typeof (SerializerEditorWindow));
		window.name = "Serializers";
		window.title = "Serializers";
		window.Initialize();
	}
	bool initialized = false;
	public void Initialize()
	{
		editor.Clear();
		editor.window = this;
		editor.LoadAllTypes();

		/*for( int i = 0 ; i < SerializerSystem.serializers.Count ; i++ )
		{
			Serializer s = SerializerSystem.serializers[i];


			int index = editor.allTypes.FindIndex( x=>x.Type == s.SerializerOf );
			s.tHolder = editor.allTypes[index]  ;
			foreach( SerializerType sub in s.SerializerOf.SubTypes )
			{
				index = editor.allTypes.FindIndex( x=>x.Type == sub.Type );
				editor.allTypes[index] = sub ;
			}


		}*/
		SerializerSystem.Init();
		//finish replace reference then init to get proper reference of field types
		for( int i = 0 ; i < SerializerSystem.serializers.Count ; i++ )
		{
			Serializer s = SerializerSystem.serializers[i];
			editor.InitSerializer( s );
		}
		initialized = true;
	}



	void OnGUI () 
	{
		if( initialized )
			editor.OnGUI();
	}

	void Update()
	{
		editor.Update();
	}

}