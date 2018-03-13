using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot {

    public Equipment.EquipmentType equipType;

    public Weapon.WeaponType wpType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.Instance.IsPickedItem == false && transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();

                //脱掉装备 放入背包中
                Item itemtmp = currentItemUI.Item;
                DestroyImmediate(currentItemUI.gameObject);
                transform.parent.SendMessage("PutOff", itemtmp);
                
                InventoryManager.Instance.HideToolTip(); 
                   
                
            }
        }

        bool isUpdateProperty = false;
        if (eventData.button != PointerEventData.InputButton.Left) return;//不是左键点击就返回
        //手上有 东西
        //////当前装备槽  有装备
        //////当前装备槽  无装备
        //手上没 东西
        //////当前装备槽  有装备  
        //////当前装备槽  无装备  不做处理 
        if (InventoryManager.Instance.IsPickedItem == true)
        {
            //手上有东西的情况
            //////当前装备槽  有装备
            ItemUI pickedItem = InventoryManager.Instance.PickedItem;
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();//当前装备槽中的物品

                if (IsRightItem(pickedItem.Item))
                {
                    InventoryManager.Instance.PickedItem.Exchange(currentItemUI);
                    isUpdateProperty = true;
                }
            }
            //手上有 东西
            //////当前装备槽  无装备
            else
            {
                if (IsRightItem(pickedItem.Item))
                {
                    this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    InventoryManager.Instance.RemoveItem(1);
                    isUpdateProperty = true;
                }
            }
        }
        else
        {
            //手上没有东西
            //////当前装备槽  有装备  
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                InventoryManager.Instance.PickupItem(currentItemUI.Item, currentItemUI.Amount);
                Destroy(currentItemUI.gameObject);
                isUpdateProperty = true;
            }
            //手上没 东西
            //////当前装备槽  无装备  不做处理 
        }

        if (isUpdateProperty)
        {
            transform.parent.SendMessage("UpdatePropertyText");
        }
    }

    /// <summary>
    /// 判断item是否适合这个装备槽
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsRightItem(Item item)
    {
        if ((item is Equipment) && (((Equipment)(item)).EquipType == this.equipType)
                    ||
                    (item is Weapon) && (((Weapon)(item)).WpType == this.wpType))
        {
            return true;
        }
        return false;
    }
}
