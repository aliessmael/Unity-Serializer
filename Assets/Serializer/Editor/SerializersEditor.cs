using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using cloudsoft;
using System.Linq;
using System.IO;

public partial class SerializersEditor 
{

	public void OnGUI()
	{
		if( Event.current.type == EventType.MouseDown )
		{
			Vector2 mousePos = Event.current.mousePosition ;
			bool rightClick = Event.current.button == 1 ;
			bool ctrl = Event.current.control ;
			Rect rect1 = serializedRect ;
			rect1.width -= Size.x;
			Rect rect2 = allTypeRect ;
			rect2.width -= Size.x;
			if( rect1.Contains( mousePos))
			{
				calculateSelectedFromSerialized( mousePos, rightClick, ctrl, Event.current.shift );
			}
			else if( rect2.Contains(mousePos))
			{
				calculateSelectedFromAllTypes( mousePos, rightClick, ctrl, Event.current.shift );
			}
			if ( rightClick )
			{
				Event.current.Use();
				contextObject = selected ;
				window.Repaint();
			}
		}
		
		DrawInspector();
		DrawSerializerdTypes();
		DrawAllTypes();
	}
}
