using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {
    #region Data 
    public Item Item { get; private set; }
    public int Amount { get; private set; }
    #endregion

    #region UI Component

    private float targetScale = 1f;
    private Vector3 animationScale = new Vector3(1.4f, 1.4f, 1.4f);
    private float smoothing = 0.05f;

    private void Update()
    {
        if (transform.localScale.x != targetScale)
        {
            //动画
            float scale = Mathf.Lerp(transform.localScale.x, targetScale,smoothing);
            transform.localScale = new Vector3(scale, scale, scale);
            if (Mathf.Abs(transform.localScale.x - targetScale) < 0.002)
            {
                transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            }
        

        }
    }
    private Image itemImage;
    private Text amountText;

    private Image ItemImage
    {
        get
        {
            if (itemImage == null)
            {
                itemImage = GetComponent<Image>();

            }
            return itemImage;
        }
    }

    private Text AmountText
    {
        get
        {
            if(amountText == null)
            {
                amountText = GetComponentInChildren<Text>();
            }
            return amountText;
        }
    }
    #endregion
    private void Start()
    { 
       
    }

    public void SetItem(Item item,int amount = 1)
    {
        transform.localScale = animationScale;
        this.Item = item;
        this.Amount = amount;

        ItemImage.sprite = Resources.Load<Sprite>(item.Sprite);
        AmountText.text = Amount.ToString();
        if (Item.Capacity > 1)
            AmountText.text = Amount.ToString();
        else
            AmountText.text = "";
    }
   
    public void AddAmount(int amount = 1)
    {
        transform.localScale = animationScale;
        this.Amount += amount;
        //update UI 
        if (Item.Capacity > 1)
            AmountText.text = Amount.ToString();
        else
            AmountText.text = "";
    }

    public void ReduceAmount(int amount = 1)
    {
        transform.localScale = animationScale;

        this.Amount -= amount;
        if(Item.Capacity > 1)
            AmountText.text = Amount.ToString();
        else
            AmountText.text = "";

    }
    public void SetAmount(int amount)
    {
        transform.localScale = animationScale;
        this.Amount = amount;
        if(Item.Capacity > 1)
            AmountText.text = Amount.ToString();
        else
            AmountText.text = "";
    }
    /// <summary>
    /// 当前物品和itemUI交换
    /// </summary>
    /// <param name="itemUI"></param>
    public void Exchange(ItemUI itemUI)
    {
        Item itemTemp = itemUI.Item;
        int amountTemp = itemUI.Amount;
        itemUI.SetItem(this.Item, this.Amount);
        this.SetItem(itemTemp);
        this.SetAmount(amountTemp);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position; 
    }
}
