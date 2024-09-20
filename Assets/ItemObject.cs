using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public bool isHanded;
    public IItemIteraction itemInteractive;
    public Item item;
    public SpriteRenderer spriteRenderer;
    public ItemTakeLogic itl;
    void Update()
    {
        spriteRenderer.sprite = item.sprite;


        if (isHanded)
        {
            if (!CustomInputField.me.select)
                if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(Player.TryGetPlayer())
                itemInteractive.Use(Player.me);
            }
        }
        else
        {
            itl.help.text = "Нажмите E чтобы подобрать предмет";
            itl.nameitem.text = item.name;
           
        }
    }
}
