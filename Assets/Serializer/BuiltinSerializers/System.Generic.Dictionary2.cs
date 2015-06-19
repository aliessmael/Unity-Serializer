using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;
using UnityEngine;

namespace cloudsoft
{
	public class System_Generic_Dictionary2_Serializer :Serializer{

		public System_Generic_Dictionary2_Serializer()
		{
			SerializerOf = typeof(Dictionary<,>);
			ManualChange = true;
			CanHasSubtype = false;
		}
		


		public override int Write( SerializerStream stream, object obj )
		{
			return stream.WriteDictionary( (IDictionary)obj);
		}

		public override object Read( SerializerStream stream, Type type )
		{
			return stream.ReadDictionary( type );
		}
	}

	public partial class SerializerStream
	{
		int writeDefaultsBytes(IDictionary dic, Serializer ser , out BitArray isDefault , out bool hasSubtype)
		{
			int dataSize = 0;
			uint count = (uint)dic.Count ;
			isDefault = null;
			bool canHaveSubtype = ser.Subtypes.Count > 0 ;
			hasSubtype = false;
			int i = 0 ;
			foreach( var key in dic.Keys )
			{
				object value = dic[ key] ;
				
				if( SerializerSystem.IsDefaultValue( ser, ref value ) )
				{
					if( isDefault == null )
						isDefault = new BitArray(dic.Keys.Count,false);
					isDefault[i] = true;
				}
				else if( canHaveSubtype && !hasSubtype  )
				{
					hasSubtype = ( value.GetType() != ser.SerializerOf );
				}
				i++;
			}
			if( isDefault != null )
			{
				byte[] isDefaultBytes = new byte[(int)(count/8)+1];
				isDefault.CopyTo(isDefaultBytes,0);
				dataSize += WriteBytes( isDefaultBytes);
			}
			else 
			{
				dataSize += WriteSystem_UInt32(  (uint)0 );
			}
			return dataSize;
		}

		public int WriteDictionary(  IDictionary value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == null )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
				dataSize += WriteSlotInfo(slot);

			IDictionary dic = (IDictionary)value;
			if( dic == null || dic.Count == 0 ){
				dataSize += WriteSystem_UInt32(  (uint)0);
			}
			else{
				uint count = (uint)dic.Count ;
				dataSize += WriteSystem_UInt32(  count );
				Type type = value.GetType();
				Type[] types = type.GetGenericArguments();
				Type elementType1 = types[0] ;
				Type elementType2 = types[1] ;
				Serializer ser1 = SerializerSystem.GetSerializerOf(ref elementType1 );
				Serializer ser2 = SerializerSystem.GetSerializerOf(ref elementType2 );
				BitArray isDefault = null ;
				bool hasSubtype;
				dataSize += writeDefaultsBytes(dic,ser2,out isDefault, out hasSubtype );
				dataSize += WriteSystem_Boolean( hasSubtype );
				int i = 0 ;
				foreach( var key in dic.Keys )
				{
					dataSize += SerializerSystem.Write(this,key, ser1);
					object _value = dic[ key] ;
					//dataSize += SerializerSystem.WriteBool( writer,value ==null );
					if( isDefault ==null || isDefault[i] == false)
					{
						type = _value.GetType();

						if( hasSubtype )
						{
							if( type == ser2.SerializerOf )
							{
								WriteSystem_UInt32( 0 );
								dataSize += SerializerSystem.Write(this,_value, ser2);
							}
							else 
							{
								SubtypeSerializer ss = ser2.GetSubtype( ref type );
								WriteSystem_UInt32( ss.Id );
								dataSize += SerializerSystem.Write(this,_value, ss.Ser);
							}
						}
						else
						{
							dataSize += SerializerSystem.Write(this,_value, ser2);
						}
					}
					i++;
				}
			}
			return dataSize ;
		}

		public IDictionary ReadDictionary(  System.Type type )
		{
			uint count = ReadSystem_UInt32( );
			
			IDictionary  listObj = (IDictionary)Activator.CreateInstance( type );
			Type[] types = type.GetGenericArguments();
			Type elementType1 = types[0] ;
			Type elementType2 = types[1] ;
			Serializer ser1 = SerializerSystem.GetSerializerOf( ref elementType1);
			Serializer ser2 = SerializerSystem.GetSerializerOf( ref elementType2);
			IDictionary dic = (IDictionary)listObj ;
			if( count > 0 )
			{
				BitArray isDefault = null;
				byte[] isDefaultBytes = ReadBytes();
				if( isDefaultBytes != null )
					isDefault = new BitArray( isDefaultBytes );
				
				bool hasSubtype = ReadSystem_Boolean();
				for( int i = 0 ; i < count ; i++ )
				{
					object key = SerializerSystem.Read(this,elementType1,ser1 );
					//bool isNull = SerializerSystem.ReadBool(reader);
					if( isDefault != null && isDefault[i] == true )
					{
						dic[ key ] = ser2.DefaultValue ;
						continue;
					}
					uint typeId = 0;
					if( hasSubtype )
					{
						typeId = ReadSystem_UInt32();
						if( typeId == 0 )
						{
							object _value = SerializerSystem.Read(this,elementType2,ser2);
							dic[ key ] = _value ;
						}
						else 
						{
							SubtypeSerializer ss = ser2.GetSubtype( typeId);
							object _value = SerializerSystem.Read(this,elementType2,ss.Ser);
							dic[ key ] = _value ;
						}
					}
					else 
					{
						object _value = SerializerSystem.Read(this,elementType2,ser2);
						dic[ key ] = _value ;
					}
				}
			}
			return listObj ;
		}
	}
}

