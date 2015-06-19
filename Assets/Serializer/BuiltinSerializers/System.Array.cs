using System;
using System.IO;
using System.Collections;
using cloudsoft ;
using UnityEngine;


namespace cloudsoft
{
	public class System_Array_Serializer : Serializer {

		public System_Array_Serializer()
		{
			SerializerOf = typeof(Array);
			ManualChange = true;
			CanHasSubtype = false;
		}
		
		public override int Write( SerializerStream stream, object value )
		{
			return stream.WriteArray((Array)value);
		}
		public override object Read( SerializerStream stream, Type type )
		{
			return stream.ReadArray( type );
		}
	}

	public partial class SerializerStream
	{
		int writeDefaultsBytes( Array array, Serializer ser , out BitArray isDefault, out bool hasSubtype )
		{
			int dataSize = 0;
			uint count = (uint)array.Length ;
			
			isDefault = null;
			bool canHaveSubtype = ser.Subtypes.Count > 0 ;
			hasSubtype = false;
			for( int i = 0 ; i < count ; i++ )
			{
				object element = array.GetValue(i) ;
				
				if( SerializerSystem.IsDefaultValue( ser, ref element ) )
				{
					if( isDefault == null )
						isDefault = new BitArray((int)count,false);
					isDefault[i] = true;
				}
				else if( canHaveSubtype && !hasSubtype)
				{
					hasSubtype = ( element.GetType() != ser.SerializerOf );
				}
			}
			if( isDefault != null )
			{
				int c = (int)(count/8)+1 ;
				byte[] isDefaultBytes = new byte[c];
				isDefault.CopyTo(isDefaultBytes,0);
				dataSize += WriteBytes( isDefaultBytes,0);
				//Debug.Log("write " + c + " bytes");
			}
			else 
			{
				dataSize += WriteSystem_UInt32( (uint)0 );
				//Debug.Log("write 0  bytes");
			}
			return dataSize ;
		}

		public int WriteArray(   Array value, bool dontWriteDefault = false , int slot=-1)
		{
			if( dontWriteDefault && value == null )
				return 0;

			int dataSize = 0;

			if( slot != -1 )
			{
				dataSize += WriteSlotInfo(slot);
			}

			//Debug.Log("WriteArray start");

			Array array = value ;
			uint count = (uint)array.Length ;
			//Debug.Log("WriteArray count is " + count );
			dataSize += WriteSystem_UInt32(  count );
			if( count > 0 )
			{
				Type elementType = value.GetType().GetElementType();
				Serializer ser = SerializerSystem.GetSerializerOf(ref elementType );
				BitArray isDefault = null;
				bool hasSubtype ;
				dataSize += writeDefaultsBytes(  array, ser, out isDefault, out hasSubtype );
				dataSize += WriteSystem_Boolean( hasSubtype );
				for(int i = 0 ; i < count ; i++ )
				{
					object element = array.GetValue(i) ;
					if( isDefault ==null || !isDefault[i] )
					{


						if( hasSubtype )
						{
							Type type = element.GetType();
							if( type == ser.SerializerOf )
							{
								dataSize += WriteSystem_UInt32( 0);
								dataSize += SerializerSystem.Write( this,element, ser );
							}
							else 
							{
								SubtypeSerializer ss = ser.GetSubtype( ref type );
								dataSize += WriteSystem_UInt32( ss.Id);
								dataSize += SerializerSystem.Write( this,element, ss.Ser );
							}
						}
						else 
						{
							dataSize += SerializerSystem.Write( this,element, ser );
						}
					}
				}
			}
			
			return dataSize;
		}



		public Array ReadArray(  System.Type type )
		{
			//Debug.Log("ReadArray start");
			uint count = ReadSystem_UInt32();
			//Debug.Log("ReadArray count is " + count );
			Type elementType = type.GetElementType() ;
			Serializer ser = SerializerSystem.GetSerializerOf( ref elementType);
			Array arrayObj = Array.CreateInstance( elementType , count );
			Array array = (Array)arrayObj ;
			if( count > 0 )
			{
				BitArray isDefault = null;
				
				
				byte[] isDefaultBytes = ReadBytes();
				if( isDefaultBytes != null )
				{
					isDefault = new BitArray( isDefaultBytes );
					//Debug.Log("isDefaultBytes length is " + isDefaultBytes.Length );
				}
				
				bool hasSubtype = ReadSystem_Boolean();
				
				for( int i = 0 ; i < count ; i++ )
				{
					if( isDefault!= null && isDefault[i] == true )
					{
						array.SetValue( ser.DefaultValue, i);
						continue;
					}
					uint typeId = 0 ;
					if( hasSubtype )
					{
						typeId = ReadSystem_UInt32();
						if( typeId == 0 )
						{
							object v = SerializerSystem.Read(this,elementType, ser );
							array.SetValue( v , i ) ;
						}
						else 
						{
							SubtypeSerializer ss = ser.GetSubtype( typeId );
							object v = SerializerSystem.Read(this,elementType, ss.Ser );
							array.SetValue( v , i ) ;
						}
					}
					else 
					{
						object v = SerializerSystem.Read(this,elementType, ser );
						array.SetValue( v , i ) ;
					}
				}
			}
			return  arrayObj;
		}
	}
}


