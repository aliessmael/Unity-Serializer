
using UnityEngine;
using cloudsoft;


#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public partial class SerializersEditor   
{
	string allTypeSearchText ;
	string allTypeSearchTextLower ;
	public List<TypeHolder> allTypes = new List<TypeHolder>();
	List<TypeHolder> filteredTypes = new List<TypeHolder>();
	public void LoadAllTypes()
	{
		allTypes = new List<TypeHolder>();

		foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (System.Type t in a.GetTypes())
			{
				string typeStr = t.ToString();
				int index = typeStr.IndexOf("[");
				if( index != -1 )
					typeStr = typeStr.Substring(0,index);
				allTypes.Add( new TypeHolder(){ TypeStr =  typeStr,TypeStrLower = typeStr.ToLower(),Type = t } );
			}
		}
		//claculateSubclass();//take long time
		calculatedFiltered();
	}
	object allTypes_getObjectAtLine( int line )
	{
		for( int i = 0 ; i < filteredTypes.Count ; i++ )
		{
			if( i == line )
				return filteredTypes[i] ;

		}
		
		return null;
	}
	int allTypes_getLineIndex( object obj )
	{

		for( int i = 0 ; i < filteredTypes.Count ; i++ )
		{
			if( obj == filteredTypes[i] )
				return i ;
		}
		
		return -1;
	}
	Rect allTypes_firstItemPos ;
	List<object> allTypesSelected = new List<object>();
	void calculateSelectedFromAllTypes( Vector2 mousePos,bool rightClick, bool ctrl , bool shift )
	{
		int minIndex = int.MaxValue;
		int maxIndex = int.MinValue;
		if( shift )
		{
			for( int i = 0 ; i < allTypesSelected.Count ; i ++ )
			{
				int lineIndex = allTypes_getLineIndex( allTypesSelected[i] );
				if( lineIndex < minIndex ) 
					minIndex = lineIndex ;
				if( lineIndex > maxIndex )
					maxIndex = lineIndex ;
			}
		}
		if( ctrl == false && rightClick == false && shift == false)
			allTypesSelected.Clear();
		float index = 0;
		if( allTypeScrollPosition.y > 0 )
			index = (mousePos.y-allTypes_firstItemPos.y+ allTypeScrollPosition.y) / Size.y ;
		else 
			index = (mousePos.y-allTypes_firstItemPos.y+ allTypeScrollPosition.y-Size.y) / Size.y ;
		if( index < 0 )
			return;
		index = Mathf.Floor( index);
		for( int i = 0 ; i < filteredTypes.Count ; i++ )
		{
			TypeHolder rt = filteredTypes[i] ;
			if( index == i )
			{
				if( !allTypesSelected.Contains( rt ))
				{
					if( rightClick )
					{
						allTypesSelected.Clear();
					}
					if( shift )
					{
						if( index < minIndex )
						{
							for( int s = (int)index ; s < minIndex ; s++ )
							{
								object obj = allTypes_getObjectAtLine( s );
								if( !allTypesSelected.Contains( obj ))
									allTypesSelected.Add( obj );
							}
						}
						else if( index > maxIndex )
						{
							for( int s = maxIndex ; s < index ; s++ )
							{
								object obj = allTypes_getObjectAtLine( s );
								if( !allTypesSelected.Contains( obj ))
									allTypesSelected.Add( obj );
							}
						}
					}
					allTypesSelected.Add( rt);
				}
				else 
				{
					if( ctrl )
					{
						allTypesSelected.Remove( rt);
					}
				}
				break;
			}
		}
	}

	public void claculateSubclass( TypeHolder type )
	{


		if( type.SubclassIsCalculated == true )
			return;

		type.SubclassIsCalculated = true; 

		if( type.Type == typeof(object) || type.Type == typeof(ValueType))
			return;

		for( int i = 0 ; i < allTypes.Count ; i++ )
		{
			if( allTypes[i] == type )
				continue;
			if( allTypes[i].Type.IsSubclassOf( type.Type ))
			{
				if( type.Serializer != null )
				{
					if( type.SubTypes.Contains( allTypes[i] ))
						continue;
					//type.Serializer.LastSubTypeIndex ++;
					//allTypes[i].Index = type.Serializer.LastSubTypeIndex ;
				}
				type.SubTypes.Add( allTypes[i] );
				//allTypes[i].BaseType = type;
			}
			/*else if( type.type.IsSubclassOf( allTypes[i].type ))
			{
				allTypes[i].subTypes.Add( type );
			}*/
		}
	}
	void calculatedFiltered()
	{
		filteredTypes.Clear();
		if( string.IsNullOrEmpty( allTypeSearchTextLower ))
		{
			filteredTypes = new List<TypeHolder>( allTypes );
			return;
		}
		for( int i = 0 ; i < allTypes.Count ; i++ )
		{
			if( allTypes[i].TypeStrLower.Contains( allTypeSearchTextLower ))
				filteredTypes.Add( allTypes[i] );
		}
	}
	
	Rect allTypeRect;
	Vector2 allTypeScrollPosition ;
	int allTypesStartIndex;
	int allTypeTotalVisibleCount ;
	int allTypeSelectedIndex;
	#if UNITY_EDITOR
	public void DrawAllTypes()
	{
		allTypeRect.x = Screen.width * 0.5f;
		allTypeRect.y = 0f ;
		allTypeRect.width = Screen.width *0.5f;
		allTypeRect.height = Screen.height * 0.8f ;
		
		GUILayout.BeginArea(allTypeRect);
		
		GUILayout.Label ("Select types you want to be serialized", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal();
		string str = EditorGUILayout.TextField ("Search", allTypeSearchText);
		if( str != allTypeSearchText )
		{
			allTypeSearchText = str ;
			allTypeSearchTextLower = allTypeSearchText.ToLower();
			calculatedFiltered();
		}
		GUILayout.EndHorizontal();
		allTypeScrollPosition = EditorGUILayout.BeginScrollView( allTypeScrollPosition );
		allTypesStartIndex = (int)(allTypeScrollPosition.y /Size.y)  ;
		allTypeTotalVisibleCount = (int)(allTypeRect.height  / Size.y)  ;

		if( allTypes.Count < allTypeTotalVisibleCount ) allTypesStartIndex = 0 ;
		else if( allTypesStartIndex > filteredTypes.Count - allTypeTotalVisibleCount )
		{
			allTypesStartIndex = filteredTypes.Count - allTypeTotalVisibleCount ;
		}
		if( allTypesStartIndex < 0 ) allTypesStartIndex = 0 ;
		
		
		allTypeSelectedIndex = Mathf.Clamp( allTypeSelectedIndex , -1 , filteredTypes.Count -1);
		int beforeHeight = (int)(allTypesStartIndex*Size.y) ;
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
		for( int i = allTypesStartIndex ;  currentVisible  <= allTypeTotalVisibleCount ; i++ )
		{
			if( i >= filteredTypes.Count )
				break;
			if( currentVisible >= allTypeTotalVisibleCount )
			{
				break;
			}

			TypeHolder h = filteredTypes[allTypesStartIndex +index];
			string typeStr = labelColorStr +  h.TypeStr + "</color>";
			if( allTypesSelected.Contains( h ) )
			{
				GUILayout.BeginHorizontal( SelectedStyle);
			}
			else 
			{
				GUILayout.BeginHorizontal();
			}
			if( h.Serializer == null ) 
			{
				if( GUILayout.Button( "" , AddStyle ))
				{
					AddNewSerializer( h );
				}
			}
			else 
			{
				if( GUILayout.Button( "" , RemoveStyle ))
				{
					RemoveSerializer( h.Serializer );
				}
			}
			Type baseType = h.Type.BaseType ;
			if( baseType != null && baseType != typeof(object) && baseType != typeof(ValueType))
				typeStr += blueColorStr + " : " + baseType.ToString() + "</color>" ;
			GUILayout.Button(typeStr, NonStyle);
			//GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			currentVisible++;
			index++;
		}
		int afterHeight = (int)((filteredTypes.Count - (allTypesStartIndex + index) )*Size.y) ;
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
		EditorGUILayout.EndScrollView();
		Rect _rect = GUILayoutUtility.GetLastRect();
		if( _rect.height > 1 )
			allTypes_firstItemPos = _rect;
		GUILayout.EndArea();

	}
	#endif

}
