using System;
using System.IO ;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using cloudsoft;


public class SerializerSystem  {

	static SerializerSystem()
	{
		Init();
	}

	static public List<Serializer> 						serializers 	= null;
	static Dictionary<System.Type, Serializer> 			serializersDictionary 	= null ;



	static public void AddSerializer( Serializer ser )
	{
		serializers.Add( ser );
		serializersDictionary[ ser.SerializerOf ] =  ser ;
	}

	static public void RemoveSerializer( Serializer ser )
	{
		serializers.Remove( ser );
		serializersDictionary[ ser.SerializerOf ] = null;
	}

	static public Serializer GetSerializerOf( ref Type type )
	{
		bool found = false ;
		
		Serializer s ;
		if( type.IsGenericType )
			found = serializersDictionary.TryGetValue( type.GetGenericTypeDefinition() , out s );
		else if( type.IsArray )
			found = serializersDictionary.TryGetValue( typeof(Array) , out s );
		else 
			found = serializersDictionary.TryGetValue( type , out s );
		
		if( found )
		{
			return s ;

		}

		//throw new System.Exception("Can not find Serializer of " + type.ToString());
		return null;
	}


	
	static void loadCurrent(){

		serializers = new List<Serializer>();
		serializersDictionary = new Dictionary<Type, Serializer>();

		foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (Type t in a.GetTypes())
			{
				if( t.IsSubclassOf( typeof(Serializer)))
				{
					Serializer r = (Serializer)Activator.CreateInstance( t );
					if( r.SerializerOf == null )
					{
						UnityEngine.Debug.LogError( "Serializer " + r.GetType().ToString() + " is not serialozer of any type" );
						continue;
					}
					AddSerializer(  r);
				}
			}
			
		}

		foreach(Serializer s in serializers )
		{
			if( s.SerializerOf.IsValueType )
			{
				s.DefaultValue = Activator.CreateInstance( s.SerializerOf );
			}
			else 
			{
				s.DefaultValue = null;
			}

			s.SetFieldsSerializers();
			s.SetSubclassSerializer();
		}
	}
	
	static SerializerStream stream; 
	static public void Init( bool forceUpdate = false)
	{
		if( serializersDictionary == null || forceUpdate )
		{
			loadCurrent();
			stream = new SerializerStream(1024*1204*5);
		}
	}
	

	public static T Deserialize<T>( byte[] data  )
	{
		return (T)Deserialize( data, typeof(T)) ;
	}

	public static object Deserialize( byte[] data , Type type )
	{
		try
		{
			if( data == null )
				return null;

			stream.Clear();
			stream.Set( data );
			Serializer ser = GetSerializerOf(ref type );
			return Read(stream,type,ser);
		}
		catch( System.Exception e )
		{
			Debug.LogException(e);
			return null;
		}
		
	}

	public static byte[] Serialize( object obj )
	{
		try
		{
			stream.Clear();
			if( obj == null )
			{
				return null;
			}
			Type type = obj.GetType();
			Serializer ser = GetSerializerOf(ref type );
			Write( stream , obj, ser);
			return stream.ToArray() ;
		}
		catch( System.Exception e )
		{
			Debug.LogException(e);
			return null;
		}
	}
	

	public static object Read( SerializerStream stream , Type type, Serializer ser )//type here is used for generic type to now exactly what is element type
	{
		if( ser == null )
			throw new System.Exception( "Serializer is null " );


		return ser.Read(stream,type);

	}

	
	//object can not be null
	public static int Write( SerializerStream stream , object obj, Serializer ser  )
	{
	
		if( ser == null )
			throw new System.Exception( "Can not find serializer of " + obj.GetType());
		int length = 0 ;

		length += ser.Write( stream, obj );
		ser.BytesWritten += length ;
		ser.WrittenCount++;
		return length;
	}

	public static bool IsDefaultValue( Serializer ser, ref object obj )
	{
		if( ser == null )
		{
			return obj == null;
		}
		else 
		{
			return ser.IsDefaultValue( ref obj );
		}
	}
}
