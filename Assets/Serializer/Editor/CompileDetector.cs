using UnityEngine;
using UnityEditor ;
using UnityEditor.Callbacks;

[InitializeOnLoad]
public class CompileDetector 
{
	static CompileDetector ()
	{
		EditorApplication.update += Update;
	}
	
	static bool isCompiling = true ; 
	static void Update ()
	{
		if( EditorApplication.isCompiling && !isCompiling )
		{
			Debug.Log( "Compile Started");
			if( SerializerEditorWindow.IsOpen )
			{
				SerializerEditorWindow window = SerializerEditorWindow.Instance;
				window.title = "Compiling...";
				window.Repaint();
			}
		}
		if( !EditorApplication.isCompiling && isCompiling )
		{
			Debug.Log("Compile Finished");
			if( SerializerEditorWindow.IsOpen )
			{
				SerializerEditorWindow window = SerializerEditorWindow.Instance;
				window.title = "Serializers"; 
				window.Initialize();
				window.Repaint();
			}
		}
		
		isCompiling = EditorApplication.isCompiling ;
	}
}
