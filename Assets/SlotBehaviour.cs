using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class SlotBehaviour : MonoBehaviour
{
   
    public GameObject ghost;
    public Image Gim;
    public EventSystem es;
    public GraphicRaycaster gr;
    public List<CustomEventTrigger> CET=new();
    public static SlotBehaviour me;
    public void Awake()
    {
        me = this;
    }
    public void CetAdd(CustomEventTrigger cet)
    {
        CET.Add(cet);
    }
    public void CetRemvoe(CustomEventTrigger cet)
    {
        CET.Remove(cet);
    }
    public GameObject RayCast(Vector2 pos)
    {
        PointerEventData ped = new(es);
        ped.position = pos;
        List<RaycastResult> result = new();
        gr.Raycast(ped, result);
        if (result.Count > 0)
        {
            return result[0].gameObject;
        }
        else return null;
    }
    public void Update()
    {
        /*
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            SlotUI slotUI = RayCast(Input.mousePosition).GetComponent<SlotUI>();
            slotUI.cet.OnPointerClick(new PointerEventData(es));
        }
        */
    }

}
public interface CustomClick
{
    void OnPointerEnter(PointerEventData eventData);
}
