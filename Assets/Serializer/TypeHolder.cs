using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace cloudsoft
{
	public class TypeHolder  {

		public Type   				Type ;
		//public SerializerType		BaseType = null;
		public bool 				SubclassIsCalculated = false;
		public List<TypeHolder> SubTypes = new List<TypeHolder>();
		//public int  				Index = 0;
		public Serializer			Serializer;


		//for editor
		//public bool 				ReplaceThis = false;//to replace with proper reference
		public string 				TypeStr = "";
		public string				TypeStrLower;
	}
}
