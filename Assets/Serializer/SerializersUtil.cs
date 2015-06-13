using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using cloudsoft;
using System.Linq;
using System.IO;

public static class SerializersUtil 
{
	public static Type GetType( string typeStr )
	{
		var type = System.Type.GetType(typeStr);
		if (type != null) 
			return type;
		foreach (var a in System.AppDomain.CurrentDomain.GetAssemblies())
		{
			type = a.GetType(typeStr);
			if (type != null)
				return type;
		}
		return null ;
	}



	public static bool HasPublicDefaultConstructor( System.Type type )
	{
		if( type.IsEnum )
			return true;

		if( type.IsValueType && !type.IsPrimitive )//is struct
			return true;

		ConstructorInfo[] p = type.GetConstructors();
		return p.FirstOrDefault( x => x.IsPublic && x.GetParameters().Length == 0) != null ;
	}

	public static string GetSerializerPathOf( ref Type t)
	{
		Serializer s = SerializerSystem.GetSerializerOf( ref t );
		if( s == null )
			return null;

		string fileName = t.ToString().Replace("+",".")  + ".cs" ;
		return FindFile( fileName );
	}

	public static string FindFile( string file )
	{
#if UNITY_WEBPLAYER 
		return "";
#else
		string[] files =  Directory.GetFiles( Application.dataPath, file, SearchOption.AllDirectories );
		if( files == null || files.Length == 0 )
		{
			return null;
		}

		return files[0];
#endif
	}
	public static string ToHex( this Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}
	public static string ToHex( this Color color)
	{
		float red = color.r * 255;
		float green = color.g * 255;
		float blue = color.b * 255;
		
		string a = getHex((int)Mathf.Floor(red / 16));
		string b = getHex((int)Mathf.Round(red % 16));
		string c = getHex((int)Mathf.Floor(green / 16));
		string d = getHex((int)Mathf.Round(green % 16));
		string e = getHex((int)Mathf.Floor(blue / 16));
		string f = getHex((int)Mathf.Round(blue % 16));

		string z = a + b + c + d + e + f;
		
		return z;
	}
	static string alpha = "0123456789ABCDEF";
	static  string getHex ( int d) {
		string res = "" + alpha[d];
		return res;
	}
}
