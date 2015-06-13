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
		addCodeLine("namespace cloudsoft");
		addCodeLine("{");
		tabCount++;
		addCodeLine("using System.IO;");
		addCodeLine("using System.Collections;");
		addCodeLine("using System.Collections.Generic;");


		addSerializerClass();
		addCodeLine("");

		if( !type.IsAbstract )
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

	string getWriteString( Type t, string name )
	{
		string writeString = "Write" + t.ToString().Replace("+","_").Replace(".","_")  + "(value." + name + ");";
		if( t.IsArray )
			writeString = "WriteArray( value." + name + ");";
		else if( t.IsGenericType && ( t.GetGenericTypeDefinition() == typeof(List<>) ))
			writeString = "WriteList( value." + name + ");";
		else if( t.IsGenericType && ( t.GetGenericTypeDefinition() == typeof(Dictionary<,>) ))
			writeString = "WriteDictionary( value." + name + " );";
		
		return writeString ;
	}
	void addExtenderWrite()
	{
		addCodeLine("public int Write" + typeName + "( " + type.ToString().Replace("+",".") + " value)");
		addCodeLine("{");
		tabCount++;
		addCodeLine("int dataSize = 0;");
		for( int i = 0 ; i < ser.Fields.Count ; i++ )
		{
			if( !ser.Fields[i].Toggled )
				continue;

			SerializerField field = ser.Fields[i];

			if( type.IsEnum )
			{
				addCodeLine("if ((value == " + type.ToString().Replace("+",".")+ "." +field.Name + " ))");
				addCodeLine("{");
				tabCount++;
				addCodeLine("dataSize += WriteSystem_UInt32(" + field.Id + ");");
				addCodeLine("return dataSize;");
				tabCount--;
				addCodeLine("}");

			}
			else 
			{
				
				string def = getDefaultExpression( field );
				addCodeLine("if ( " + def + ")");
				addCodeLine("{");
				tabCount++;

				Serializer fSer = SerializerSystem.GetSerializerOf( ref field.Type );

				if( !field.Type.IsAbstract && fSer.Subtypes.Count == 0 )
				{
					addCodeLine("dataSize += WriteFieldInfo(" + field.Id + ");");
					addCodeLine("dataSize += " + getWriteString( field.Type, field.Name) );
				}
				else 
				{
					for( int j = 0 ; j < fSer.Subtypes.Count ; j++ )
					{
						SubtypeSerializer sub = fSer.Subtypes[j];
						if( !sub.Type.IsAbstract)
						{
							addCodeLine( sub.Type.ToString().Replace("+",".") + " sub" + j + " = value." + field.Name + " as " + sub.Type.ToString().Replace("+",".") + ";");
							addCodeLine( "if (sub" + j +" != null)");
							addCodeLine("{");
							tabCount++;
							addCodeLine( "dataSize += WriteFieldInfo( " + field.Id + ", " + sub.Id + " );");
							addCodeLine( "dataSize += Write" + sub.Type.ToString().Replace("+","_").Replace(".","_") + "(sub" + j + ");");
							addCodeLine( "goto FinishSub" + field.Id + ";");
							tabCount--;
							addCodeLine("}");

						}
					}
					addCodeLine("FinishSub" + field.Id + ":;");
				}
				
				tabCount--;
				addCodeLine("}");
			}

		}



		if( !type.IsEnum )
		{
			addCodeLine("dataSize += WriteSystem_UInt32(0u);");
			addCodeLine("return dataSize;");
		}
		else 
		{
			addCodeLine("throw new System.Exception(\" Can not find index of \" + value );") ;
		}


		tabCount--;
		addCodeLine("}");
	}

	string getReadString( Type t )
	{
		string readString = "Read" + t.ToString().Replace("+","_").Replace(".","_") + "();";
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
		addCodeLine("public " + type.ToString().Replace("+",".") + " Read" + typeName + "( )");
		addCodeLine("{");
		tabCount++;

		if( type.IsEnum )
		{
			addCodeLine( "uint enumId = ReadSystem_UInt32();");
		}
		else 
		{
			addCodeLine( type.ToString().Replace("+",".") + " value = new " + type.ToString().Replace("+",".") + "();");
			addCodeLine( "int fieldId = 1;");
			addCodeLine( "uint typeId = 1;");
			addCodeLine( "for ( ;  ;  )");
			addCodeLine("{");
			tabCount++;
			addCodeLine("ReadFieldInfo(out fieldId, out typeId);");
			addCodeLine("if (fieldId == 0)");
			addCodeLine("{");
			tabCount++;
			addCodeLine("break;");
			tabCount--;
			addCodeLine("}");
		}


		for( int i = 0 ; i < ser.Fields.Count ; i++ )
		{
			SerializerField field = ser.Fields[i];
			if( !field.Toggled )
				continue;

			if( type.IsEnum )
			{
				addCodeLine("if (enumId == " + field.Id + ")");
				addCodeLine("{");
				tabCount++;
				addCodeLine("return " + type.ToString().Replace("+",".") + "." + field.Name + ";");
				tabCount--;
				addCodeLine("}");
			}
			else 
			{
				addCodeLine("if (fieldId == " + field.Id + ") // " + field.Name);
				addCodeLine("{");
				tabCount++;

				if( !field.Type.IsAbstract )
				{
					addCodeLine("if (typeId == 0)");
					addCodeLine("{");
					tabCount++;

					addCodeLine("value." + field.Name + " = " + getReadString(field.Type) );
					addCodeLine("continue;");
					tabCount--;
					addCodeLine("}");
			
				}
				Serializer fSer = SerializerSystem.GetSerializerOf( ref field.Type );

				for( int j = 0 ; j < fSer.Subtypes.Count ;  j++ )
				{
					SubtypeSerializer subType = fSer.Subtypes[j];
					if( !subType.Type.IsAbstract)
					{
						addCodeLine("if (typeId == " + subType.Id + ") // " + subType.Type.ToString());
						addCodeLine("{");
						tabCount++;
						addCodeLine("value." + field.Name + " = Read" + subType.Type.ToString().Replace("+","_").Replace(".","_")+ "( );");
						addCodeLine("continue;");
						tabCount--;
						addCodeLine("}");
					}
				}

				tabCount--;
				addCodeLine("}");
			}
		}

		if( type.IsEnum )
		{
			addCodeLine( "throw new System.Exception(\"Can not find enum of index \" + enumId);");
		}
		else 
		{
			tabCount--;
			addCodeLine("}");
			addCodeLine( "return value;");
		}
		tabCount--;
		addCodeLine("}");

	}

	void addExtenderClass()
	{
		addCodeLine("public partial class SerializerStream");
		addCodeLine("{");
		tabCount++;

		addExtenderWrite();
		addCodeLine("");
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
					return "value." + f.Name + " != " + f.Type.ToString().Replace("+",".") + "." + defaultValue.ToString();
				else
					return "value."+ f.Name + " != " + f.Type.ToString().Replace("+","_").Replace(".","_") + "_Serializer.defaultValue"  ;
			}
			else 
			{
				return "value." + f.Name + " != " + defStr ;
			}
		}
		if( f.Type == typeof(string))
			return "!string.IsNullOrEmpty( value." + f.Name + ")";
		else
			return "value." + f.Name + " != null ";
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
