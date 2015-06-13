using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;

public class Test5: Test {
	
	public class Person
	{
		public string 		name ;
		public int 			age ;
		public string 		phone ;
		public string 		address ;
	}
	public class Friend : Person
	{
		public string email ;
		public string password ;
	}
	
	public class TestClass
	{
		public Person		bestFriend = new Person();
		public Person		worsFriend = null ;
		public List<Person> friends = new List<Person>();
	}

	override public void DoTest()
	{
		Description = "Test 5 Succeed : Same of previous test but force it to be dynamic types" ;
		Succeed = false ;

		TestClass a = new TestClass();
		for( int i = 0 ; i < 10000 ; i++ )
		{
			a.friends.Add( new Person(){ name = "n"+i, age = 21+i , phone = "p"+i , address = "a"+i});
		}
		float t = Time.realtimeSinceStartup ;
		byte[] d =  SerializerSystem.Serialize( a );
		DataSize = d.Length ;
		SerializeTime = ( Time.realtimeSinceStartup - t ) ;
		t = Time.realtimeSinceStartup ;
		TestClass b = SerializerSystem.Deserialize<TestClass>( d );
		DeserializeTime = ( Time.realtimeSinceStartup - t ) ;
		if( a.friends.Count == b.friends.Count )
		{
			for( int i = 0 ; i < a.friends.Count ; i++ )
			{
				if( !a.friends[i].name.Equals( b.friends[i].name )||
				   a.friends[i].age != b.friends[i].age ||
				   !a.friends[i].phone.Equals( b.friends[i].phone) ||
				   !a.friends[i].address.Equals( b.friends[i].address) )
				{
					return;
				}
			}
			
		}

		Succeed = true ;
	}
}
