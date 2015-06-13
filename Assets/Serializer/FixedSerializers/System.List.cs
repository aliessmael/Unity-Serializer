using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;
using UnityEngine;

namespace cloudsoft
{
	public class System_List_Serializer : Serializer{

		public System_List_Serializer()
		{
			SerializerOf = typeof(List<>);
			ManualChange = true;
		}



		public override int Write( SerializerStream stream, object obj )
		{
			return stream.WriteList( (IList) obj);
		}
		public override object Read( SerializerStream stream, System.Type type )
		{
			return stream.ReadList( type );
		}
	}

	public partial class SerializerStream
	{

		int writeDefaultsBytes(IList list, Serializer ser , out BitArray isDefault, out bool hasSubtype)
		{
			int dataSize = 0;
			uint count = (uint)list.Count ;
			
			isDefault = null;
			bool canHaveSubtype = ser.Subtypes.Count > 0 ;
			hasSubtype = false;
			for( int i = 0 ; i < count ; i++ )
			{
				object element = list[i] ;
				
				if( SerializerSystem.IsDefaultValue( ser, ref element ) )
				{
					if( isDefault == null )
						isDefault = new BitArray((int)count,false);
					isDefault[i] = true;
				}
				else if( canHaveSubtype && !hasSubtype )
				{
					hasSubtype = ( element.GetType() != ser.SerializerOf );
				}
			}
			if( isDefault != null )
			{
				byte[] isDefaultBytes = new byte[(int)(count/8)+1];
				isDefault.CopyTo(isDefaultBytes,0);
				dataSize += WriteBytes( isDefaultBytes);
			}
			else 
			{
				dataSize += WriteSystem_UInt32( (uint)0 );
			}
			return dataSize ;
		}
		public int WriteList(   IList value)
		{
			int dataSize = 0;
			IList list = value;
			uint count = (uint)list.Count ;
			dataSize += WriteSystem_UInt32( count );
			Type type = value.GetType();
			Type elementType = type.GetGenericArguments()[0] ;
			
			Serializer ser = SerializerSystem.GetSerializerOf(ref elementType );
			if( count > 0 )
			{
				BitArray isDefault = null;
				bool hasSubtype = false;
				dataSize += writeDefaultsBytes(list,ser,out isDefault, out hasSubtype );
				dataSize += WriteSystem_Boolean( hasSubtype );
				
				for(int i = 0 ; i < count ; i++)
				{
					if( isDefault ==null || isDefault[i] == false)
					{
						type = list[i].GetType();

						if( hasSubtype )
						{
							SubtypeSerializer ss = ser.GetSubtype( ref type);
							WriteSystem_UInt32( ss.Id );
							dataSize += SerializerSystem.Write(this, list[i], ss.Ser);
						}
						else 
						{
							dataSize += SerializerSystem.Write(this, list[i], ser);
						}
					}
				}
			}
			return dataSize ;
		}

		public IList ReadList<T>( )
		{
			return ReadList(typeof(T));
		}
		public IList ReadList( System.Type type )
		{
			uint count = ReadSystem_UInt32();
			
			IList  listObj = (IList)Activator.CreateInstance( type );
			Type elementType = type.GetGenericArguments()[0] ;
			
			Serializer ser = SerializerSystem.GetSerializerOf( ref elementType);
			IList list = (IList)listObj ;
			if( count > 0 )
			{
				BitArray isDefault = null;
				byte[] isDefaultBytes = ReadBytes();
				if( isDefaultBytes != null )
					isDefault = new BitArray( isDefaultBytes );
				
				bool hasSubtype = ReadSystem_Boolean();
				
				for( int i = 0 ; i < count ; i++ )
				{
					if( isDefault != null && isDefault[i] == true )
					{
						list.Add( ser.DefaultValue ) ;
						continue;
					}
					
					uint typeId = 0;
					if( hasSubtype )
					{
						typeId = ReadSystem_UInt32();
						SubtypeSerializer ss = ser.GetSubtype( typeId );
						object obj = SerializerSystem.Read( this ,elementType, ss.Ser );
						list.Add( obj) ;
					}
					else 
					{
						object obj = SerializerSystem.Read( this ,elementType, ser);
						list.Add( obj) ;
					}
				}
			}
			return listObj ;
		}
	}
}

