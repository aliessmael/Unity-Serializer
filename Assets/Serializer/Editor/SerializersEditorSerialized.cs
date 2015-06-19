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
	string searchText ;
	string _searchText = "";

	List<Serializer> toRemove = new List<Serializer>();
	
	#if UNITY_EDITOR
	public EditorWindow window;
	#endif

	bool Changed = false;
	public void Clear()
	{
		toRemove.Clear();
	}
	public void AddNewSerializer( TypeHolder serType )
	{

		if( SerializerSystem.GetSerializerOf( ref serType.Type ) != null )
			return;
		if( SerializerSystem.serializers.Find( x=>x.SerializerOf == serType.Type ) != null )
			return;

		Changed = true;

		Serializer t = new Serializer();
		t.IsNew = true;

		t.SerializerOf = serType.Type ;
		//t.tHolder = serType;
		SerializerSystem.AddSerializer( t );

		InitSerializer( t );
		t.SetFieldsSerializers();
		//t.SetSubclassSerializer();
	}
	public void RemoveSerializer( Serializer ser )
	{
		if( ser.ManualChange )
		{
		}
		else if( ser.IsNew )
		{
			SerializerSystem.RemoveSerializer( ser );
			ser.tHolder.Serializer = null;

		}
		else 
		{
			toRemove.Add( ser);
		}
	}

	public void UntoggleAllUsingThis( Type type )
	{

		
		for( int i = 0 ; i < SerializerSystem.serializers.Count ; i++ )
		{
			Serializer rt = SerializerSystem.serializers[i];
			for( int j = 0 ; j < rt.Fields.Count ; j++ )
			{
				if( !rt.Fields[j].Toggled)
					continue;
				SerializerField member = rt.Fields[j];
				foreach( TypeHolder h in member.Types )
				{
					if( h.Type.IsSubclassOf( type) || h.Type == type )
					{
						rt.UntoggleField( member );
						break;
					}
				}
			}
		}
	}

	public void AddAllInheritedFrom( TypeHolder h )
	{
	
		for( int i = 0 ; i < h.SubTypes.Count ; i++ )
		{
			AddNewSerializer( h.SubTypes[i] );
		}
	}


	int getLineIndex( object obj )
	{
		int index = 0;
		//bool found = false;
		for( int i = 0 ; i < SerializerSystem.serializers.Count ; i++ )
		{
			if( obj == SerializerSystem.serializers[i] )
				return index ;
			index ++;
			if( SerializerSystem.serializers[i].foldout )
			{
				for( int j = 0 ; j < SerializerSystem.serializers[i].Fields.Count ; j++ )
				{
					if( SerializerSystem.serializers[i].Fields[j] == obj )
						return index;
					index++;
				}

			}
		}

		return -1;
	}

	object getObjectAtLine( int line )
	{
		int _line = 0;
		for( int i = 0 ; i < SerializerSystem.serializers.Count ; i++ )
		{
			if( _line == line )
				return SerializerSystem.serializers[i] ;
			_line ++;
			if( SerializerSystem.serializers[i].foldout )
			{
				for( int j = 0 ; j < SerializerSystem.serializers[i].Fields.Count ; j++ )
				{
					if( _line == line )
						return SerializerSystem.serializers[i].Fields[j];
					_line++;
				}
			}
		}
		
		return null;
	}
	List<object> selected = new List<object>();

	void calculateSelectedFromSerialized( Vector2 mousePos,bool rightClick, bool ctrl , bool shift )
	{
		int minIndex = int.MaxValue;
		int maxIndex = int.MinValue;
		if( shift )
		{
			for( int i = 0 ; i < selected.Count ; i ++ )
			{
				int lineIndex = getLineIndex( selected[i] );
				if( lineIndex < minIndex ) 
					minIndex = lineIndex ;
				if( lineIndex > maxIndex )
					maxIndex = lineIndex ;
			}
		}
		if( ctrl == false && rightClick == false && shift == false)
			selected.Clear();
		float index = 0;
		if( scrollPosition.y > 0 )
			index = (mousePos.y-firstItemPos.y+ scrollPosition.y) / Size.y ;
		else 
			index = (mousePos.y-firstItemPos.y+ scrollPosition.y-Size.y) / Size.y ;


		if( index < 0 )
			return;
		index = Mathf.Floor( index);
		int prevLinesCount = 0 ;
		for( int i = 0 ; i < SerializerSystem.serializers.Count ; i++ )
		{
			Serializer rt = SerializerSystem.serializers[i] ;
			int linesCount = rt.VisibleLinesCount ;
			if( index == prevLinesCount )
			{
				if( !selected.Contains( rt ))
				{
					if( rightClick )
					{
						selected.Clear();
					}
					if( shift )
					{
						if( index < minIndex )
						{
							for( int s = (int)index ; s < minIndex ; s++ )
							{
								object obj = getObjectAtLine( s );
								if( !selected.Contains( obj ))
									selected.Add( obj );
							}
						}
						else if( index > maxIndex )
						{
							for( int s = maxIndex ; s < index ; s++ )
							{
								object obj = getObjectAtLine( s );
								if( !selected.Contains( obj ))
									selected.Add( obj );
							}
						}
					}
					selected.Add( rt);
				}
				else 
				{
					if( ctrl )
					{
						selected.Remove( rt);
					}
				}
				break;
			}
			if( index < linesCount + prevLinesCount )
			{
				int memIndex = (int)index - prevLinesCount-1 ;
				if( memIndex < rt.Fields.Count )
				{
					SerializerField member = rt.Fields[ memIndex ] ;
					if( !selected.Contains( member ))
					{
						if( rightClick )
						{
							selected.Clear();
						}
						if( shift )
						{
							if( index < minIndex )
							{
								for( int s = (int)index ; s < minIndex ; s++ )
								{
									object obj = getObjectAtLine( s );
									if( !selected.Contains( obj ))
										selected.Add( obj );
								}
							}
							else if( index > maxIndex )
							{
								for( int s = maxIndex ; s < index ; s++ )
								{
									object obj = getObjectAtLine( s );
									if( !selected.Contains( obj ))
										selected.Add( obj );
								}
							}
						}
						selected.Add( member );
					}
					else 
					{
						if( ctrl )
						{
							selected.Remove( member );
						}
					}
				}
				break;
			}
			prevLinesCount += linesCount ;
		}
	}
	//initialize serializer editor data
	public void InitSerializer( Serializer t )
	{


		Type type = t.SerializerOf;
		//t.codePath = AssetDatabase.GetAssetPath( script );
		if( type == null )
		{
			Debug.LogError( "Can not find type " + t.tHolder.TypeStr );
			return;
		}

		t.tHolder = allTypes.Find( x=>x.Type == type );
		t.tHolder.Serializer = t;
		claculateSubclass( t.tHolder ); 
		if( !t.ManualChange )
		{
		
			for( int i = 0 ; i < t.tHolder.SubTypes.Count ; i++ )
			{
				if( t.tHolder.SubTypes[i].Serializer == null )//add only subtype wich is serialized, otherwise System.Object will have huge subtypes
					continue;

				if( t.Subtypes.Find( x=> x.Type == t.tHolder.SubTypes[i].Type) != null )
					continue;

				SubtypeSerializer subType = new SubtypeSerializer();
				subType.Type = t.tHolder.SubTypes[i].Type ;
				t.LastSubTypeIndex ++;
				subType.Id = t.LastSubTypeIndex ;
				subType.IsNew = true;
				t.Subtypes.Add( subType );
			}
		}


		for( int i = 0 ; i < t.Fields.Count ; i ++ )
		{
			string name = t.Fields[i].Name ;
			FieldInfo field =  t.SerializerOf.GetField( name );
			PropertyInfo property =  t.SerializerOf.GetProperty( name );
			if( field != null )
			{
				t.Fields[i].isPublicGet =  field.IsPublic;
				t.Fields[i].isPublicSet =  field.IsPublic;
				Type protoType = SerializersUtil.GetType("ProtoBuf.ProtoMemberAttribute");
				if( protoType != null )
					t.Fields[i].hasProtoMember = field.GetCustomAttributes(protoType,true).Length > 0 ;
			}
			else if( property != null )
			{
				MethodInfo getter = property.GetGetMethod();
				MethodInfo setter = property.GetSetMethod();
				t.Fields[i].isPublicGet =  getter != null && getter.IsPublic ;
				t.Fields[i].isPublicSet =  setter != null && setter.IsPublic;
				Type protoType = SerializersUtil.GetType("ProtoBuf.ProtoMemberAttribute");
				if( protoType != null )
					t.Fields[i].hasProtoMember = property.GetCustomAttributes( protoType,true).Length > 0 ;
			}
			else if( !t.Fields[i].IsNew && t.Fields[i].Toggled)
			{
				t.Fields[i].IsDeleted = true;
			}

			t.Fields[i].claculateTypes( allTypes );
			for( int j = 0 ; j < t.Fields[i].Types.Count ; j ++ )
			{
				if( t.Fields[i].Types[j].SubclassIsCalculated == false )
					claculateSubclass( t.Fields[i].Types[j] );
			}
		}





		if( t.SerializerOf.IsEnum )
		{
			string[] names = Enum.GetNames( t.SerializerOf );
			//Array    values = Enum.GetValues( t.SerializerOf.Type );
			for( int j = 0 ; j < names.Length ; j++ )
			{
				if( t.HasField( t.SerializerOf, names[j] ))
					continue;

				SerializerField member = new SerializerField();
				member.IsNew = true;


				member.Owner = t ;
				member.Type = t.SerializerOf ;
				member.Name = names[j];

				member.isPublicGet = true;
				member.isPublicSet = true;
				member.claculateTypes( allTypes );
				for( int i = 0 ; i < member.Types.Count ; i ++ )
				{
					if( member.Types[i].SubclassIsCalculated == false )
						claculateSubclass( member.Types[i] );
				}
				t.Fields.Add( member);

				if( t.IsNew || !member.IsNew )
					t.ToggleField( member);
				else if( !member.IsNew )
					t.ToggleField( member);

				t.Fields.Sort((a,b) => 
				            { 
					return a.Id.CompareTo(b.Id);
				} );
			}

		}
		else 
		{
			FieldInfo[] fields = t.SerializerOf.GetFields( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic  | BindingFlags.FlattenHierarchy ) ;
			for( int j = 0 ; j < fields.Length ; j++ )
			{

				System.NonSerializedAttribute[] att2 = (NonSerializedAttribute[])fields[j].GetCustomAttributes(typeof( NonSerializedAttribute ), true) ;
				if ( att2.Length > 0) {
					continue;
				}
				if( fields[j].IsInitOnly )
					continue;

				//if(  fields[j].FieldType.IsAbstract || !SerializersUtil.HasPublicDefaultConstructor(fields[j].FieldType))
				//	continue;
				if( t.HasField( fields[j].FieldType, fields[j].Name ))
					continue;

				SerializerField member = new SerializerField();
				member.IsNew = true;

				member.Owner = t ;
				member.field = fields[j];
				member.Type = fields[j].FieldType;
				member.Name = fields[j].Name;

				member.isPublicGet =  fields[j].IsPublic;
				member.isPublicSet =  fields[j].IsPublic;
				//member.hasProtoMember = fields[j].GetCustomAttributes(typeof(ProtoBuf.ProtoMemberAttribute),true).Length > 0 ;
				Type protoType = SerializersUtil.GetType("ProtoBuf.ProtoMemberAttribute");
				if( protoType != null )
					member.hasProtoMember =  fields[j].GetCustomAttributes(protoType,true).Length > 0 ;
				member.claculateTypes( allTypes );
				for( int i = 0 ; i < member.Types.Count ; i ++ )
				{
					if( member.Types[i].SubclassIsCalculated == false )
						claculateSubclass( member.Types[i] );
				}
				//member.IsSerialized = t.IsMemberSerialized( member );

				t.Fields.Add( member);

				if( member.isPublic )
				{
					if( t.IsNew || !member.IsNew )
						t.ToggleField( member);
				}
				else if( !member.IsNew )
				{
					t.ToggleField( member);
				}
				t.Fields.Sort((a,b) => 
				            { 
					return a.Id.CompareTo(b.Id);
				} );
			}
			
			PropertyInfo[] properties = t.SerializerOf.GetProperties( BindingFlags.Instance |  BindingFlags.Public| BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			for( int j = 0 ; j < properties.Length ; j++ )
			{

				if( !properties[j].CanRead || !properties[j].CanWrite)
					continue;
				
				if( properties[j].GetIndexParameters().Length > 0 )
					continue;
				
				object[] attr = properties[j].GetCustomAttributes (typeof(ObsoleteAttribute), true);
				if(attr.Length > 0)
					continue;

				if( t.HasField( properties[j].PropertyType, properties[j].Name ))
					continue;

				//if(  properties[j].PropertyType.IsAbstract || !SerializersUtil.HasPublicDefaultConstructor(properties[j].PropertyType))
				//	continue;

				SerializerField member = new SerializerField();
				member.IsNew = true;

				member.Owner = t ;
				member.property = properties[j];
				member.Type = properties[j].PropertyType;
				member.Name = properties[j].Name;


				MethodInfo getter = properties[j].GetGetMethod();
				MethodInfo setter = properties[j].GetSetMethod();
				member.isPublicGet =  getter != null && getter.IsPublic ;
				member.isPublicSet =  setter != null && setter.IsPublic;
				//member.hasProtoMember = properties[j].GetCustomAttributes(typeof(ProtoBuf.ProtoMemberAttribute),true).Length > 0 ;
				Type protoType = SerializersUtil.GetType("ProtoBuf.ProtoMemberAttribute");
				if( protoType != null )
					member.hasProtoMember =  properties[j].GetCustomAttributes(protoType,true).Length > 0 ;
				member.claculateTypes( allTypes );
				for( int i = 0 ; i < member.Types.Count ; i ++ )
				{
					if( member.Types[i].SubclassIsCalculated == false )
						claculateSubclass( member.Types[i] );
				}
				t.Fields.Add( member);

				if( member.isPublic )
				{
					if( t.IsNew || !member.IsNew )
						t.ToggleField( member);
				}
				else if( !member.IsNew )
				{
					t.ToggleField( member);
				}
				t.Fields.Sort((a,b) => 
				            { 
					return a.Id.CompareTo(b.Id);
				} );
			}
		}
		//t.FillDeletedMember();
		
		if( !SerializerSystem.serializers.Contains( t ))
			SerializerSystem.AddSerializer( t );

	}

	public void Update()
	{

		if( Changed )
		{
			for( int i = 0 ; i < SerializerSystem.serializers.Count ; i++ )
			{
				SerializerSystem.serializers[i].updateLoop = true;
			}
			Changed = false;
		}
	}
	

	void UntoggleAllUsingThis (object obj) {
		#if UNITY_EDITOR
		//if( EditorUtility.DisplayDialog("Confirm","Are you sure you want to remove ???" ,"OK", "Cancel" ) )
		{
			List<object> list = (List<object>)obj;
			for( int i = 0 ; i < list.Count ; i++ )
			{
				if( list[i] is Serializer )
				{
					Serializer t = (Serializer) list[i];
					UntoggleAllUsingThis( t.SerializerOf );

				}
				else if( list[i] is SerializerField )
				{
					SerializerField m = (SerializerField) list[i];
					UntoggleAllUsingThis( m.Type );
				}
			}

			AssetDatabase.Refresh();
		}
		#endif
	}
	void UntoggleAllUsingBase (object obj) {
		List<object> list = (List<object>)obj;
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
				Serializer t = (Serializer) list[i];
				UntoggleAllUsingThis( t.SerializerOf.BaseType );
			}
		}
	}
	void DeleteSerializer (object obj)
	{
		List<object> list = (List<object>)obj;
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
				Serializer t = (Serializer) list[i];
				RemoveSerializer( t );
			}
		}
	}

	void GenerateCode (object obj) {
		List<Serializer> toGenerate = new List<Serializer>();
		List<object> list = (List<object>)obj;
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
				toGenerate.Add( (Serializer)list[i] );
			}
		}
		generateCode( toGenerate );
	}


	void AddAllTypesInheriteThis (object obj) {
		List<object> list = (List<object>)obj;
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
				Serializer t = (Serializer) list[i];
				AddAllInheritedFrom( t.tHolder );
			}
			else 
			{
				SerializerField m = (SerializerField) list[i];
				foreach( TypeHolder h in m.Types )
					AddAllInheritedFrom( h );
			}
		}
	}
	void FindSerializerCode(object obj)
	{
		List<object> list = (List<object>)obj;
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
				Serializer t = list[i] as Serializer ;
				if(!string.IsNullOrEmpty( t.CodePath ))
				{
					var code = AssetDatabase.LoadAssetAtPath( t.CodePath , typeof(System.Object));
					//Selection.objects = new UnityEngine.Object[]{code};
					EditorGUIUtility.PingObject( code);
				}
			}
			/*else 
			{
				SerializerField m = (SerializerField) list[i];
				foreach( TypeHolder h in m.Types )
				{

				}
				
			}*/
		}
	}
	void AddThisType (object obj) {
		List<object> list = (List<object>)obj;
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
			}
			else 
			{
				SerializerField m = (SerializerField) list[i];
				foreach( TypeHolder h in m.Types )
				{
					AddNewSerializer( h );
				}

			}
		}
	}

	void FindThisType (object obj) {
		List<object> list = (List<object>)obj;
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
			}
			else 
			{
				//SerializerField m = (SerializerField) list[i];
				/*foreach( SerializerType h in m.Types )
				{
				}*/
				
				//_searchText = t.ToString().Replace("+",".").ToLower();
			}
		}

	}

	void AddAllMisingMemberTypes (object obj) {
		List<object> list = (List<object>)obj;
		List<object> members = new List<object>();
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
				Serializer t = (Serializer) list[i];
				foreach( SerializerField member in t.Fields )
				{
					if( member.Toggled )
						members.Add( member );
				}
			}
			else 
			{
				
			}
		}
		
		AddThisType(members);
	}

	void ToggleAll (object obj) {
		List<object> list = (List<object>)obj;
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
				/*Serializer ser = (Serializer)list[i];
				foreach( SerializerField f in ser.Fields )
					ser.ToggleField( f );*/
			}
			else 
			{
				SerializerField m = (SerializerField) list[i];
				m.Owner.ToggleField( m);
			}
		}

	}

	void UnToggleAll (object obj) {
		List<object> list = (List<object>)obj;
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
				/*Serializer ser = (Serializer)list[i];
				foreach( SerializerField f in ser.Fields )
					ser.UntoggleField( f );*/
			}
			else 
			{
				SerializerField m = (SerializerField) list[i];
				m.Owner.UntoggleField( m);
			}
		}
		
	}
	void ToggleProtoMembersOnly (object obj) {
		List<object> list = (List<object>)obj;
		for( int i = 0 ; i < list.Count ; i++ )
		{
			if( list[i] is Serializer )
			{
				Serializer t = (Serializer) list[i];
				for( int m = 0 ; m < t.Fields.Count ; m++ )
				{
					SerializerField member = t.Fields[m];
					if( member.hasProtoMember )
					{
						t.ToggleField( member);
					}
					else 
					{
						t.UntoggleField( member);
					}
				}
				t.Fields.Sort((a,b) => 
				            { 
					return a.Id.CompareTo(b.Id);
				} );
			}
			else 
			{
				SerializerField m = (SerializerField) list[i];
				if( m.hasProtoMember )
				{
					m.Owner.ToggleField( m);
				}
				else 
				{
					m.Owner.UntoggleField( m);
				}
			}
		}
		
	}


	Vector2 scrollPosition;
	Rect serializedRect;
	Rect buttonsRect;
	Rect tempRect;
	int startIndex;
	int totalVisibleCount ;
	List<object> contextObject;
	int linesCount = 0;
	Rect firstItemPos ;
	#if UNITY_EDITOR
	string labelColorStr = "";
	string blueColorStr  = "";
	string greenColorStr  = "";
	string redColorStr  = "";
	string orangeColorStr  = "";
	string yelloColorStr  = "";
	string colorEnd  = "</color>";
	public void DrawSerializerdTypes()
	{
		if( string.IsNullOrEmpty( labelColorStr))
		{
			Color labelColor = EditorStyles.label.normal.textColor;
			labelColorStr = "<color=#" + EditorStyles.label.normal.textColor.ToHex() + ">";
			blueColorStr  = "";
			greenColorStr  = "";
			redColorStr  = "";
			orangeColorStr  = "";
			yelloColorStr  = "";
			if( labelColor.r > 0.5f )
			{
				blueColorStr = "<color=#8888ff>";
				greenColorStr = "<color=#88ff88>";
				redColorStr = "<color=#ff8888>";
				orangeColorStr= "<color=#ff8888>";
				yelloColorStr= "<color=#ffff88>";
			}
			else 
			{
				blueColorStr = "<color=#0000ff>";
				greenColorStr = "<color=#006600>";
				redColorStr = "<color=#ff0000>";
				orangeColorStr= "<color=#ff8800>";
				yelloColorStr= "<color=#555500>";
			}
		}

		linesCount = 0;
		int i = 0;
		for( i = 0 ; i < SerializerSystem.serializers.Count ; i++ )
			linesCount += SerializerSystem.serializers[i].VisibleLinesCount ;
		//Debug.Log("Event.current.type = " + Event.current.type );

		serializedRect.x = 0f ;
		serializedRect.y = 0f ;
		serializedRect.width = Screen.width / 2f;
		serializedRect.height = Screen.height*0.8f  ;






		
		GUILayout.BeginArea(serializedRect);
		
		/*if( isDirty() )
			GUILayout.Label ("Serialized types*", EditorStyles.boldLabel,GUILayout.Height(Size.y));
		else */
			GUILayout.Label ("Serialized types", EditorStyles.boldLabel,GUILayout.Height(Size.y));

		GUILayout.BeginHorizontal();

		searchText = EditorGUILayout.TextField ("Search", searchText);


		if( GUILayout.Button("Search" , GUILayout.Width( Size.x * 4)))
		{
			if( !string.IsNullOrEmpty(searchText))
				_searchText = searchText.ToLower();
			else 
				_searchText = "";
		}
		GUILayout.EndHorizontal();

		scrollPosition = EditorGUILayout.BeginScrollView( scrollPosition );

		startIndex = (int)(scrollPosition.y /Size.y)   ;
		totalVisibleCount = (int)(serializedRect.height  / Size.y)  ;
		int totalLinesCount = 0;

		for( i = 0 ;  i < SerializerSystem.serializers.Count ; i++ )
		{
			Serializer t = SerializerSystem.serializers[i];
			totalLinesCount += 1;
			if( totalLinesCount - startIndex> totalVisibleCount )
				break;
			if( t.foldout )
			{
				
				for( int j = 0 ; j < t.Fields.Count ; j++ ){
					totalLinesCount+=1;
				}

			}
		}
		if( totalLinesCount - startIndex  < totalVisibleCount )
		{
			startIndex -= ( totalVisibleCount-(totalLinesCount - startIndex) );
			if( startIndex < 0 )
				startIndex = 0;
		}
		int beforeHeight = (int)(startIndex*Size.y) ;
		if( beforeHeight > 0 )
		{
			GUILayout.BeginHorizontal(  GUILayout.Height( beforeHeight ) );
			GUILayout.Box(" ",NonStyle);
			GUILayout.EndHorizontal();
		}
		else 
		{
			GUILayout.BeginHorizontal(  ZeroStyle );
			GUILayout.Box(" ",ZeroStyle);
			GUILayout.EndHorizontal();
		}


		totalLinesCount = 0;
		for( i = 0 ;  i < SerializerSystem.serializers.Count ; i++ )
		{
			totalLinesCount += 1;
			if( totalLinesCount - startIndex> totalVisibleCount )
				break;



			//List<SerializerType> looped = null; 
			Serializer t = SerializerSystem.serializers[i];
			if( totalLinesCount > startIndex )
			{

				string color = labelColorStr;
				if( t.ManualChange )
					color = yelloColorStr;
				else if( t.hasMemberWithMissingType )
					color = redColorStr;
				string typeStr = "";


				if( toRemove.Contains(t))
					typeStr += redColorStr + "to delete" + colorEnd; 
				typeStr +=  color + t.tHolder.TypeStr.Replace("+",".") + colorEnd;
				if( t.SerializerOf.BaseType!=null && t.SerializerOf.BaseType != null && t.SerializerOf.BaseType != typeof(System.Object) && t.SerializerOf.BaseType != typeof(System.ValueType))
					typeStr += blueColorStr + " : " + t.SerializerOf.BaseType.ToString() + colorEnd;


				if( selected.Contains( t ) )
				{
					GUILayout.BeginHorizontal( SelectedStyle);
				}
				else if( !string.IsNullOrEmpty(_searchText) && typeStr.ToLower().Contains(_searchText))
				{
					GUILayout.BeginHorizontal( BorderStyle);
				}
				else 
				{
					GUILayout.BeginHorizontal( NonStyle);
				}

				string str = typeStr;
				if( t.IsNew )
					str = "*" + typeStr ;

				GUIStyle typeStyle = CollapsedStyle ;
				if( t.Fields.Count == 0 )
					typeStyle = EmptyBoxStyle ;
				else if( t.foldout )
					typeStyle = ExpandedStyle ;

				GUILayout.Space( 0f );//to make same as field draw to remove unity gui errors
				if( GUILayout.Button("",typeStyle))
				{
					t.foldout = !t.foldout;
				}
				if( GUILayout.Button(str,NonStyle))
				{

				}
				tempRect = GUILayoutUtility.GetLastRect();
				tempRect.x = serializedRect.x + serializedRect.width - Size.x * 2 ;
				tempRect.width = Size.x ;
				tempRect.height = Size.y ;
				//GUILayout.FlexibleSpace();

				if(  t.IsLooping  )
				{
					if( GUI.Button(tempRect, "",LoopedStyle ))
					{
					}
				}
				else
				{
					if( GUI.Button(tempRect, "",NonStyle ))
					{
					}
				}

				GUILayout.EndHorizontal();
			}

			if( !t.foldout && !string.IsNullOrEmpty(_searchText))
			{
				for( int m = 0 ; m < t.Fields.Count ; m++ )
				{
					if( t.Fields[m].Name.ToLower().Contains(_searchText) || t.Fields[m].Type.ToString().ToLower().Contains(_searchText))
					{
						t.foldout = true;
						break;
					}
				}
			}

			if( t.foldout )
			{

				for( int j = 0 ; j < t.Fields.Count ; j++ )
				{

					SerializerField member = t.Fields[j];

					totalLinesCount += 1;
					if( totalLinesCount - startIndex > totalVisibleCount )
						break;
					if( totalLinesCount > startIndex )
					{
						string memberStr = " ";
						if( member.Id != int.MaxValue )
							memberStr += yelloColorStr + " " + member.Id + " </color>";
						if( member.IsDeleted )
							memberStr+= redColorStr +  "deleted </color>" ;
						if( !member.isPublic )
						{
							memberStr += orangeColorStr;
						}
						else 
						{
							memberStr += greenColorStr;
						}
						if( member.IsNew )
							memberStr += "*" + member.Name;
						else
							memberStr += member.Name ;

						memberStr += colorEnd;
						string color = blueColorStr ;
						if( member.Toggled && member.isMemberTypeMissing )
							color = ":" + redColorStr;
						//if( member.Type.IsEnum )
						//memberStr += color + " " + member.Index.ToString()+ colorEnd;
						memberStr += color + member.Type.ToString()+ colorEnd;



						if( selected.Contains(member) )
						{
							GUILayout.BeginHorizontal( SelectedStyle );
						}
						else if( !string.IsNullOrEmpty(_searchText) && memberStr.ToLower().Contains(_searchText))
						{
							GUILayout.BeginHorizontal( BorderStyle);
						}
						else 
						{
							GUILayout.BeginHorizontal(  NonStyle );
						}

						GUILayout.Space( Size.x );




						GUIStyle toggleStyle = CorrectStyle ;
						if( !member.Toggled )
							toggleStyle = WrongStyle;
						if( GUILayout.Button("",toggleStyle))
						{
							if( member.Toggled )
								t.UntoggleField( member);
							else 
								t.ToggleField( member);

							Changed = true;
						}
						if( GUILayout.Button(memberStr,NonStyle))
						{

						}
						tempRect = GUILayoutUtility.GetLastRect();
						tempRect.x = serializedRect.x + serializedRect.width - Size.x * 2 ;
						tempRect.width = Size.x ;
						tempRect.height = Size.y ;
						//GUILayout.FlexibleSpacSize.x ;e();

						if( member.Toggled && member.isLooped )
						{
							if( GUI.Button(tempRect,"",LoopedStyle ))
							{
							}
						}
						else
						{
							if( GUI.Button(tempRect,"",NonStyle ))
							{
							}
						}

						GUILayout.EndHorizontal();
					}
				}

			}
		}

		int afterHeight = (int)((linesCount - totalLinesCount )*Size.y) ;
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
			GUILayout.Box(" ",ZeroStyle);
			GUILayout.EndHorizontal();
		}

		EditorGUILayout.EndScrollView();

		Rect _rect = GUILayoutUtility.GetLastRect();
		if( _rect.height > 1 )
			firstItemPos = _rect;

		GUILayout.EndArea();
		
		
		
		buttonsRect.x = 0f ;
		buttonsRect.y = Screen.height - Size.y * 3;
		buttonsRect.width = Screen.width * 0.5f;
		buttonsRect.height = Size.y * 2 ;
		GUILayout.BeginArea(buttonsRect);
		GUILayout.BeginHorizontal();

		if( GUILayout.Button("expand all"))
		{
			for( i = 0 ; i < SerializerSystem.serializers.Count ; i++ )
			{
				SerializerSystem.serializers[i].foldout = true;
			}
		}
		if( GUILayout.Button("collapse all"))
		{
			_searchText = "";
			for( i = 0 ; i < SerializerSystem.serializers.Count ; i++ )
			{
				SerializerSystem.serializers[i].foldout = false;
			}
		}

		GUILayout.FlexibleSpace();
		if( GUILayout.Button("Save"))
		{
			generateCode();
		}
		GUILayout.Space( Size.x );
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

		if( Event.current.type == EventType.Repaint )
		{
			if( contextObject != null && contextObject.Count > 0 )
			{
				GenericMenu menu = new GenericMenu();
				menu.AddItem( new GUIContent("Find Serializer Code"),false,FindSerializerCode,contextObject);
				menu.AddItem( new GUIContent("Add This Type"),false,AddThisType,contextObject);
				menu.AddItem( new GUIContent("Add All Subclass of this"),false,AddAllTypesInheriteThis,contextObject);
				menu.AddItem( new GUIContent("Add All Missing member types"),false,AddAllMisingMemberTypes,contextObject);
				menu.AddItem( new GUIContent("Toggle All"),false,ToggleAll,contextObject);
				menu.AddItem( new GUIContent("UnToggle All"),false,UnToggleAll,contextObject);
				menu.AddItem( new GUIContent("Toggle ProtoMembers only"),false,ToggleProtoMembersOnly,contextObject);
				menu.AddItem( new GUIContent("Find this type"),false,FindThisType,contextObject);
				menu.AddItem( new GUIContent("Untoggle all fields using this type"),false,UntoggleAllUsingThis,contextObject);
				menu.AddItem( new GUIContent("Untoggle all fields using base Type"),false,UntoggleAllUsingBase,contextObject);
				menu.AddItem( new GUIContent("Delete Serializer"),false,DeleteSerializer,contextObject);
				menu.AddItem( new GUIContent("Generate Code"),false,GenerateCode,contextObject);


				menu.ShowAsContext();
				
				contextObject = null;
				window.Repaint();
			}
		}
	}
	#endif

	/*bool isDirty()
	{
		for( int i = 0 ; i < serialized.Count ; i ++ )
		{
			if( serialized[i].isDirty )
				return true;
		}
		return false;
	}*/

	void generateCode()
	{
		for( int i = 0 ; i < toRemove.Count ; i++ )
		{
			SerializerSystem.RemoveSerializer( toRemove[i]);
		}
		toRemove.Clear();
		generateCode( SerializerSystem.serializers );
	}



	void generateCode( List<Serializer> toGenerate )
	{
		#if UNITY_EDITOR
		if( toGenerate == null || toGenerate.Count == 0 )
		{
			Debug.LogError("No types to generate");
			return;
		}

		//validate
		/*List<string> errors = new List<string>();
		for( int i = 0 ; i < toGenerate.Count ; i++ )
		{
			Serializer rt = toGenerate[i] ;
			if( rt.IsFrozen )
				continue;

			for( int j = 0 ; j < toGenerate[i].allMembers.Count ; j ++ )
			{
				SerializerField member = toGenerate[i].allMembers[j];
				if( !member.toggled )
					continue;
				Type t = member.type ;
				Serializer s =  SerializerSystem.GetSerializerOf( ref t );
				if( s == null )
				{
					if( serialized.Find( x=> x.type.type == member.type ) != null )
					{

					}
					else 
					{
						errors.Add( "Error in " + toGenerate[i].type + " in " + member.name + " Can not find serializer for " + member.type );
					}
				}
			}

			//check for infinite loop

			if( toGenerate[i].Loop.Count > 0 )
			{
				string loop_path = "";
				for( int l = 0 ; l < toGenerate[i].Loop.Count ; l++ )
					loop_path +=  toGenerate[i].Loop[l].typeStr +  "->" ;
				errors.Add( "Error in " + toGenerate[i].type.type + " type is looped ->" + loop_path );
			}
		}

		if( errors.Count > 0 )
		{
			for( int i = 0 ; i < errors.Count ; i++ )
				Debug.LogError( errors[i] );

			if( EditorUtility.DisplayDialog("Confirm","There is errors, do you want to continue any way?" ,"OK", "Cancel" ) )
			{
			}
			else
			{
				return;
			}
		}*/

		string path = EditorPrefs.GetString("serializer_code_path" , "");
		path = EditorUtility.SaveFolderPanel("Code Generate" , path , "" );
		if( !string.IsNullOrEmpty(path))
			EditorPrefs.SetString("serializer_code_path" , path);
		

		
		//ushort typeId = (ushort)(SerializerSystem.GetLastTypeId() + 1);
		for( int i = 0 ; i < toGenerate.Count ; i++ )
		{
			Serializer rt = toGenerate[i];
			if( rt.ManualChange )
				continue;

			GenerateCode( rt, path );

		}
		
		AssetDatabase.Refresh();
		#endif
	}

	public void GenerateCode( Serializer ser, string path )
	{
		//bool IdIsUsed = false;
		//if( !t.isDirty) 
		//	return;
		
		/*if( ser.IsNew )
		{
			ser.Id = typeId++;
			IdIsUsed = true;
		}*/

		
		EditorTypeSerializerGenerator generator = new EditorTypeSerializerGenerator( ser);
		generator.WriteCode(path);
		//return IdIsUsed;
	}

}
