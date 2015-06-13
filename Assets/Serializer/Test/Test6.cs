using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;

public class Test6: Test {
	
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
		Description = "Test 6 Succeed : Serialize + Deserialize Custom class with fixed type field" ;
		Succeed = false ;

		TestClass a = new TestClass();
		a.bestFriend = new Friend(){ name = "b2", age = 26 , phone = "bp2" , address = "ba2" , email = "bem" , password = "b**" };
		a.friends.Add( new Person(){ name = "n1", age = 21 , phone = "p1" , address = "a1"});
		a.friends.Add( new Friend(){ name = "n2", age = 21 , phone = "p2" , address = "a2" , email = "em" , password = "**"});

		float t = Time.realtimeSinceStartup ;
		byte[] d =  SerializerSystem.Serialize( a );
		DataSize = d.Length ;
		SerializeTime = (Time.realtimeSinceStartup-t);
		t = Time.realtimeSinceStartup ;
		TestClass b = SerializerSystem.Deserialize<TestClass>( d );
		DeserializeTime = (Time.realtimeSinceStartup-t);
		if( a.friends.Count == b.friends.Count )
		{
			if(b.bestFriend.GetType() != typeof(Friend ))
			{
				Error = "Different type";
				return;
			}
			else
			{
				for( int i = 0 ; i < b.friends.Count ; i++ )
				{
					if(  b.friends[i].GetType() != a.friends[i].GetType())
					{
						Error = "Different type";
						return;
					}
					if( !a.friends[i].name.Equals( b.friends[i].name )||
					   a.friends[i].age != b.friends[i].age ||
					   !a.friends[i].phone.Equals( b.friends[i].phone) ||
					   !a.friends[i].address.Equals( b.friends[i].address) )
					{
						Error = "Value are different";
						return;
					}
				}
			}
			
		}

		Succeed = true ;
	}
}
