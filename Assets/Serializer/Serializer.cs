using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cloudsoft
{
	public class SubtypeSerializer
	{
		public Type Type;
		public uint Id ;
		public bool IsNew = false;
		public Serializer Ser = null;
	}
	public class Serializer
	{
		public int WrittenCount = 0 ;
		public int BytesWritten = 0 ;
		public float WriteTime = 0f;

		public int FieldsBytesWritten{
			get{
				int bytesCount = 0 ;
				for( int i = 0 ; i < Fields.Count ; i++ )
					bytesCount += Fields[i].BytesWritten ;
				return bytesCount;
			}
		}


		public List<SerializerField> Fields = new List<SerializerField>();
		public bool HasField(Type type, string name )//used in editor
		{
			for( int i = 0 ; i < Fields.Count ; i++ )
			{
				if( Fields[i].Type == type && Fields[i].Name == name )
					return true;
			}
			return false;
		}

		public object DefaultValue;
		public bool IsDefaultValue( ref object obj)
		{
			if( DefaultValue == null )
			{
				return obj == null;
			}
			else 
			{
				return DefaultValue.Equals( obj );
			}
		}

		public Type SerializerOf ;


	

		public List<SubtypeSerializer> Subtypes = new List<SubtypeSerializer>();
		public uint LastSubTypeIndex = 0 ;
		public void AddSubType( Type type, uint id )
		{
			SubtypeSerializer subType = new SubtypeSerializer();
			subType.Type = type;
			subType.Id = id;

			Subtypes.Add( subType );
			if( LastSubTypeIndex < id )
				LastSubTypeIndex = id ;
		}
		public SubtypeSerializer GetSubtype( ref Type t )
		{
			for( int i = 0 ; i < Subtypes.Count ; i++ )
			{
				if( Subtypes[i].IsNew )
				   continue;
				if( Subtypes[i].Type == t )
					return Subtypes[i];
			}
			return null;
		}
		public SubtypeSerializer GetSubtype( uint id )
		{
			for( int i = 0 ; i < Subtypes.Count ; i++ )
			{
				if( Subtypes[i].IsNew )
					continue;
				if( Subtypes[i].Id == id )
					return Subtypes[i];
			}
			return null;
		}

		public  bool ManualChange ;



		public int LastFieldIndex = 0 ;
		public void AddField( Type type, string name, int id)
		{
			var f = new SerializerField( type, name, id);
			f.IndexInArray = Fields.Count ;
			Fields.Add( f );
			if( LastFieldIndex < id )
				LastFieldIndex = id ;
		}
		public void AddEnumField( string name, int id )
		{
			Fields.Add(new SerializerField( SerializerOf, name, id));
			if( LastFieldIndex < id )
				LastFieldIndex = id ;
		}

		public virtual int Write( SerializerStream stream , object obj  )
		{

			return 0 ;
		}

		public virtual object Read( SerializerStream stream , Type type)
		{
			return null;
		}

		public void ResetDebug()
		{
			WrittenCount = 0;
			BytesWritten = 0;
			WriteTime = 0;
			for( int i = 0 ; i < Fields.Count; i++ )
			{
				Fields[i].BytesWritten = 0;
			}
		}

		public void SetFieldsSerializers()
		{
			for( int i = 0 ; i < Fields.Count; i++ )
			{
				Fields[i].Owner = this;
				//Fields[i].Serializer = SerializerSystem.GetSerializerOf( ref Fields[i].Type );
			}
		}
		public void SetSubclassSerializer()
		{
			for( int i = 0 ; i < Subtypes.Count ; i++ )
			{
				Subtypes[i].Ser = SerializerSystem.GetSerializerOf( ref Subtypes[i].Type );
			}
		}


		//for editor

		public TypeHolder tHolder;

		string codePath = null;
		public string CodePath{
			get{
				if( codePath == null )
				{
#if UNITY_EDITOR 
					//fils find does not compile in web player
					codePath = SerializersUtil.GetSerializerPathOf( ref SerializerOf );
					if( !string.IsNullOrEmpty( codePath ))
					{
						int index = codePath.IndexOf("Assets");
						codePath = codePath.Remove( 0,index);
					}
#endif
				}
				return codePath;
			}
		}

		public bool foldout ;



		public bool IsNew = false;
		public SerializerField SerializerGetField( string name , Type type )
		{
			return Fields.Find( x=> x.Name == name && x.Type == type);
		}



		
		public int VisibleLinesCount{
			get{
				if( !foldout )
					return 1 ;
				else 
				{
					return 1 + Fields.Count ;
				}
			}
		}

		public void ToggleField( SerializerField f )
		{
			if( f.Toggled == true )
				return;
			f.Toggled = true;
			if( f.IsNew )
			{
				LastFieldIndex++;
				f.Id = LastFieldIndex; 
			}
			/*Fields.Sort((a,b) => 
			              { 
				return a.Id.CompareTo(b.Id);
			} );*/
		}
		public void UntoggleField( SerializerField f )
		{
			if( f.Toggled == false )
				return;
			f.Toggled = false;
			if( f.IsNew )
			{
				int fIndex = f.Id ;
				f.Id = int.MaxValue;
				LastFieldIndex--;
				for( int i = 0 ; i < Fields.Count ; i++ )
				{
					if( Fields[i].Toggled && Fields[i].Id > fIndex )
						Fields[i].Id--;
				}
			}
			/*Fields.Sort((a,b) => 
			            { 
				return a.Id.CompareTo(b.Id);
			} );*/
		}
		public bool HasNonPublicSerialized{
			get{
				for( int i = 0 ; i < Fields.Count ; i++ )
				{
					if( !Fields[i].Toggled )
						continue;
					if( !Fields[i].isPublic)
						return true;
				}
				return false;
			}
		}
		
		public bool hasMemberWithMissingType{
			get{
				for( int i = 0 ; i < Fields.Count ; i++ )
				{
					if( !Fields[i].Toggled )
						continue;
					if( Fields[i].isMemberTypeMissing )
						return true;
				}
				return false;
			}
		}

		public bool updateLoop = true;
		List<TypeHolder> loop = new List<TypeHolder>();
		public List<TypeHolder> Loop{
			get{
				if( updateLoop )
				{
					List<TypeHolder> depens = new List<TypeHolder>();
					loop =  checkLoop( tHolder, depens  );
					updateLoop = false;
				}
				return loop;
			}
		}
		public bool IsLooping
		{
			get{
				if( updateLoop )
				{
					List<TypeHolder> depens = new List<TypeHolder>();
					loop =  checkLoop( tHolder, depens  );
					updateLoop = false;
				}
				return (loop != null) && (loop.Count > 0) ;
			}
		}
		public List<TypeHolder> checkLoop( TypeHolder h, List<TypeHolder> depends = null )
		{
			if( h.Type.IsEnum )
				return null;
			
			if( depends == null )
				depends = new List<TypeHolder>();
			if( depends.Contains( h ))
			{
				depends.Add( h );
				return depends;
			}
			depends.Add( h );
			

			for( int i = 0 ; i < h.SubTypes.Count ; i ++ )
			{
				
				if( h.SubTypes[i].Serializer == null )
					continue;
				List<TypeHolder> loop =  checkLoop( h.SubTypes[i], new List<TypeHolder>( depends) );
				if( loop != null )
				{
					return loop;
				}
			}
			
			Serializer t = h.Serializer;
			if( t != null )
			{
				
				for( int i = 0 ; i < t.Fields.Count ; i ++ )
				{
					if( !t.Fields[i].Toggled )
						continue;
					
					for( int j = 0 ; j < t.Fields[i].Types.Count ; j++ )
					{
						TypeHolder th = t.Fields[i].Types[j];
						
						List<TypeHolder> loop =  checkLoop( th, new List<TypeHolder>( depends) );
						if( loop != null )
						{
							return loop;
						}
					}
				}
				
			}
			return null;
		}


	}
}
