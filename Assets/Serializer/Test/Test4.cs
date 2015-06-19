using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using cloudsoft ;

public class Test4: SerializerTestUnit {

	public Test4()
	{
		Id = 4;
	}

	List<TestClass> testList = new List<TestClass>();
	override public void DoTest()
	{
		Description = "Test 4 Succeed : Serialize + Deserialize Dictionary of all built-in types" ;
		Succeed = false ;
		
		TestClass test = null;
		
		test = new TestClass();
		Dictionary<bool,bool> boolDictionary = new Dictionary<bool,bool>();
		boolDictionary[true]= true;
		boolDictionary[false]= false;
		test.value = boolDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<byte,byte> byteDictionary = new Dictionary<byte,byte>();
		byteDictionary[0x44] = 0x44;
		test.value = byteDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<sbyte,sbyte> sbyteDictionary = new Dictionary<sbyte,sbyte>();
		sbyteDictionary[0x44] = 0x44;
		test.value = sbyteDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<char,char> charDictionary = new Dictionary<char,char>();
		charDictionary['a'] = 'a';
		test.value = charDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<short,short> shortDictionary = new Dictionary<short,short>();
		shortDictionary[-434] = -434;
		test.value = shortDictionary ;
		testList.Add( test );
		
		
		test = new TestClass();
		Dictionary<ushort,ushort> ushortDictionary = new Dictionary<ushort,ushort>();
		ushortDictionary[6677] = 6677;
		test.value = ushortDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<int,int> intDictionary = new Dictionary<int,int>();
		intDictionary[-9897] = -9897;
		test.value = intDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<uint,uint> uintDictionary = new Dictionary<uint,uint>();
		uintDictionary[87876] = 87876;
		test.value = uintDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<long,long> longDictionary = new Dictionary<long,long>();
		longDictionary[55667] = 55667;
		test.value = longDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<ulong,ulong> ulongDictionary = new Dictionary<ulong,ulong>();
		ulongDictionary[77665] = 77665;
		test.value = ulongDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<float,float> floatDictionary = new Dictionary<float,float>();
		floatDictionary[565.9947f] = 565.9947f;
		test.value = floatDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<double,double> doubleDictionary = new Dictionary<double,double>();
		doubleDictionary[455.6644] = 455.6644;
		test.value = doubleDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<decimal,decimal> decimalDictionary = new Dictionary<decimal,decimal>();
		decimalDictionary[new decimal(1,2,3,true,0x0f)] = new decimal(1,2,3,true,0x0f);
		test.value = decimalDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<string,string> stringDictionary = new Dictionary<string,string>();
		stringDictionary["gdgdf"] = "gdgdf";
		test.value = stringDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<System.Type,System.Type> TypeDictionary = new Dictionary<System.Type,System.Type>();
		TypeDictionary[typeof(int)] = typeof(int);
		TypeDictionary[this.GetType()] = this.GetType();
		test.value = TypeDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<System.DateTime,System.DateTime> DateTimeDictionary = new Dictionary<System.DateTime,System.DateTime>();
		DateTimeDictionary[new System.DateTime(2015,2,3,5,32,2)] = new System.DateTime(2015,2,3,5,32,2);
		DateTimeDictionary[new System.DateTime()] = new System.DateTime();
		test.value = DateTimeDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<Color,Color> ColorDictionary = new Dictionary<Color,Color>();
		ColorDictionary[Color.white] = Color.white;
		ColorDictionary[Color.red] = Color.red;
		test.value = ColorDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<Vector2,Vector2> Vector2Dictionary = new Dictionary<Vector2,Vector2>();
		Vector2Dictionary[Vector2.zero] = Vector2.zero;
		Vector2Dictionary[Vector2.one] = Vector2.one;
		test.value = Vector2Dictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<Vector3,Vector3> Vector3Dictionary = new Dictionary<Vector3,Vector3>();
		Vector3Dictionary[Vector3.zero] = Vector3.zero;
		Vector3Dictionary[Vector3.one] = Vector3.one;
		test.value = Vector3Dictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<Vector4,Vector4> Vector4Dictionary = new Dictionary<Vector4,Vector4>();
		Vector4Dictionary[Vector4.zero] = Vector4.zero;
		Vector4Dictionary[Vector4.one] = Vector4.one;
		test.value = Vector4Dictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<Quaternion,Quaternion> QuaternionDictionary = new Dictionary<Quaternion,Quaternion>();
		QuaternionDictionary[Quaternion.identity] = Quaternion.identity;
		QuaternionDictionary[new Quaternion(0.1f,0.2f,0.3f,0.4f)] =  new Quaternion(0.1f,0.2f,0.3f,0.4f);
		test.value = QuaternionDictionary ;
		testList.Add( test );
		
		test = new TestClass();
		Dictionary<Matrix4x4,Matrix4x4> Matrix4x4Dictionary = new Dictionary<Matrix4x4,Matrix4x4>();
		Matrix4x4Dictionary[Matrix4x4.identity] = Matrix4x4.identity;
		Matrix4x4Dictionary[Matrix4x4.Perspective(0.5f,0.8f,0.3f,4.5f)] =  Matrix4x4.Perspective(0.5f,0.8f,0.3f,4.5f);
		test.value = Matrix4x4Dictionary ;
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
			IDictionary dictionary1 = (IDictionary)t.value ;
			IDictionary dictionary2 = (IDictionary)dValue ;
			if( dValue == null )
			{
				Error = "dictionary is null";
				Debug.LogError( Error );
				return ;
			}
			if( dictionary1.Count != dictionary2.Count)
			{
				Error = "dictionary lengths are not same  dictionary1.Length = " + dictionary1.Count + " dictionary2.Length = " + dictionary2.Count ;
				Debug.LogError( Error );
				return;
			}
			foreach( var key in dictionary2.Keys )
			{
				if( !dictionary1.Contains (key ))
				{
					Error = "dictionary1 does not has key " + key ;
					Debug.LogError( Error );
					return;
				}
				if( !dictionary2[key].Equals( dictionary1[key] ) )
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
