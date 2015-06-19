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
	int inspectorStartIndex;
	int inspectorTotalVisibleCount;
	public void DrawInspector()
	{
		//if( selected == null || selected.Count == 0 )
		//	return ;

		Serializer ser = null;
		if( selected != null && selected.Count > 0 )
		{
			if( selected[0] is Serializer )
			{
				ser = selected[0] as Serializer;
			}
			else if( selected[0] is SerializerField)
			{
				SerializerField member = (SerializerField)selected[0];
				ser = member.Types[0].Serializer ;
			}
		}

		//if( ser == null || ser.Subtypes.Count == 0 )
		//	return;

		inspectorRect.x = 0f;
		inspectorRect.y = Screen.height*0.8f ;
		inspectorRect.width = Screen.width ;
		inspectorRect.height = Screen.height *0.2f - Size.y*3;

		GUILayout.BeginArea(inspectorRect);

		inspectorScrollPosition = EditorGUILayout.BeginScrollView( inspectorScrollPosition );
		inspectorStartIndex = (int)(inspectorScrollPosition.y /Size.y)  ;
		inspectorTotalVisibleCount = (int)(inspectorRect.height  / Size.y) ;


		if( ser == null || (ser.Subtypes.Count < inspectorTotalVisibleCount) ) 
			inspectorStartIndex = 0 ;
		else if( inspectorStartIndex > ser.Subtypes.Count - inspectorTotalVisibleCount )
		{
			inspectorStartIndex = ser.Subtypes.Count - inspectorTotalVisibleCount ;
		}
		if( inspectorStartIndex < 0 ) inspectorStartIndex = 0 ;
		
		
		//allTypeSelectedIndex = Mathf.Clamp( allTypeSelectedIndex , -1 , ser.Subtypes.Count -1);
		int beforeHeight = (int)(inspectorStartIndex*Size.y) ;
		if( beforeHeight > 0 )
		{
			GUILayout.BeginHorizontal(  GUILayout.Height( beforeHeight ) );
			GUILayout.Box(" " ,NonStyle );
			GUILayout.EndHorizontal();
		}
		else 
		{
			GUILayout.BeginHorizontal(  NonStyle );
			GUILayout.Box(" " , NonStyle );
			GUILayout.EndHorizontal();
		}

		int index = 0 ;
		int currentVisible = 0 ;
		for( int i = inspectorStartIndex ;  currentVisible  <= inspectorTotalVisibleCount ; i++ )
		{
			string str = "";
			if( ser != null && (i < ser.tHolder.SubTypes.Count) )
			{
				if( ser.tHolder.SubTypes[i].Serializer != null )
					str = greenColorStr + ser.tHolder.SubTypes[i].Type.ToString() + colorEnd;
				else 
					str = redColorStr + ser.tHolder.SubTypes[i].Type.ToString() + colorEnd;
			}
			if( currentVisible >= inspectorTotalVisibleCount )
			{
				break;
			}

			GUILayout.BeginHorizontal();
			GUILayout.Button(str, NonStyle);
			GUILayout.EndHorizontal();
			currentVisible++;
			index++;
		}

		int afterHeight = 0;
		if( ser != null )
			afterHeight = (int)((ser.Subtypes.Count - (inspectorStartIndex + index) )*Size.y) ;
		if( afterHeight > 0 )
		{
			//fill invisible gap after scroller to make proper scroller pos
			GUILayout.BeginHorizontal(  GUILayout.Height(afterHeight ) );
			GUILayout.Box(" ",NonStyle);
			GUILayout.EndHorizontal();
		}
		else
		{
			GUILayout.BeginHorizontal(  ZeroStyle );
			GUILayout.Box(" ",NonStyle);
			GUILayout.EndHorizontal();
		}
		/*string labelStr = "";
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
					labelStr += "<color=#ff0000>Error in " + t.SerializerOf + " type is looped ->" + loop_path + colorEnd ;
					linesCount++;
				}
			}
		}

		GUILayout.Label( labelStr , InspectorLabelStyle , GUILayout.Height(Size.y * linesCount));*/

		EditorGUILayout.EndScrollView();
		GUILayout.EndArea();
	}
}
