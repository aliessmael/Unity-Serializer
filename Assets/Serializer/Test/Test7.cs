using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cloudsoft ;

public class Test7: Test {
	
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
		Description = "Test 7 Succeed : Serialize + Deserialize Custom class with dynamic type field" ;
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
				Error = "bestFriend is not Friend type";
				return;
			}
			else
			{
				if(  b.friends[0].GetType() != typeof(Person ))
				{
					Error = " b.friends[0] is not Person type";
					return;
				}
				if(  b.friends[1].GetType() != typeof(Friend ))
				{
					Error = " b.friends[1] is not Friend type";
					return;
				}
				for( int i = 0 ; i < b.friends.Count ; i++ )
				{
					
					if( !a.friends[i].name.Equals( b.friends[i].name )||
					   a.friends[i].age != b.friends[i].age ||
					   !a.friends[i].phone.Equals( b.friends[i].phone) ||
					   !a.friends[i].address.Equals( b.friends[i].address) )
					{
						Error = "a.friends[i] not b.friends[i]";
						return;
					}
				}
			}
			
		}
		Succeed = true ;
	}
}
