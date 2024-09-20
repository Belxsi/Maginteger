using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonSlot : MonoBehaviour
{
    public bool card;
    public string value;
    public TextMeshProUGUI tmp;
    public Image sr;
    public Slot slot;
    public int index;
    
    public Sprite Default;
    public void Init(bool card, string value,Slot s=null)
    {
        Default = sr.sprite;
        this.card = card;
        this.value = value;
        slot = s;
    }
    public void Action()
    {
        tmp.text = value;
        if (card)
        {
            sr.sprite = Default;

        }
        else
        {
            
            sr.sprite = BaseFunc.GetSpritePrefab("Square");
        }
    }
    public void Update()
    {
        transform.SetSiblingIndex(index);
    }
    public void AutoDestroy(Slot slot)
    {
        if (slot != null)
        {
            Inventory.RemoveItem(slot.item);
           // if(card)
            //Inventory.AddItem(slot.item);

            Destroy(gameObject);
        }
        else Destroy(gameObject);
    }


}
