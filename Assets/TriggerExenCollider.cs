using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExenCollider : MonoBehaviour
{
    public TypeExCollider typeEx;
    public BehaviourExCollider behaviour;

    public void OnTriggerExit2D(Collider2D col)
    {
        if (typeEx != TypeExCollider.NoValue)
        {
            if (col.name == "Player")
                if ((behaviour.value == "") || (behaviour.value != typeEx.ToString()[0] + ""))
                    behaviour.value += typeEx.ToString()[0];
        }
        else
        {
            if (col.name == "Player")
                behaviour.value = "";
        }
    }

}
public enum TypeExCollider
{
    Input,
    Output,
    NoValue
}
