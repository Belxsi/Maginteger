using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Content;
    public InterfaceAudioEffect iae;
    public static Dictionary<string,Slot> slots = new();
    public static Inventory me;
    public static bool open;
    void Start()
    {
        me = this;
        ObserveNullUI();
    }
    public void SetStateOpen(bool trfl)
    {
        open = trfl;
        BaseFunc.SetPause(trfl);
        
    }
 
    public void AddSlot()
    {
        
        //slotsUI.Add(Instantiate(BaseFunc.GetPrefab("SlotUI"), Content).GetComponent<SlotUI>().slot);


    }
    public void ObserveNullUI()
    {
        if (slots.Count > 0)
        {
            foreach(var slot in slots)
            {
                if (slot.Value.ui == null)
                {
                    SlotUI slotUI = Instantiate(BaseFunc.GetPrefab("SlotUI"), me.Content).GetComponent<SlotUI>();
                    slotUI.SetValueSlot(slot.Value);
                    slot.Value.ui = slotUI;
                    slotUI.InitSlotItem();
                }
            }
        }
    }
    public static bool TryGetItem(string nam, out Slot item)
    {
        if(slots.TryGetValue(nam,out Slot value)){
            item = value;
            return true;
        }
        item = null; 
        return false;
    }
    public static void AddItem(Item item)
    {
        
        if (TryGetItem(item.name+ IItemIteraction.Features(item.iii), out Slot get))
        {
            get.count++;

        }
        else
        {
            SlotUI slotUI = Instantiate(BaseFunc.GetPrefab("SlotUI"),me.Content).GetComponent<SlotUI>();
            slotUI.SetValueSlot(new(1, item));
          //  slotUI.slot.objs.Add(obj);
            slotUI.GetSlot().ui = slotUI;
            slotUI.InitSlotItem();
            item.slot = slotUI;
            
            slots.Add(item.name+ IItemIteraction.Features(item.iii), slotUI.GetSlot()) ;
        }

    }
    public static void RemoveItem(Item item)
    {

        if (TryGetItem(item.name + IItemIteraction.Features(item.iii), out Slot get))
        {
            get.count--;
           
            if (get.count == 0)
            {
                slots.Remove(item.name+ IItemIteraction.Features(item.iii));
                Destroy(get.ui.gameObject);
            }

        }
      

    }
    /*
    public static void CreateSlot(Slot slot)
    {
       
        SlotUI su = Instantiate(BaseFunc.GetPrefab("SlotUI"), me.Content).GetComponent<SlotUI>();
        su.slot.count = slot.count;
        su.slot.item = slot.item;
        
        slotsUI.Add(su.slot);
    }
    */
    /*
    public bool SumCountSlot(Slot s)
    {
        for(int i = 0; i < slotsUI.Count; i++)
        {
            
            if (s.item.id == slotsUI[i].item.id)
            {
                slotsUI[i].count++;
                return true;
            }
        }
        return false;
    }
    public void ReadSlot(Slot readed)
    {
        if (!SumCountSlot(readed))
        {
           
            SlotUI added = Instantiate(BaseFunc.GetPrefab("SlotUI"), Content).GetComponent<SlotUI>();
            added.free = false;
            added.slot.count = 1;
            added.slot.item = readed.item;
            
            added.slot.ui = added;
            slotsUI.Add(added.slot);
        }
        

    }
    */
    /*
    public static void AddSlot(SlotUI slot)
    {
       
        slotsUI.Add(slot.slot);

    }
    public static void RemoveSlot(Slot slot)
    {
        slotsUI.Remove(slot);

    }
    */
    // Update is called once per frame
    void Update()
    {
        
    }
}
[Serializable]

public class Item
{
    public string name;
    public int id;
    public float value;
    public Sprite sprite;
    public ItemType type;
    public GameObject valueObj;
    public SlotUI slot;
    public IItemIteraction iii;
    public void UpdateId()
    {
        id= UnityEngine.Random.Range(int.MinValue,int.MaxValue);
    }
    public Item(string name,int id,float value)
    {
        this.name = name;
        this.id = id;
        this.value = value;
        type = ItemType.Numeric;
    }
    public Item(string name, int id,GameObject valueObj,Sprite sprite)
    {
        this.name = name;
        this.id = id;
        this.valueObj = valueObj;
        type = ItemType.Item;
    }
}
public enum ItemType
{
    Numeric,
    Item
}
[Serializable]
public class Slot
{
    public Item item=null;
    public int count=0;
    public SlotUI ui;
    public List<GameObject> objs;
    public Slot(SlotUI ui)
    {
        this.ui = ui;
    }
    public Slot(int count,Item item)
    {
        this.count = count;
        this.item = item;
    }
}
