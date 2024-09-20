using UnityEngine;
using UnityEngine.UI;

public class CustomInputField : MonoBehaviour
{
    public string AllBut = "qwertyuiopasdfghjklzxcvbnm-+*/,=(){}[]. ;";
    public int caretPosition = 1, virtualcaretpositon = 1;
    public MagIntegerField mif;
    public bool select;
    public GameObject Caret;
    public Transform offsetCaret;
    public Image image;
    public float DeltaPas;
    public CustomScrollRect scrollbar;
    public Animator animator;
    public int scrolldelta;
    public static CustomInputField me;
    void Awake()
    {
        me = this;
    }
    public void OnSelect()
    {
        select = true;
    }
    public void OffSelect()
    {
        // if(Input.GetKey(KeyCode.Mouse0))
        select = false;
    }
    public void InBounds()
    {
        if (mif.bss.Count > 0)
            if (caretPosition >= mif.bss.Count)
            {
                caretPosition = mif.bss.Count - 1;
            }
    }
    public bool IsButton(char c)
    {
        for (int i = 0; i < AllBut.Length; i++)
        {
            if (c == AllBut[i])
            {
                return true;
            }
        }
        return false;
    }
    public void ViewCaret()
    {

        if (mif.bss.Count != 0)
        {




            scrollbar.ViewControl((caretPosition - 1) - scrolldelta, -DeltaPas);
        }

    }
    public GameObject GetRaycast()
    {
        return SlotBehaviour.me.RayCast(BaseFunc.GetMousePos());
    }
    void Update()
    {
        ViewCaret();
        //Event.KeyboardEvent("W");
        if (SlotBehaviour.me != null)
        {
            if (mif.bss.Count > 0)
                if (virtualcaretpositon > 1 & virtualcaretpositon < mif.bss.Count + 2)
                {

                    Caret.transform.position = mif.bss[virtualcaretpositon - 2].transform.position;
                }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject obj = GetRaycast();
                InfoPropertiesItem.me.Hide();
                if (obj == null)
                {
                    select = false;
                }
                else
                {
                    if (!obj.CompareTag("Field"))
                    {
                        select = false;
                    }
                    else select = true;
                }
            }
            //Debug.Log(Console.ReadLine());
            //EventSystem.current.currentInputModule.input.
            //  Debug.Log(Event.current.keyCode);
            Caret.SetActive(select);

            if (select)
            {
                animator.SetTrigger("Open");
                animator.ResetTrigger("Close");
                // image.color = Color.white * 1;
                if (Input.anyKeyDown)
                {

                    string symbol = Input.inputString;

                    if (symbol.Length > 0)
                        if (IsButton(symbol[0]))
                        {

                            mif.AddSymbol(symbol[0].ToString().ToUpper(), false, null);
                            if (mif.bss.Count != 0)
                            {
                                ViewCaret();
                            }
                        }
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        mif.RemoveSymbol();
                        virtualcaretpositon = caretPosition;
                        ViewCaret();
                    }






                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    GameObject obj = GetRaycast();
                    if (obj != null)
                    {
                        
                        if (obj.GetComponent<ButtonSlot>())
                        {

                            ButtonSlot bs = obj.GetComponent<ButtonSlot>();

                            caretPosition = 1 + (bs.index + 1);
                            virtualcaretpositon = caretPosition;
                            ViewCaret();
                            virtualcaretpositon = 1 + (bs.index + 1);
                        }
                      
                       
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            if (obj.GetComponent<MagIntegerField>())
                            {
                                caretPosition = 2;
                                virtualcaretpositon = caretPosition;
                            }
                        }
                    }

                    // InBounds();
                }
            }
            else
            {
                animator.SetTrigger("Close");
                animator.ResetTrigger("Open");
                //image.color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        }
    }
}
