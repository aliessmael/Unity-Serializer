using UnityEngine;
using System.Text;


using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.CodeDom;
using cloudsoft;
using System.Reflection;
using System.IO;
using System.CodeDom.Compiler;
using System.Linq;

public class EditorTypeSerializerGenerator 
{

	StringBuilder code = new StringBuilder();
	int tabCount = 0;
	string doShift()
	{
		string tabs = "";
		for( int i = 0 ; i < tabCount ; i++ )
			tabs += "\t" ;

		return tabs;
	}

	void addCodeLine( string line )
	{
		code.Append( doShift() + line + "\n");
	}
	Serializer ser;
	Type type;
	string typeName;
	public EditorTypeSerializerGenerator( Serializer ser  )
	{
		type = ser.SerializerOf;
		this.ser = ser;

		typeName = ser.SerializerOf.ToString().Replace(".","_").Replace("+","_") ;

		addCodeLine("/*");
		addCodeLine("* Generated from " + SystemInfo.deviceName + " at " + System.DateTime.Now );
		addCodeLine("* */");
		addCodeLine("");
		addCodeLine("namespace cloudsoft");
		addCodeLine("{");
		tabCount++;
		addCodeLine("using System.IO;");
		addCodeLine("using System.Collections;");
		addCodeLine("using System.Collections.Generic;");


		addSerializerClass();
		addCodeLine("");

		//if( !type.IsAbstract )
			addExtenderClass();

		tabCount--;
		addCodeLine("}");

	}

