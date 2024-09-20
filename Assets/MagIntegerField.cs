using System.Collections.Generic;
using UnityEngine;
public class MagIntegerField : MonoBehaviour
{
    public CustomInputField inputField;
    public string buffertext;
    public GameObject Content;
    public List<ButtonSlot> bss = new();
    public static MagIntegerField mif;
    public string startText;
    public InterfaceAudioEffect iae;
    public string BSStoText()
    {
        string sum = "";
        for (int i = 0; i < bss.Count; i++)
        {
            sum += bss[i].value;

        }
        return sum;


    }
    public void Clear()
    {
        inputField.caretPosition = 1;
        for (int i = 0; i < bss.Count; i++)
        {
            if (bss[i] != null)
            {
                Destroy(bss[i].gameObject);
                
            }
        }
        bss.Clear();
    }
    public CreaterMagic cm;
    public void Update()
    {

        if(!inputField.select)
            if(Player.TryGetPlayer())
            if(Player.me.Hand_IO==null)
        if (cm != null)
        {
            if (!cm.drag)
                if (Inventory.open == false)
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                        if (SlotBehaviour.me.RayCast(BaseFunc.GetMousePos()) == null)
                            if (Player.TryGetPlayer())
                            {
                                cm = Player.me.GetComponent<CreaterMagic>();
                                if (cm.FomritMagic(BSStoText(), Player.me, cm,out Energy energy))
                                {
                                    Player.me.energies.Add(energy);
                                    Player.me.npcAE.AudioAwake(Player.me.npcAE.concentrat, false);
                                }


                            }

            if (cm.drag)
                if (Input.GetKeyUp(KeyCode.Mouse0))
                    if (Player.TryGetPlayer())
                    {
                        cm.mattery.magic.dir = -BaseFunc.DirPointOfMouse(BaseFunc.GetScreenPos(Player.me.transform.position));
                        cm.Fire(cm);
                        Player.me.npcAE.AudioStop(Player.me.npcAE.concentrat);
                        Player.me.npcAE.AudioAwake(Player.me.npcAE.fire, false, true);
                        cm.mattery = null;
                    }
        }
        else if (Player.TryGetPlayer()) cm = Player.me.GetComponent<CreaterMagic>();


    }
    public void Awake()
    {
        Init();

        SetText(startText);
    }
    public void Init()
    {
        mif = this;
    }
    public void SetText(string text)
    {
        Clear();
        text = text.ToUpper();
        for (int i = 0; i < text.Length; i++)
        {
            AddSymbol(text[i] + "", false, null);
        }
        inputField.caretPosition = bss.Count / 2;
    }
    public void AddSymbol(string symbol, bool card, Slot slot)
    {

        ButtonSlot bs = Instantiate(BaseFunc.GetPrefab("CardField"), Content.transform).GetComponent<ButtonSlot>();
        if (card == false)
        {
            bs.Init(card, symbol + "");
        }
        else
        {
            bs.Init(card, symbol + "", slot);
        }
        iae.AudioAwakeDefault(iae.write);
        bs.slot = slot;
        bs.Action();
        bss.Insert(inputField.caretPosition - 1, bs);
        inputField.caretPosition++;
        //  Content.transform.
        mif.ReIndex();

    }
    public void ReIndex()
    {
        for (int i = 0; i < bss.Count; i++)
        {
            bss[i].index = i;
        }
    }
    public void RemoveSymbol()
    {
        mif.ReIndex();
        if (bss.Count > 0)
        {
            ButtonSlot bs = bss[inputField.caretPosition - 2];
            bss.Remove(bs);
            bs.AutoDestroy(bs.slot);
            inputField.caretPosition--;
            mif.ReIndex();
            iae.AudioAwakeDefault(iae.unwrite);
        }
    }

}
