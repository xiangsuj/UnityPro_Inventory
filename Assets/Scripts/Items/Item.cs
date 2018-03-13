using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// item base class
/// </summary>
public class Item  {

	public int ID{ get; set;}

	public string Name{ get; set;}
	public ItemType Type{ get;set;}
	public ItemQuality Quality{  get;set;}
	public string  Description{ get; set;}
	public int Capacity{ get; set;}
	public int BuyPrice{ get; set;}
	public int SellPrice{ get; set;} 
	public string Sprite{ get; set;}
	public Item(int id,string name,ItemType itemType,ItemQuality quality,
		string des,int capacity,int buyPrice,int sellPrice,string sprite){
		this.ID = id;
		this.Name = name;
		this.Type = itemType;
		this.Quality = quality;
		this.Description = des;
		this.Capacity = capacity; 
		this.BuyPrice = buyPrice;
		this.SellPrice = sellPrice;
		this.Sprite = sprite;
	}

	/// <summary>
	/// Item type.
	/// </summary>
	public enum ItemType
	{
		Consumable,
		Equipment,
		Weapon,
		Material
	}

	public enum ItemQuality
	{
		Common,
		Uncommon,
		Rare,
		Epic,
        Legendary,
		Artifact
	}

    /// <summary>
    /// 得到提示面板显示的内容
    /// </summary>
    /// <returns></returns>
    public virtual string GetToolTipText()
    {
        string color = "";
        switch (Quality)
        {
            case ItemQuality.Common:
                color = "white";
                break;
            case ItemQuality.Uncommon:
                color = "lime";
                break;
            case ItemQuality.Rare:
                color = "navy";
                break;
            case ItemQuality.Epic:
                color = "magenta";
                break;
            case ItemQuality.Legendary:
                color = "orange";
                break;
            case ItemQuality.Artifact:
                color = "red";
                break;
        }
        string text = string.Format("<color={4}>{0}</color>\n<size=20><color=green>购买价格：{1} 出售价格：{2}</color></size>\n<color=yellow><size=20>{3}</size></color>", Name, BuyPrice, SellPrice, Description, color);
        return text;
    }
}
