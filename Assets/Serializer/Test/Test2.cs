using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;

public class Test2: Test {


	List<TestClass> testList = new List<TestClass>();
	override public void DoTest()
	{
		Description = "Test 2 Succeed : Serialize + Deserialize Array of all built-in types" ;
		Succeed = false ;

		testList.Add( new TestClass(){  value = new bool[]{true} });
		testList.Add( new TestClass(){  value = new byte[]{0x44} });
		testList.Add( new TestClass(){  value = new sbyte[]{0x44} });
		testList.Add( new TestClass(){  value = new char[]{'a'} });
		testList.Add( new TestClass(){  value = new short[] {-434} });
		testList.Add( new TestClass(){  value = new ushort[] {6677} });
		testList.Add( new TestClass(){  value = new int[]{-9897} });
		testList.Add( new TestClass(){  value = new uint[]{87876} });
		testList.Add( new TestClass(){  value = new long[]{55667} });
		testList.Add( new TestClass(){  value = new ulong[]{77665} });
		testList.Add( new TestClass(){  value = new float[]{565.9947f} });
		testList.Add( new TestClass(){  value = new double[] {455.6644} });
		testList.Add( new TestClass(){  value = new decimal[]{ new decimal(1,2,3,true,0x0f)} });
		testList.Add( new TestClass(){  value = new string[]{"gdgdf dfgg"} });
		testList.Add( new TestClass(){  value = new Type[]{ this.GetType()} });
		testList.Add( new TestClass(){  value = new DateTime[]{ new DateTime(2015,2,3,5,32,2)} } );
		testList.Add( new TestClass(){  value = new Color[]{Color.white} });
		testList.Add( new TestClass(){  value = new Vector2[]{Vector2.one} });
		testList.Add( new TestClass(){  value = new Vector3[]{Vector3.one} });
		testList.Add( new TestClass(){  value = new Vector4[]{Vector4.one} });
		testList.Add( new TestClass(){  value = new Quaternion[]{Quaternion.LookRotation( Vector3.one)} });
		testList.Add( new TestClass(){  value = new Matrix4x4[]{Matrix4x4.identity} });

		float time = Time.realtimeSinceStartup ;
		for( int i = 0 ; i < testList.Count ; i++ )
		{
			Debug.Log("Serialize Test : " + i );
			TestClass t = testList[i];
			t.d = SerializerSystem.Serialize( t.value );
			DataSize += t.d.Length ;
			
			
		}
		SerializeTime = (Time.realtimeSinceStartup-time);
		time = Time.realtimeSinceStartup ;
		for( int i = 0 ; i < testList.Count ; i++ )
		{
			Debug.Log("Deserialize Test : " + i );
			TestClass t = testList[i];
			object dValue = SerializerSystem.Deserialize( t.d , t.value.GetType() );
			Array array1 = (Array)t.value ;
			Array array2 = (Array)dValue ;
			if( dValue == null )
			{
				Error = "array is null";
				return ;
			}
			if( array1.Length != array2.Length)
			{
				Error = "array lengths are not same  array1.Length = " + array1.Length + " array2.Length = " + array2.Length ;
				return;
			}
			for( int j = 0 ; j < array2.Length ; j++ )
			{
				if( !array2.GetValue(j).Equals( array1.GetValue(j) ) )
				{
					Error = "value is not same for " + t.value.GetType() + " " + t.value ;
					return;
				}
			}
		}
		DeserializeTime = (Time.realtimeSinceStartup-time);
		
		Succeed = true ;
	}

}
