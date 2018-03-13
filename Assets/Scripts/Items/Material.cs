using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material : Item {

	public Material(int id,string name,ItemType itemType,ItemQuality quality,
		string des,int capacity,int buyPrice,int sellPrice,string sprite)
		:base(id,name,itemType,quality,des,capacity,buyPrice,sellPrice,sprite)
	{
		
	}
}