	void addSerializerConstructor()
	{


		addCodeLine("public " + typeName + "_Serializer()");
		addCodeLine("{");
		tabCount++;
		addCodeLine("SerializerOf = typeof(" + type.ToString().Replace("+",".") + ");");
		for( int i = 0 ; i < ser.Subtypes.Count ; i++ )
		{
			uint id = ser.Subtypes[i].Id ;
			addCodeLine("AddSubType(typeof(" + ser.Subtypes[i].Type.ToString().Replace("+",".") + "), " + id + "u);");
		}
		
		addCodeLine("");
		for( int i = 0 ; i < ser.Fields.Count ; i++ )
		{
			
			SerializerField member = ser.Fields[i];
			if( member.Toggled )
			{
				Type fieldType = member.Type ;
				string fieldTypeStr = fieldType.ToString().Replace("+",".");
				if( fieldType.IsArray )
					fieldTypeStr = fieldType.GetElementType().ToString().Replace("+",".") + "[]" ;
				else if( fieldType.IsGenericType && (fieldType.GetGenericTypeDefinition() == typeof(List<>)))
					fieldTypeStr = "List<" + fieldType.GetGenericArguments()[0].ToString().Replace("+",".") + ">";
				else if( fieldType.IsGenericType && (fieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
					fieldTypeStr = "Dictionary<" + fieldType.GetGenericArguments()[0].ToString().Replace("+",".") + "," + fieldType.GetGenericArguments()[1].ToString().Replace("+",".") + ">";
				string name = member.Name ;
				if( ser.SerializerOf.IsEnum )
					addCodeLine("AddEnumField(\"" + name + "\", " + member.Id + ");");
				else 
					addCodeLine("AddField(typeof(" + fieldTypeStr + "), \"" + name + "\", " + member.Id + ");");
			}
		}
		tabCount--;
		addCodeLine("}");
	}

	void addSerializerMethods()
	{

		if( !ser.SerializerOf.IsAbstract )
		{
			addCodeLine("public override int Write( SerializerStream stream, object value )");
			addCodeLine("{");
			tabCount++;
			addCodeLine("return stream.Write" + typeName + "( (" + type.ToString().Replace("+",".") + ")value);");
			
			tabCount--;
			addCodeLine("}");
			addCodeLine("");
			
			addCodeLine("public override object Read( SerializerStream stream, System.Type type )");
			addCodeLine("{");
			tabCount++;
			addCodeLine("return stream.Read" + typeName + "();");
			
			tabCount--;
			addCodeLine("}");
			addCodeLine("");
		}
	}
	void addSerializerClass()
	{
		addCodeLine("public class " + typeName + "_Serializer : Serializer");
		addCodeLine("{");
		tabCount++;
		addDefaultValue();
		addSerializerConstructor();
		addCodeLine("");
		addSerializerMethods();
		tabCount--;
		addCodeLine("}");
	}

	string getWriteString( SerializerField f )
	{
		Type t = f.Type;
		string name = f.Name ;
		string writeString = "Write" + t.ToString().Replace("+","_").Replace(".","_")  + "(value." + name + ", true, "+  f.Id +");";
		if( t.IsArray )
			writeString = "WriteArray( value." + name + ", true, "+  f.Id +");";
		else if( t.IsGenericType && ( t.GetGenericTypeDefinition() == typeof(List<>) ))
			writeString = "WriteList( value." + name + ", true, "+  f.Id +");";
		else if( t.IsGenericType && ( t.GetGenericTypeDefinition() == typeof(Dictionary<,>) ))
			writeString = "WriteDictionary( value." + name + ", true, "+  f.Id +" );";
		
		return writeString ;
	}
	void addExtenderWrite()
	{


		addCodeLine("public int Write" + typeName + "( " + type.ToString().Replace("+",".") + " value, bool dontWriteDefault = false, int slot = -1)");
		addCodeLine("{");
		tabCount++;

		SerializerField f = new SerializerField( type , "value" );
		string def = getDefaultExpression( f );
		addCodeLine("if ( dontWriteDefault && " + def + ")");
		tabCount++;
		addCodeLine("return 0;");
		tabCount--;

		addCodeLine("");
		addCodeLine("int dataSize = 0;");
		addCodeLine("");
		//Serializer ser = SerializerSystem.GetSerializerOf( ref type );



		if( !type.IsAbstract )
		{
			if( ser.CanHasSubtype && ser.Subtypes.Count > 0 )
			{
				addCodeLine("if( value.GetType() == typeof( " + type.ToString().Replace("+",".") + " ))");
				addCodeLine("{");
				tabCount++;
			}

			addCodeLine("if( slot != -1 )");
			addCodeLine("{");
			tabCount++;
			addCodeLine("dataSize += WriteSlotInfo( slot );");
			tabCount--;
			addCodeLine("}");
			//addCodeLine("WriteSlotInfo(slot,0);");
			for( int i = 0 ; i < ser.Fields.Count ; i++ )
			{
				SerializerField field = ser.Fields[i];
				
				if( !field.Toggled )
					continue;
				
				if( field.IsDeleted )
					continue;
				
				addCodeLine("dataSize += " + getWriteString( field ) );
				
			}

			addCodeLine("dataSize += WriteSystem_UInt32(0u);");
			addCodeLine("return dataSize;");
			if( ser.CanHasSubtype && ser.Subtypes.Count > 0 )
			{
				tabCount--;
				addCodeLine("}");
			}
		}
		
		if( ser.Subtypes.Count > 0 )
		{
		
			for( int j = 0 ; j < ser.Subtypes.Count ; j++ )
			{
				SubtypeSerializer sub = ser.Subtypes[j];
				if( !sub.Type.IsAbstract)
				{
					addCodeLine( sub.Type.ToString().Replace("+",".") + " sub" + j + " = value as " + sub.Type.ToString().Replace("+",".") + ";");
					addCodeLine( "if (sub" + j +" != null)");
					addCodeLine("{");
					tabCount++;
					addCodeLine( "dataSize += WriteSlotInfo( slot, " +  sub.Id + " );");
					addCodeLine( "dataSize += Write" + sub.Type.ToString().Replace("+","_").Replace(".","_") + "(sub" + j + ");");
					//addCodeLine( "goto FinishSub ;");
					addCodeLine("return dataSize;");
					tabCount--;
					addCodeLine("}");
					
				}
			}

		}

		if( ser.CanHasSubtype && ser.Subtypes.Count > 0 )
			addCodeLine("throw new System.Exception(\"Can not find serializer for \" + value.GetType());");
		tabCount--;
		addCodeLine("}");
	}
	void addExtenderWriteEnum()
	{
		
		
		addCodeLine("public int Write" + typeName + "( " + type.ToString().Replace("+",".") + " value, bool dontWriteDefault = false, int slot = -1)");
		addCodeLine("{");
		tabCount++;
		
		SerializerField f = new SerializerField( type , "value" );
		string def = getDefaultExpression( f );
		addCodeLine("if ( dontWriteDefault && " + def + ")");
		tabCount++;
		addCodeLine("return 0;");
		tabCount--;
		
		addCodeLine("");
		addCodeLine("int dataSize = 0;");
		addCodeLine("");
		Serializer ser = SerializerSystem.GetSerializerOf( ref type );
		
		
		addCodeLine("if( slot != -1 )");
		addCodeLine("{");
		tabCount++;
		addCodeLine("dataSize += WriteSlotInfo( slot );");
		tabCount--;
		addCodeLine("}");


		for( int i = 0 ; i < ser.Fields.Count ; i++ )
		{
			SerializerField field = ser.Fields[i];
			
			if( !field.Toggled )
				continue;
			
			if( field.IsDeleted )
				continue;
			
			addCodeLine("if ((value == " + type.ToString().Replace("+",".")+ "." +field.Name + " ))");
			addCodeLine("{");
			tabCount++;
			addCodeLine("dataSize += WriteSystem_UInt32(" + field.Id + ");");
			addCodeLine("return dataSize;");
			tabCount--;
			addCodeLine("}");

			
		}

		addCodeLine("throw new System.Exception(\" Can not find index of \" + value );") ;
		
		
		tabCount--;
		addCodeLine("}");
	}

	string getReadString( Type t )
	{
		string readString = "Read" + t.ToString().Replace("+","_").Replace(".","_") + "( typeId );";
		Serializer ser = SerializerSystem.GetSerializerOf( ref t );
		if( ser != null && !ser.CanHasSubtype)
			readString = "Read" + t.ToString().Replace("+","_").Replace(".","_") + "( );";
		if( t.IsArray )
		{
			string ss = t.GetElementType().ToString().Replace("+",".") + "[]";
			readString = "(" + ss + ")ReadArray(typeof(" + ss +") );";
		}
		else if( t.IsGenericType && ( t.GetGenericTypeDefinition() == typeof(List<>) ))
		{
			string ss = "List<" + t.GetGenericArguments()[0].ToString().Replace("+",".") + ">";
			readString = "("+ss+")ReadList(typeof("+ss+") );";
		}
		else if( t.IsGenericType && ( t.GetGenericTypeDefinition() == typeof(Dictionary<,>) ))
		{
			string ss = "Dictionary<" + t.GetGenericArguments()[0].ToString().Replace("+",".") + ", " + t.GetGenericArguments()[1].ToString().Replace("+",".") + ">";
			readString = "("+ss+")ReadDictionary(typeof("+ss+") );";
		}

		return readString ;
	}
	void addExtenderRead()
	{
		addCodeLine("public " + type.ToString().Replace("+",".") + " Read" + typeName + "( uint subType = 0 )");
		addCodeLine("{");
		tabCount++;

		addCodeLine( "if( subType == 0 )");
		addCodeLine("{");
		tabCount++;
		if( type.IsAbstract )
		{
			addCodeLine("throw new System.Exception(\"Can not read abstract class " + type + "\");");
		}
		else 
		{
			addCodeLine( type.ToString().Replace("+",".") + " value = new " + type.ToString().Replace("+",".") + "();");
			addCodeLine( "int fieldId = 1;");
			addCodeLine( "uint typeId = 1;");
			addCodeLine( "while( true )");
			addCodeLine("{");
			tabCount++;
			addCodeLine("ReadSlotInfo(out fieldId, out typeId);");
			addCodeLine("if (fieldId == 0)");
			addCodeLine("{");
			tabCount++;
			addCodeLine("break;");
			tabCount--;
			addCodeLine("}");

			for( int i = 0 ; i < ser.Fields.Count ; i++ )
			{
				SerializerField field = ser.Fields[i];
				
				if( field.IsDeleted )
					continue;
				
				if( !field.Toggled )
					continue;
				
				addCodeLine("if (fieldId == " + field.Id + ")" );
				addCodeLine("{");
				tabCount++;
				
				addCodeLine("value." + field.Name + " = " + getReadString(field.Type) );
				addCodeLine("continue;");
				
				
				tabCount--;
				addCodeLine("}");
			}
		}

		tabCount--;
		addCodeLine("}");
		
		if( !type.IsAbstract )
		{
			addCodeLine( "return value;");
			tabCount--;
			addCodeLine("}");
		}
		

		for( int j = 0 ; j < ser.Subtypes.Count ;  j++ )
		{
			SubtypeSerializer subType = ser.Subtypes[j];
			if( !subType.Type.IsAbstract)
			{
				addCodeLine("if (subType == " + subType.Id + ")" );
				addCodeLine("{");
				tabCount++;
				addCodeLine("return  Read" + subType.Type.ToString().Replace("+","_").Replace(".","_")+ "( 0 );");
				tabCount--;
				addCodeLine("}");
			}
		}
		
		addCodeLine("throw new System.Exception(\"Can not Read" + typeName + "\");");

		tabCount--;
		addCodeLine("}");

	}
	void addExtenderReadEnum()
	{
		addCodeLine("public " + type.ToString().Replace("+",".") + " Read" + typeName + "( uint subType = 0 )");
		addCodeLine("{");
		tabCount++;
		
		addCodeLine( "uint enumId = ReadSystem_UInt32();");

		
		
		for( int i = 0 ; i < ser.Fields.Count ; i++ )
		{
			SerializerField field = ser.Fields[i];
			
			if( field.IsDeleted )
				continue;
			
			if( !field.Toggled )
				continue;
			
			addCodeLine("if (enumId == " + field.Id + ")");
			addCodeLine("{");
			tabCount++;
			addCodeLine("return " + type.ToString().Replace("+",".") + "." + field.Name + ";");
			tabCount--;
			addCodeLine("}");
		}
		
		addCodeLine( "throw new System.Exception(\"Can not find enum of index \" + enumId);");

		tabCount--;
		addCodeLine("}");
		
	}

	void addExtenderClass()
	{
		addCodeLine("public partial class SerializerStream");
		addCodeLine("{");
		tabCount++;

		if( type.IsEnum )
			addExtenderWriteEnum();
		else
			addExtenderWrite();

		addCodeLine("");
		if( type.IsEnum )
			addExtenderReadEnum();
		else
			addExtenderRead();
		addCodeLine("");

		//addIsDefaultValue();

		tabCount--;
		addCodeLine("}");
	}


	string getDefaultExpression( SerializerField f )
	{
		if( f.Type.IsValueType )
		{
			object defaultValue = Activator.CreateInstance( f.Type ) ;
			string defStr = defaultValue.ToString().ToLower();

			if( defStr != "0" && defStr != "false")
			{
				if( f.Type.IsEnum ) 
					return "" + f.Name + " == " + f.Type.ToString().Replace("+",".") + "." + defaultValue.ToString();
				else
					return ""+ f.Name + " == " + f.Type.ToString().Replace("+","_").Replace(".","_") + "_Serializer.defaultValue"  ;
			}
			else 
			{
				return "" + f.Name + " == " + defStr ;
			}
		}
		if( f.Type == typeof(string))
			return "string.IsNullOrEmpty( " + f.Name + ")";
		else
			return "" + f.Name + " == null ";
	}




	void addDefaultValue()
	{
		if( type.IsValueType )
		{
			if( type.IsEnum )
			{
				return;
			}
			else 
			{
				object defaultValue = Activator.CreateInstance( type );
				if( defaultValue.ToString()== "0" || defaultValue.ToString().ToLower() == "false")
					return;
				else 
				{
					addCodeLine("static public " + type.ToString().Replace("+",".") + " defaultValue = new " + type.ToString().Replace("+",".") + "();");
					/*addCodeLine("public static bool IsDefaultValue( this " + type.ToString().Replace("+",".") + " _this)");
					addCodeLine("{");
					tabCount++;
					addCodeLine("return defaultValue == _this;");
					tabCount--;
					addCodeLine("}");*/
				}
			}
		}
	}


	public void WriteCode( string path)
	{
		System.IO.File.WriteAllText( path + "/" + type.ToString().Replace("+",".")+ ".cs"  , code.ToString() );

	}
}
