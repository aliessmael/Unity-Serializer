using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using cloudsoft ;

public class Test5: SerializerTestUnit {

	public Test5()
	{
		Id = 5;
	}
	public class CustomClass
	{
		public bool 	member0 	= true;
		public bool 	member1 	= false;
		public byte 	member2 	= (byte)0x44;
		public sbyte 	member3 	= (sbyte)0x44;
		public char 	member4 	= (char)'a';
		public short 	member5 	= (short)-434;
		public ushort 	member6 	= (ushort)6677;
		public int 		member7 	= (int)-9897;
		public uint 	member8 	= (uint)87876;
		public long 	member9 	= (long)55667;
		public ulong 	member10 	= (ulong)77665;
		public float 	member11 	= (float)565.9947f;
		public double 	member12 	= (double)455.6644;
		public decimal 	member13 	= new decimal(1,2,3,true,0x0f);
		public string 	member14 	= "gdgdf dfgg";
		public string 	member15 	= "";
		public System.Type member16 = (new Vector2()).GetType();
		public System.Type member17 = typeof(int);
		public System.DateTime member18 = new System.DateTime(2015,2,3,5,32,2);
		public Color 	member19 	= Color.white;
		public Color 	member20 	= Color.black;
		public Vector2 	member21 	= Vector2.one;
		public Vector2 	member22 	= Vector2.zero;
		public Vector3 	member23 	= Vector3.one;
		public Vector3 	member24 	= Vector3.zero;
		public Vector4 	member25 	= Vector4.one;
		public Vector4 	member26 	= Vector4.zero;
		public Quaternion member27 	= Quaternion.LookRotation(new Vector3(5f,5f,5f),new Vector3(1f,8f,9f));
		public Quaternion member28 	= Quaternion.identity;
		public Matrix4x4 member29 	= Matrix4x4.Perspective(0.5f,2f,1f,3f);
		public Matrix4x4 member30 	= Matrix4x4.identity;

		public int[]	 member31 	= null;
		public int[]	 member32 	= new int[0];

		public List<int> member33 	= null;
		public List<int> member34 	= new List<int>();

		public Dictionary<int,int>	 member35 	= null;
		public Dictionary<int,int>	 member36 	= new Dictionary<int,int>();
		//public object	 member37 	= 10;
		public object	 member38 	= null;

	}
	
	override public void DoTest()
	{
		Description = "Test 5 Succeed : Serialize + Deserialize Custom class" ;
		Succeed = false ;
		
		CustomClass custom = new CustomClass();


		
		float time = Time.realtimeSinceStartup ;

		byte[] data = SerializerSystem.Serialize( custom );
		DataSize += data.Length ;
		SerializeTime = (Time.realtimeSinceStartup-time);

		time = Time.realtimeSinceStartup ;
		CustomClass dCustom = SerializerSystem.Deserialize<CustomClass>( data );
		DeserializeTime = (Time.realtimeSinceStartup-time);

		if( custom.member0 != dCustom.member0 )
		{
			Error = "member0 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member1 != dCustom.member1 )
		{
			Error = "member1 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member2 != dCustom.member2 )
		{
			Error = "member2 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member3 != dCustom.member3 )
		{
			Error = "member3 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member4 != dCustom.member4 )
		{
			Error = "member4 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member5 != dCustom.member5 )
		{
			Error = "member5 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member6 != dCustom.member6 )
		{
			Error = "member6 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member7 != dCustom.member7 )
		{
			Error = "member7 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member8 != dCustom.member8 )
		{
			Error = "member8 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member9 != dCustom.member9 )
		{
			Error = "member9 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member10 != dCustom.member10 )
		{
			Error = "member10 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member11 != dCustom.member11 )
		{
			Error = "member11 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member12 != dCustom.member12 )
		{
			Error = "member12 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member13 != dCustom.member13 )
		{
			Error = "member13 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member14 != dCustom.member14 )
		{
			Error = "member14 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member15 != dCustom.member15 )
		{
			Error = "member15 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member16 != dCustom.member16 )
		{
			Error = "member16 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member17 != dCustom.member17 )
		{
			Error = "member17 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member18 != dCustom.member18 )
		{
			Error = "member18 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member19 != dCustom.member19 )
		{
			Error = "member19 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member20 != dCustom.member20 )
		{
			Error = "member20 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member21 != dCustom.member21 )
		{
			Error = "member21 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member22 != dCustom.member22 )
		{
			Error = "member22 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member23 != dCustom.member23 )
		{
			Error = "member23 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member0 != dCustom.member0 )
		{
			Error = "member0 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member24 != dCustom.member24 )
		{
			Error = "member24 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member25 != dCustom.member25 )
		{
			Error = "member25 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member26 != dCustom.member26 )
		{
			Error = "member26 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member27 != dCustom.member27 )
		{
			Error = "member27 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member28 != dCustom.member28 )
		{
			Error = "member28 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member29 != dCustom.member29 )
		{
			Error = "member29 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member30 != dCustom.member30 )
		{
			Error = "member30 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member31 != dCustom.member31 )
		{
			Error = "member31 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member32.Length != 0 || (custom.member32.Length != dCustom.member32.Length))
		{
			Error = "member32 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member33 != dCustom.member33 )
		{
			Error = "member33 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member34.Count != 0 || ( custom.member34.Count != dCustom.member34.Count) )
		{
			Error = "member34 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member35 != dCustom.member35 )
		{
			Error = "member35 is not same";
			Debug.LogError( Error );
			return ;
		}
		if( custom.member36.Count != 0 ||  ( custom.member36.Count != dCustom.member36.Count)  )
		{
			Error = "member36 is not same";
			Debug.LogError( Error );
			return ;
		}
		/*if( !custom.member37.Equals( dCustom.member37 ) )
		{
			Error = "member37 is not same";
			Debug.LogError( Error );
			return ;
		}*/
		if( custom.member38 != dCustom.member38 )
		{
			Error = "member38 is not same";
			Debug.LogError( Error );
			return ;
		}


		Succeed = true ;
	}
}
