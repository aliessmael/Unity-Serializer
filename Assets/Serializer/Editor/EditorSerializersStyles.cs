using UnityEngine;
using System.Collections;
using UnityEditor;


public partial class SerializersEditor 
{
	public Vector2 Size = new Vector2(16f,16f);

	GUIStyle nonStyle ;
	public GUIStyle NonStyle{
		get{
			if( nonStyle == null )
			{
				nonStyle = new GUIStyle(); 
				//nonStyle.fixedWidth = Size.x;
				nonStyle.fixedHeight = Size.y;
			}
			return nonStyle;
		}
	}
	private GUIStyle 		emptyBoxStyle ;
	public  GUIStyle 		EmptyBoxStyle{
		get{
			if( emptyBoxStyle == null )
			{
				emptyBoxStyle = new GUIStyle();
				emptyBoxStyle.fixedWidth = Size.x;
				emptyBoxStyle.fixedHeight = Size.y;

			}
			return emptyBoxStyle ;
		}
	}

	GUIStyle zeroStyle ;
	public GUIStyle ZeroStyle{
		get{
			if( zeroStyle == null )
			{
				zeroStyle = new GUIStyle(); 
				zeroStyle.fixedHeight = 0f;
			}
			return zeroStyle;
		}
	}

	GUIStyle inspectorLabelStyle ;
	public GUIStyle InspectorLabelStyle{
		get{
			if( inspectorLabelStyle == null )
			{
				inspectorLabelStyle = new GUIStyle(); 

			}
			return inspectorLabelStyle;
		}
	}

	GUIStyle   	redFontStyle ;
	public GUIStyle RedFontStyle{
		get{
			if( redFontStyle == null )
			{
				redFontStyle = new GUIStyle( EditorStyles.label );
				redFontStyle.normal.textColor = Color.red;
			}
			return redFontStyle;
		}
	}


	private GUIStyle 		foldStyle ;
	public  GUIStyle 		FoldStyle{
		get{
			if( foldStyle == null )
			{
				foldStyle = new GUIStyle( EditorStyles.foldout );
				foldStyle.fixedHeight = Size.y;
				foldStyle.normal.textColor = Color.black;
				foldStyle.onNormal.textColor = Color.black;
				foldStyle.hover.textColor = Color.black;
				foldStyle.onHover.textColor = Color.black;
				foldStyle.focused.textColor = Color.black;
				foldStyle.onFocused.textColor = Color.black;
				foldStyle.active.textColor = Color.black;
				foldStyle.onActive.textColor = Color.black;
			}
			return foldStyle ;
		}
	}

	private GUIStyle 		correctStyle ;
	public  GUIStyle 		CorrectStyle{
		get{
			if( correctStyle == null )
			{
				correctStyle = new GUIStyle();
				correctStyle.fixedWidth = Size.x;
				correctStyle.fixedHeight = Size.y;
				correctStyle.normal.background = getImage( "correct" );
			}
			return correctStyle ;
		}
	}

	private GUIStyle 		wrongStyle ;
	public  GUIStyle 		WrongStyle{
		get{
			if( wrongStyle == null )
			{
				wrongStyle = new GUIStyle();
				wrongStyle.fixedWidth = Size.x;
				wrongStyle.fixedHeight = Size.y;
				wrongStyle.normal.background = getImage( "wrong" );
			}
			return wrongStyle ;
		}
	}

	private GUIStyle 		collapsedStyle ;
	public  GUIStyle 		CollapsedStyle{
		get{
			if( collapsedStyle == null )
			{
				collapsedStyle = new GUIStyle();
				collapsedStyle.fixedWidth = Size.x;
				collapsedStyle.fixedHeight = Size.y;
				collapsedStyle.normal.background = getImage( "collapsed" );
			}
			return collapsedStyle ;
		}
	}

	private GUIStyle 		expandedStyle ;
	public  GUIStyle 		ExpandedStyle{
		get{
			if( expandedStyle == null )
			{
				expandedStyle = new GUIStyle();
				expandedStyle.fixedWidth = Size.x;
				expandedStyle.fixedHeight = Size.y;
				expandedStyle.normal.background = getImage( "expanded" );
			}
			return expandedStyle ;
		}
	}

	private GUIStyle 		addStyle ;
	public  GUIStyle 		AddStyle{
		get{
			if( addStyle == null )
			{
				addStyle = new GUIStyle();
				addStyle.fixedWidth = Size.x;
				addStyle.fixedHeight = Size.y;
				addStyle.normal.background = getImage( "add" );
			}
			return addStyle ;
		}
	}
	private GUIStyle 		removeStyle ;
	public  GUIStyle 		RemoveStyle{
		get{
			if( removeStyle == null )
			{
				removeStyle = new GUIStyle();
				removeStyle.fixedWidth = Size.x;
				removeStyle.fixedHeight = Size.y;
				removeStyle.normal.background = getImage( "remove" );
			}
			return removeStyle ;
		}
	}

	private GUIStyle 		borderStyle ;
	public  GUIStyle 		BorderStyle{
		get{
			if( borderStyle == null )
			{
				borderStyle = new GUIStyle();
				borderStyle.normal.background = getImage( "border" );
				borderStyle.border = new RectOffset(4, 4, 4, 4);
			}
			return borderStyle ;
		}
	}

	private GUIStyle 		selectedStyle ;
	public  GUIStyle 		SelectedStyle{
		get{
			if( selectedStyle == null )
			{
				selectedStyle = new GUIStyle();
				selectedStyle.normal.background = getImage( "selected" );
				selectedStyle.fixedHeight = Size.y;
			}
			return selectedStyle ;
		}
	}

	private GUIStyle 		loopedStyle ;
	public  GUIStyle 		LoopedStyle{
		get{
			if( loopedStyle == null )
			{
				loopedStyle = new GUIStyle();
				loopedStyle.normal.background = getImage( "loop" );
				loopedStyle.fixedWidth = Size.x;
				loopedStyle.fixedHeight = Size.y;
			}
			return loopedStyle ;
		}
	}

	public Texture2D getImage( string imageFile )
	{
		Texture2D t = (Texture2D)Resources.Load<Texture2D>( imageFile ); 
		if( t != null )
		{
			return t ;
		}
		else 
		{
			Debug.LogError("can not find image " + imageFile );
			return null;
		}
	}
}
