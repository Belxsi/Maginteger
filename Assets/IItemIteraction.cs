using System.Collections;
using UnityEngine;

public abstract class IItemIteraction:MonoBehaviour
{
    public ItemObject io;
    public int state;
    public void Awake()
    {
        io = GetComponent<ItemObject>();
        io.itemInteractive = this;
        state = Random.Range(int.MinValue, int.MaxValue);
    }
    public abstract void Use(Element Creater);
    public abstract void Click();
    public static string Features(IItemIteraction iii)
    {
        string features = "";
        switch (iii.GetType().Name)
        {
            case "ItemMagicScroll":
                features = ((ItemMagicScroll)iii).Spell;
                break;


        }
        return features;
    }
    public abstract string ToStringInfo();
   
    public void UseDestoy()
    {
        if (true)
        {
            GameObject obj = SafeGameObjects.Load(io.item.name, IItemIteraction.Features(io.item.iii));
            obj.transform.position = Player.me.transform.position;
            Inventory.RemoveItem(io.item);


            Destroy(obj);
        }
        else
        {
            Player.me.DropItem();
            Destroy(io.gameObject);
        }
    }
    public void ThrowObject(Element Creater, float liq)
    {
        if (Player.TryGetPlayer())
        {
            ItemTakeLogic itl = GetComponent<ItemTakeLogic>();
            BodyObject bo = GetComponent<BodyObject>();
            itl.enabled = false;
            Player.me.DropItem();
            gameObject.layer = 6;
            bo.Creater = Creater;
            bo.matteryEnergy.LiqEnergy = liq;
            EnergyManyImpuls emi = gameObject.AddComponent<EnergyManyImpuls>();
            emi.bocreater = bo;
            emi.ctc = bo.ctc;
            emi.nomagic = true;
        }
    }
    public abstract IEnumerator RepeatUse();
}
