using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;


public class Test3: SerializerTestUnit {

	public Test3()
	{
		Id = 3;
	}

	List<TestClass> testList = new List<TestClass>();
	override public void DoTest()
	{
		Description = "Test 3 Succeed : Serialize + Deserialize List of all built-in types" ;
		Succeed = false ;

		TestClass test = null;

		test = new TestClass();
		List<bool> boolList = new List<bool>();
		boolList.Add(true);
		test.value = boolList ;
		testList.Add( test );

		test = new TestClass();
		List<byte> byteList = new List<byte>();
		byteList.Add(0x44);
		test.value = byteList ;
		testList.Add( test );

		test = new TestClass();
		List<sbyte> sbyteList = new List<sbyte>();
		sbyteList.Add(0x44);
		test.value = sbyteList ;
		testList.Add( test );

		test = new TestClass();
		List<char> charList = new List<char>();
		charList.Add('a');
		test.value = charList ;
		testList.Add( test );

		test = new TestClass();
		List<short> shortList = new List<short>();
		shortList.Add(-434);
		test.value = shortList ;
		testList.Add( test );


		test = new TestClass();
		List<ushort> ushortList = new List<ushort>();
		ushortList.Add(6677);
		test.value = ushortList ;
		testList.Add( test );

		test = new TestClass();
		List<int> intList = new List<int>();
		intList.Add(-9897);
		test.value = intList ;
		testList.Add( test );

		test = new TestClass();
		List<uint> uintList = new List<uint>();
		uintList.Add(87876);
		test.value = uintList ;
		testList.Add( test );

		test = new TestClass();
		List<long> longList = new List<long>();
		longList.Add(55667);
		test.value = longList ;
		testList.Add( test );

		test = new TestClass();
		List<ulong> ulongList = new List<ulong>();
		ulongList.Add(77665);
		test.value = ulongList ;
		testList.Add( test );

		test = new TestClass();
		List<float> floatList = new List<float>();
		floatList.Add(565.9947f);
		test.value = floatList ;
		testList.Add( test );

		test = new TestClass();
		List<double> doubleList = new List<double>();
		doubleList.Add(455.6644);
		test.value = doubleList ;
		testList.Add( test );

		test = new TestClass();
		List<decimal> decimalList = new List<decimal>();
		decimalList.Add(new decimal(1,2,3,true,0x0f));
		test.value = decimalList ;
		testList.Add( test );

		test = new TestClass();
		List<string> stringList = new List<string>();
		stringList.Add("gdgdf");
		test.value = stringList ;
		testList.Add( test );

		test = new TestClass();
		List<System.Type> TypeList = new List<System.Type>();
		TypeList.Add(typeof(int));
		TypeList.Add(this.GetType());
		test.value = TypeList ;
		testList.Add( test );

		test = new TestClass();
		List<System.DateTime> DateTimeList = new List<System.DateTime>();
		DateTimeList.Add(new System.DateTime(2015,2,3,5,32,2));
		DateTimeList.Add(new System.DateTime());
		test.value = DateTimeList ;
		testList.Add( test );

		test = new TestClass();
		List<Color> ColorList = new List<Color>();
		ColorList.Add(Color.white);
		ColorList.Add(Color.red);
		test.value = ColorList ;
		testList.Add( test );

		test = new TestClass();
		List<Vector2> Vector2List = new List<Vector2>();
		Vector2List.Add(Vector2.zero);
		Vector2List.Add(Vector2.one);
		test.value = Vector2List ;
		testList.Add( test );

		test = new TestClass();
		List<Vector3> Vector3List = new List<Vector3>();
		Vector3List.Add(Vector3.zero);
		Vector3List.Add(Vector3.one);
		test.value = Vector3List ;
		testList.Add( test );

		test = new TestClass();
		List<Vector4> Vector4List = new List<Vector4>();
		Vector4List.Add(Vector4.zero);
		Vector4List.Add(Vector4.one);
		test.value = Vector4List ;
		testList.Add( test );

		test = new TestClass();
		List<Quaternion> QuaternionList = new List<Quaternion>();
		QuaternionList.Add(Quaternion.identity);
		QuaternionList.Add( new Quaternion(0.1f,0.2f,0.3f,0.4f));
		test.value = QuaternionList ;
		testList.Add( test );

		test = new TestClass();
		List<Matrix4x4> Matrix4x4List = new List<Matrix4x4>();
		Matrix4x4List.Add(Matrix4x4.identity);
		Matrix4x4List.Add( Matrix4x4.Perspective(0.5f,0.8f,0.3f,4.5f));
		test.value = Matrix4x4List ;
		testList.Add( test );

		
		float time = Time.realtimeSinceStartup ;
		for( int i = 0 ; i < testList.Count ; i++ )
		{
			//Debug.Log("Serialize Test : " + i );
			TestClass t = testList[i];
			t.d = SerializerSystem.Serialize( t.value );
			DataSize += t.d.Length ;
			
			
		}
		SerializeTime = (Time.realtimeSinceStartup-time);
		time = Time.realtimeSinceStartup ;
		for( int i = 0 ; i < testList.Count ; i++ )
		{
			//Debug.Log("Deserialize Test : " + i );
			TestClass t = testList[i];
			object dValue = SerializerSystem.Deserialize( t.d , t.value.GetType() );
			IList list1 = (IList)t.value ;
			IList list2 = (IList)dValue ;
			if( dValue == null )
			{
				Error = "list is null";
				Debug.LogError( Error );
				return ;
			}
			if( list1.Count != list2.Count)
			{
				Error = "list lengths are not same  list1.Length = " + list1.Count + " list2.Length = " + list2.Count ;
				Debug.LogError( Error );
				return;
			}
			for( int j = 0 ; j < list2.Count ; j++ )
			{
				if( !list2[j].Equals( list1[j] ) )
				{
					Error = "value is not same for " + t.value.GetType() + " " + t.value ;
					Debug.LogError( Error );
					return;
				}
			}
		}
		DeserializeTime = (Time.realtimeSinceStartup-time);
		
		Succeed = true ;
	}

}
