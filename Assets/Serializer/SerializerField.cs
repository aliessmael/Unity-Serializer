using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace cloudsoft
{
	public class SerializerField
	{
		public Type   Type ;
		public string Name ;
		public int    Id = int.MaxValue;
		public int    IndexInArray;
		//public int 	  BytesWritten;
		public object Value;

		public bool   IsNew = false ;
		public bool   IsDeleted = false;
		public bool   IsBoolean;


		//used in editor
		public SerializerField()
		{
		}
		public SerializerField( Type type, string name, int value = 0)
		{

			Type 	= type;
			Name 	= name;
			Id 	= value;
			IsBoolean = (type == typeof(bool));
			Toggled = true;
			isPublicSet = true;
			isPublicGet = true;
		}



		//for editor

		public bool 				Toggled;
		public Serializer 			Owner;
		public FieldInfo 			field;
		public PropertyInfo 		property;


		//used in editor
		public List<TypeHolder> Types = new List<TypeHolder>();
		public void claculateTypes( List<TypeHolder> allTypes )
		{

			Type t = null;
			int index = -1;

			if( Type.IsArray )
			{
				t = Type.GetElementType();
				index =  allTypes.FindIndex( x=>x.Type == t);
				if( index == -1 )
				{
					//Debug.LogError("Can not find type " + t );
				}
				else 
				{
					Types.Add( allTypes[index] );
				}
			}
			else if( Type.IsGenericType )
			{
				Type[] args =  Type.GetGenericArguments();
				foreach( Type arg in args )
				{
					index =  allTypes.FindIndex( x=>x.Type == arg);
					if( index == -1 )
					{
						//Debug.LogError("Can not find type " + arg );
					}
					else 
					{
						Types.Add( allTypes[index] );
					}
				}
			}
			else 
			{
				index = allTypes.FindIndex( x=>x.Type == Type) ;
				if( index == -1 )
				{
					//Debug.LogError("Can not find type " + Type );
				}
				else 
				{
					Types.Add( allTypes[index] );
				}
			}


		}

		public bool isPublic{
			get{
				return isPublicSet && isPublicGet ;
			}
		}
		public bool isPublicSet;
		public bool isPublicGet;
		public bool hasProtoMember ;

		
		public bool isMemberTypeMissing{
			get{
				
				for( int i = 0 ; i < Types.Count ; i ++ )
				{
					TypeHolder h = Types[i];
					
					if( h.Serializer == null )
						return true;

					if( h.Serializer.CanHasSubtype )
					{
						for( int j = 0 ; j < h.SubTypes.Count ; j++ )
						{
							if( h.SubTypes[j].Serializer == null )
								return true;
						}
					}
				}
				
				return false;
			}
		}
		
		public bool isLooped
		{
			get{
				if( !Owner.IsLooping )
					return false;
				
				foreach( TypeHolder h in Types )
				{
					if( h.Type == typeof( object ) || h.Type == typeof( ValueType ))
						continue;
					if( Owner.Loop.Find( x=> x.Type == h.Type || x.Type.IsSubclassOf( h.Type )) != null )
						return true;
				}
				return false ;
			}
		}
	}


	public partial class SerializerStream
	{
		public int WriteSlotInfo( int slot,  uint subTypeId = 0 )
		{
			int writeType = (subTypeId > 0)? 1: 0;
			uint num = (uint) (((slot) << 1) | writeType );
			int size = WriteSystem_UInt32(num);
			
			if( writeType == 1 )
			{
				size += WriteSystem_UInt32(subTypeId);
			}
			return size;
		}
		
		public void ReadSlotInfo(out int slot , out uint typeId)
		{
			uint data = ReadSystem_UInt32();
			if( data == 0 )
			{
				slot = 0 ;
				typeId = 0;
				return;
			}
			
			int readType = (int)(data  & 0x1 );
			slot = (int)(data >> 1) ;
			
			if( readType == 1 )
			{
				typeId = ReadSystem_UInt32();
			}
			else 
			{
				typeId = 0;
			}
		}
	}
}
