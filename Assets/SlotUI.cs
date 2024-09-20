using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SlotUI : MonoBehaviour
{
    Slot slot;
    public TextMeshProUGUI tmpNum, tmpCou;
    public ItemType typeVision;
    public bool free, addAwake;
    public Image sr;
    public static EventTriggerType currentType;
    public static EventTrigger currentDrag, currentEnter;
    public CustomEventTrigger cet;
    public void Awake()
    {
        cet = GetComponent<CustomEventTrigger>();
        if (addAwake)
        {
            //Inventory.AddItem(slot.item);
            // slot.item = new("0", 0, 0);
            slot.item.UpdateId();
            SlotBehaviour.me.CetAdd(cet);
        }
    }
    public void SetValueSlot(Slot slot)
    {
        this.slot = slot;
        float size = tmpCou.fontSize;
        tmpCou.autoSizeTextContainer = false;
        tmpCou.fontSize = size;
         size = tmpNum.fontSize;
        tmpNum.autoSizeTextContainer = false;
        tmpNum.fontSize = size;
    }
    public Slot GetSlot()
    {
        return slot;
    }
    public void Init()
    {
        slot = new(this);

    }
    public void InitSlotItem()
    {
        typeVision = ItemType.Item;
        slot.ui = this;

    }

    public void PointClick()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
            slot.item.iii.Click();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            /*
            GameObject obj = SafeGameObjects.Load(slot.item.name, IItemIteraction.Features(slot.item.iii));
            obj.transform.position = Player.me.transform.position;
            Inventory.RemoveItem(slot.item);
            */
            InfoPropertiesItem.me.Show(slot.item.iii.ToStringInfo(),slot.item.name, this);

        }
        currentType = EventTriggerType.PointerClick;
        Inventory.me.iae.AudioAwakeDefault(Inventory.me.iae.button_click);
    }
    public void Exit()
    {
       
        currentType = EventTriggerType.PointerExit;
    }
    public void Enter(EventTrigger et)
    {
       
        currentType = EventTriggerType.PointerEnter;
        currentEnter = et;
    }
    public void BeginDrag(EventTrigger et)
    {
      
        currentType = EventTriggerType.BeginDrag;
        Image gim = SlotBehaviour.me.ghost.GetComponent<Image>();
        SlotBehaviour.me.Gim = gim;
        gim.sprite = et.GetComponent<Image>().sprite;
        SlotBehaviour.me.ghost.SetActive(true);

    }
    public void Drag(EventTrigger et)
    {
       
        currentType = EventTriggerType.Drag;

        SlotBehaviour.me.ghost.transform.position = BaseFunc.GetMousePos();
        SlotUI slot = SlotBehaviour.me.ghost.GetComponent<SlotUI>();
        SlotUI sl = et.GetComponent<SlotUI>();
        slot.slot = sl.slot;
        slot.typeVision = sl.typeVision;



    }
    public RayUIObjectAndComponent GetRayParent(GameObject obj)
    {
        MagIntegerField input = (MagIntegerField)BaseFunc.GetParentComponent(typeof(MagIntegerField), obj);
        if (input != null) return new(input.gameObject, input);
        return null;
    }
    public void EndDrag(EventTrigger et)
    {
       
        currentType = EventTriggerType.EndDrag;

        SlotBehaviour.me.Gim.sprite = null;
        SlotBehaviour.me.ghost.SetActive(false);
        GameObject obj = SlotBehaviour.me.RayCast(BaseFunc.GetMousePos());
        if (obj != null)
        {
            RayUIObjectAndComponent rayUI = GetRayParent(obj);
            if (rayUI != null)
            {
                switch (rayUI.type)
                {
                    case TypeRUIOAC.field:
                        if (MagIntegerField.mif != null)
                        {
                            SlotUI sl = et.GetComponent<SlotUI>();

                            if (!sl.free)
                            {
                                MagIntegerField.mif.AddSymbol(sl.slot.item.value + "", true, sl.slot);
                                sl.slot.count--;

                                AutoDestroy();
                            }

                        }
                        break;
                }
               
            }
        }
        else
        {
            GameObject itemobj = SafeGameObjects.Load(slot.item.name, IItemIteraction.Features(slot.item.iii));
            itemobj.transform.position = Player.me.transform.position;
            Inventory.RemoveItem(slot.item);
        }
        // et.transform.position = SlotBehaviour.me.ghost.transform.position;

    }
    public void Drop()
    {
      
        currentType = EventTriggerType.Drop;
    }
    private void OnDestroy()
    {
        SlotBehaviour.me.CetRemvoe(cet);
    }
    public void AutoDestroy()
    {
        if (slot.count < 1)
            if (slot != null)
            {

                Destroy(gameObject);
            }
            else Destroy(gameObject);
    }
    public class RayUIObjectAndComponent
    {
        public GameObject obj;
        public MagIntegerField mif;
        public TypeRUIOAC type;
        public RayUIObjectAndComponent(GameObject obj, MagIntegerField mif)
        {
            this.obj = obj;
            this.mif = mif;
            type = TypeRUIOAC.field;
        }

    }
    public enum TypeRUIOAC
    {
        field
    }
    void Update()
    {
        if (!free)
        {
            switch (typeVision)
            {
                case ItemType.Numeric:
                    sr.enabled = true;
                    tmpNum.text = slot.item.value + "";
                    tmpCou.text = slot.count + "";
                    tmpNum.alignment = TextAlignmentOptions.Center;

                    break;
                case ItemType.Item:
                    sr.enabled = true;
                    sr.sprite = slot.item.sprite;
                    tmpNum.text = slot.item.name + "";
                    tmpCou.text = slot.count + "";
                    tmpNum.alignment = TextAlignmentOptions.Bottom;






                    break;
            }
        }
    }
}
