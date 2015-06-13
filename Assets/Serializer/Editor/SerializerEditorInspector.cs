using UnityEngine;
using System;
using cloudsoft;


#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public partial class SerializersEditor 
{

	Vector2 inspectorScrollPosition;
	Rect inspectorRect;
	public void DrawInspector()
	{
		inspectorRect.x = 0f;
		inspectorRect.y = Screen.height*0.8f ;
		inspectorRect.width = Screen.width ;
		inspectorRect.height = Screen.height *0.2f - Size.y*3;

		GUILayout.BeginArea(inspectorRect);

		inspectorScrollPosition = EditorGUILayout.BeginScrollView( inspectorScrollPosition );

		string labelStr = "";
		int linesCount = 0;
		labelStr +=  "Subclass : \n";
		linesCount++;
		for( int i = 0 ; i < selected.Count ; i++ )
		{
			if( selected[i] is Serializer )
			{
				Serializer t = (Serializer)selected[i];
				if( t.Subtypes.Count > 0 )
				{

					for( int j = 0 ; j < t.Subtypes.Count ; j++ )
					{
						string color = "<color=#ff0000>";
						if( t.Subtypes[j].Ser != null )
							color = "<color=#008800>";

						string str = color ;
						if( t.Subtypes[j].IsNew )
							str += "* ";

						str+= t.Subtypes[j].Id + " " + t.Subtypes[j].Type.ToString().Replace("+",".") + " </color>\n" ;

						labelStr += str ;
						linesCount++;
					}
				}
			}
			else 
			{
				SerializerField member = (SerializerField)selected[i];

				foreach( TypeHolder type in member.Types )
				{
					Serializer t = type.Serializer ;
					if( t!= null )
					{
						for( int j = 0 ; j < t.tHolder.SubTypes.Count ; j++ )
						{
							string color = "<color=#ff0000>";
							if( t.tHolder.SubTypes[j].Serializer != null )
								color = "<color=#008800>";
							labelStr += color + t.tHolder.SubTypes[j].TypeStr + " </color>\n";
							linesCount++;

						}
					}
					else 
					{
						TypeHolder h = allTypes.Find( x=> x.Type == type.Type );

						for( int j = 0 ; j < h.SubTypes.Count ; j++ )
						{

							string color = "<color=#ff0000>";
							if( SerializerSystem.serializers.Find( x=> x.SerializerOf == h.SubTypes[j].Type ) != null )
								color = "<color=#008800>";
							labelStr += color + h.SubTypes[j].TypeStr + " </color>\n";
							linesCount++;
						}

					}
				}
			}
		}

		labelStr += "Errors : \n";
		for( int i = 0 ; i < selected.Count ; i++ )
		{
			if( selected[i] is Serializer )
			{
				Serializer t = (Serializer)selected[i];
				if( t.IsLooping )
				{
					string loop_path = "";
					for( int l = 0 ; l < t.Loop.Count ; l++ )
						loop_path +=  t.Loop[l].TypeStr +  "->" ;
					labelStr += "<color=#ff0000>Error in " + t.SerializerOf + " type is looped ->" + loop_path + "</color>" ;
					linesCount++;
				}
			}
		}

		GUILayout.Label( labelStr , InspectorLabelStyle , GUILayout.Height(Size.y * linesCount));

		EditorGUILayout.EndScrollView();
		GUILayout.EndArea();
	}
}
