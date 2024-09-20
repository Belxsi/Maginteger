using System.Collections;
using UnityEngine;

public class ItemStone : IItemIteraction
{
    public override void Click()
    {

    }

    public override void Use(Element Creater)
    {

        ThrowObject(Creater, 1);
        StartCoroutine(RepeatUse());


    }
    public static bool Equals(ItemStone A,ItemStone B)
    {
        return true;
    }
    public override IEnumerator RepeatUse()
    {
        Vector2 dir = BaseFunc.GetPlayerFireDir();
        PhysicMove pm = GetComponent<PhysicMove>();
        float s = 10;
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            s -= Time.deltaTime;
            if (s <= 0)
            {
                Destroy(gameObject);
                break;
            }
            pm.Move(dir * 10);

        }
    }

    public override string ToStringInfo()
    {
        return           
            "Просто камень, который можно кинуть.";
    }
}
