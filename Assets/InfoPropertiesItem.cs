using TMPro;
using UnityEngine;

public class InfoPropertiesItem : MonoBehaviour
{
    public static InfoPropertiesItem me;
    public GameObject panel;
    public TextMeshProUGUI infotmp,nameitem;
    public float maxdeltaSize;
    void Awake()
    {
        me = this;
    }

   
    public void Show(string info,string nam,SlotUI slot)
    {
        panel.SetActive(true);
        panel.transform.position = slot.transform.position;
        infotmp.text = info;
        nameitem.text = nam;
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.sizeDelta= new Vector2(rect.sizeDelta.x, 400 + info.Length / maxdeltaSize);

    }
    public void Hide()
    {
        panel.SetActive(false);
    }
}
