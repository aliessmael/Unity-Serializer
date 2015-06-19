using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;


public class Test1 : SerializerTestUnit {

	public Test1()
	{
		Id = 1;
	}
	List<TestClass> testList = new List<TestClass>();
	override public void DoTest()
	{

		Description = "Test 1 : Serialize + Deserialize all built-in type ";
		Succeed = false ;

		testList.Add( new TestClass(){  value = true});
		testList.Add( new TestClass(){  value = false});
		testList.Add( new TestClass(){  value = (byte)0x44});
		testList.Add( new TestClass(){  value = (sbyte)0x44});
		testList.Add( new TestClass(){  value = (char)'a'});
		testList.Add( new TestClass(){  value = (short)-434});
		testList.Add( new TestClass(){  value = (ushort)6677});
		testList.Add( new TestClass(){  value = (int)-9897});
		testList.Add( new TestClass(){  value = (uint)87876});
		testList.Add( new TestClass(){  value = (long)55667});
		testList.Add( new TestClass(){  value = (ulong)77665});
		testList.Add( new TestClass(){  value = (float)565.9947f});
		testList.Add( new TestClass(){  value = (double)455.6644});
		testList.Add( new TestClass(){  value = new decimal(1,2,3,true,0x0f)});
		testList.Add( new TestClass(){  value = ""});
		testList.Add( new TestClass(){  value = "gdgdf dfgg"});
		testList.Add( new TestClass(){  value = this.GetType()});
		testList.Add( new TestClass(){  value = typeof(int)});
		testList.Add( new TestClass(){  value = new DateTime(2015,2,3,5,32,2)});
		testList.Add( new TestClass(){  value = Color.white});
		testList.Add( new TestClass(){  value = Color.black});
		testList.Add( new TestClass(){  value = Vector2.one});
		testList.Add( new TestClass(){  value = Vector2.zero});
		testList.Add( new TestClass(){  value = Vector3.one});
		testList.Add( new TestClass(){  value = Vector3.zero});
		testList.Add( new TestClass(){  value = Vector4.one});
		testList.Add( new TestClass(){  value = Vector4.zero});
		testList.Add( new TestClass(){  value = Quaternion.identity});
		testList.Add( new TestClass(){  value = Quaternion.LookRotation(new Vector3(5f,5f,5f),new Vector3(1f,8f,9f))});
		testList.Add( new TestClass(){  value = Matrix4x4.identity});
		testList.Add( new TestClass(){  value = Matrix4x4.Perspective(0.5f,2f,1f,3f)});
		testList.Add( new TestClass(){  value = null});

		float time = Time.realtimeSinceStartup ;
		for( int i = 0 ; i < testList.Count ; i++ )
		{
			TestClass t = testList[i];
			t.d = SerializerSystem.Serialize( t.value );

			if( t.d != null )
				DataSize += t.d.Length ;

		}
		SerializeTime = (Time.realtimeSinceStartup-time);
		time = Time.realtimeSinceStartup ;
		for( int i = 0 ; i < testList.Count ; i++ )
		{
			TestClass t = testList[i];
			object dValue = null;
			if( t.value == null )
				dValue = SerializerSystem.Deserialize( t.d , typeof(object) );
			else 
				dValue = SerializerSystem.Deserialize( t.d , t.value.GetType() );

			if( dValue == null && t.value == null )
				continue;

			if( dValue == null && t.value != null )
			{
				Error = "test " + i +" : value is null";
				Debug.LogError( Error );
				return ;
			}
			else if( dValue != null && t.value == null )
			{
				Error = "test " + i +" : deserialized value is not null";
				Debug.LogError( Error );
				return ;
			}

			if( !dValue.Equals( t.value)  )
			{
				Error = "test " + i + " : value is not same for " + t.value.GetType() + " : " + t.value + " and " + dValue ;
				Debug.LogError( Error );
				return;
			}
		}
		DeserializeTime = (Time.realtimeSinceStartup-time);

		Succeed = true ;
	}
}
