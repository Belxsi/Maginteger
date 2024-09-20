using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemTakeLogic : MonoBehaviour
{
    public TextMeshProUGUI help, nameitem;
    public static float minDistVisible = 2;
    public ItemObject io;
    public float pocas = 2;
    public SortingGroup sg;
    public void DistVisible(bool trfl, float d)
    {
        if (trfl)
        {
            float r = (minDistVisible - d) / minDistVisible;
            help.gameObject.SetActive(true);
            nameitem.gameObject.SetActive(true);
            help.color = BaseFunc.SetColorA(help.color, r);
            nameitem.color = BaseFunc.SetColorA(nameitem.color, Mathf.Pow(r, 1 / pocas));

        }
        else
        {
            help.gameObject.SetActive(false);
            nameitem.gameObject.SetActive(false);
        }

    }
    public void ObserveTake()
    {
        if (Player.TryGetPlayer())
        {
            float d = Vector2.Distance(transform.position, Player.me.transform.position);
            bool trfl = d < minDistVisible;
            DistVisible(trfl, d);
            if (d < minDistVisible)
            {
                if (!CustomInputField.me.select)
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        Player.me.TakeItem(io);
                        DistVisible(false, 0);
                    }
                    else
                    {
                        if (Input.GetKeyUp(KeyCode.I))
                        {
                            Player.me.PutInBug(io);
                        }
                    }
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!io.isHanded)
        {
            ObserveTake();
            sg.sortingOrder = 55;
        }
        else
        {
            sg.sortingOrder = 58;
        }
    }
}
