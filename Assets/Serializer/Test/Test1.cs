using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;


public class Test1 : Test {


	List<TestClass> testList = new List<TestClass>();
	override public void DoTest()
	{
		Description = "Test 1 : Serialize + Deserialize all built-in type ";
		Succeed = false ;

		testList.Add( new TestClass(){  value = true});
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
		testList.Add( new TestClass(){  value = "gdgdf dfgg"});
		testList.Add( new TestClass(){  value = this.GetType()});
		testList.Add( new TestClass(){  value = new DateTime(2015,2,3,5,32,2)});
		testList.Add( new TestClass(){  value = Color.white});
		testList.Add( new TestClass(){  value = Vector2.one});
		testList.Add( new TestClass(){  value = Vector3.one});
		testList.Add( new TestClass(){  value = Vector4.one});
		testList.Add( new TestClass(){  value = Quaternion.LookRotation( Vector3.one)});
		testList.Add( new TestClass(){  value = Matrix4x4.identity});

		float time = Time.realtimeSinceStartup ;
		for( int i = 0 ; i < testList.Count ; i++ )
		{
			TestClass t = testList[i];
			t.d = SerializerSystem.Serialize( t.value );
			DataSize += t.d.Length ;


		}
		SerializeTime = (Time.realtimeSinceStartup-time);
		time = Time.realtimeSinceStartup ;
		for( int i = 0 ; i < testList.Count ; i++ )
		{
			TestClass t = testList[i];
			object dValue = SerializerSystem.Deserialize( t.d , t.value.GetType() );
			if( dValue == null )
			{
				Error = "value is null";
				return ;
			}
			if( !dValue.Equals( t.value ) )
			{
				Error = "value is not same for " + t.value.GetType() + " " + t.value ;
				return;
			}
		}
		DeserializeTime = (Time.realtimeSinceStartup-time);

		Succeed = true ;
	}
}
