 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler  {

    public GameObject itemPrefab;

    /// <summary>
    /// 把item放在自身下面
    /// 如果自身下面已经有item，amount++
    /// 如果自身下面没有item，根据ItemPrefab去实例化一个，放到自身下面
    /// </summary>
    /// <param name="item"></param>
	public void StoreItem(Item item)
    {
       
        if (transform.childCount == 0)
        {
            GameObject itemGameObject= Instantiate(itemPrefab)as GameObject;
            itemGameObject.transform.SetParent(this.transform,false);
            itemGameObject.transform.localScale =new Vector3(1f, 1f, 1f);
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetItem(item);
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }
    /// <summary>
    /// 得到当前物品槽存储物品的类型
    /// </summary>
    /// <returns></returns>
    public Item.ItemType GetItemType()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Type;
    }

    public bool IsFilled()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
      
        return itemUI.Amount >= itemUI.Item.Capacity;
    }

    public int GetItemId()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.ID;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if (transform.childCount > 0)
        {
            string toolTipText = transform.GetChild(0).
                GetComponent<ItemUI>().Item.GetToolTipText();
           
            InventoryManager.Instance.ShowToolTip(toolTipText);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            InventoryManager.Instance.HideToolTip();
        }
            
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.Instance.IsPickedItem==false&&transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                if(currentItemUI.Item is Equipment||currentItemUI.Item is Weapon)
                {
                    
                    currentItemUI.ReduceAmount(1);
                    Item currentItem = currentItemUI.Item;
                    if (currentItemUI.Amount <= 0)
                    {
                        DestroyImmediate(currentItemUI.gameObject);
                        InventoryManager.Instance.HideToolTip();
                    }
                    CharacterPanel.Instance.PutOn(currentItem);
                }
            }
        }
        if (eventData.button != PointerEventData.InputButton.Left) return;//不是左键点击就返回
        //自身是空slot
        ////1.isPickedItem == true   pickedItem放在这个位置
        //////按下ctrl      放置一个当前物品
        //////没有按下ctrl  放置所有当前物品
        ////2.isPickedItem == false    不做任何处理


        //自身不是空slot
        ////1.isPickedItem == true

        //////自身的id==pickedItem.id
        //////////按下ctrl      放置一个当前物品
        //////////没有按下ctrl  放置所有当前物品
        //////////////////可以完全放下
        //////////////////只能放下其中一部分
        //////自身的id！=pickedItem.id   pickedItem跟当前物品交换

        ////2.isPickedItem == false  
        /////////ctrl按下       取得当前物品槽中的一半
        /////////没有按下ctrl   把当前物品槽的物品放到鼠标上
        if (transform.childCount > 0)
        {
            ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
            if (InventoryManager.Instance.IsPickedItem == false)//当前没有选中任何物品（当前手上没有任何物品）
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    int amountPicked = (currentItem.Amount + 1) / 2;
                    InventoryManager.Instance.PickupItem(currentItem.Item, amountPicked);
                    int amountRemained = currentItem.Amount - amountPicked;
                    if (amountRemained <= 0)
                    {
                        Destroy(currentItem.gameObject);
                    }
                    else
                    {
                        currentItem.SetAmount(amountRemained);
                    }
                }
                else
                {

                    //把当前物品槽的信息，复制给PickedItem(跟随鼠标移动)
                    InventoryManager.Instance.PickupItem(currentItem.Item,currentItem.Amount);
                    Destroy(currentItem.gameObject);//销毁当前物体
                }
            }
            else
            {
                ////1.isPickedItem == true

                //////自身的id==pickedItem.id
                //////////按下ctrl      放置一个当前物品
                //////////没有按下ctrl  放置所有当前物品
                //////////////////可以完全放下
                //////////////////只能放下其中一部分
                //////自身的id！=pickedItem.id   pickedItem跟当前物品交换
                if (currentItem.Item.ID == InventoryManager.Instance.PickedItem.Item.ID)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if(currentItem.Item.Capacity>currentItem.Amount)//当前物品槽还有容量
                        {
                            currentItem.AddAmount();
                            InventoryManager.Instance.RemoveItem();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (currentItem.Item.Capacity > currentItem.Amount)//当前物品槽还有容量
                        {
                            int amountRemain = currentItem.Item.Capacity - currentItem.Amount;//当前物品槽剩余的空间
                            if (amountRemain >= InventoryManager.Instance.PickedItem.Amount)
                            {
                                currentItem.SetAmount(currentItem.Amount + InventoryManager.Instance.PickedItem.Amount);
                                InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                            }
                            else
                            {
                                currentItem.SetAmount(currentItem.Amount+amountRemain);
                                InventoryManager.Instance.RemoveItem(amountRemain);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Item item = currentItem.Item;
                    int amount = currentItem.Amount;

                    currentItem.SetItem(InventoryManager.Instance.PickedItem.Item, InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.PickedItem.SetItem(item, amount);
                     
                }
            }

        }
        else
        {
            //自身是空slot
            ////1.isPickedItem == true   pickedItem放在这个位置
            //////按下ctrl      放置一个当前物品
            //////没有按下ctrl  放置所有当前物品
            ////2.isPickedItem == false    不做任何处理
            if (InventoryManager.Instance.IsPickedItem == true)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    InventoryManager.Instance.RemoveItem();
                }
                else
                {
                    for(int i = 0; i < InventoryManager.Instance.PickedItem.Amount; i++)
                    {
                        this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    }
                    InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                }
            }
            else
            {
                return;
            }
        }
    }
}
