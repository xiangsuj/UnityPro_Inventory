using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula  {

	public int Item1ID { get; private set; }
    public int Item1Amount { get; private set; }
    public int Item2ID { get; private set; }
    public int Item2Amount { get; private set; }

    public int ResId { get; private set; }//结果物品ID

    private List<int> needIdList = new List<int>();//所需要物品的ID

    public List<int> NeedIdList
    {
        get
        {
            return needIdList;
        }
    }
    public Formula(int item1ID,int item1Amount,int item2ID,int item2Amount,int resID)
    {
        this.Item1ID = item1ID;
        this.Item1Amount = item1Amount;
        this.Item2ID = item2ID;
        this.Item2Amount = item2Amount;
        this.ResId = resID;
        for (int i = 0; i < Item1Amount; i++)
        {
            needIdList.Add(Item1ID);
        }
        for (int i = 0; i < Item2Amount; i++)
        {
            needIdList.Add(Item2ID);
        }
    }

    public bool Match(List<int> idList)//提供的物品的ID
    {
       
       

        List<int> tempIDList = new List<int>(idList);
        foreach(int id in needIdList)
        {
            bool isRemoveSuccess= tempIDList.Remove(id);
            if (isRemoveSuccess == false)
            {
                return false;
            }
        }
        return true;
    }
}
