using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourExCollider : MonoBehaviour
{
    public TriggerExenCollider input,output;
    public int levelUp,levelDown;
    public Vector3 dir;
    public string value;
    void Awake()
    {
        input.typeEx = TypeExCollider.Input;
        output.typeEx = TypeExCollider.Output;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.TryGetPlayer())
        if (value.Length >= 2)
        {
            switch (value)
            {
                case "IO":
                    LevelColliders.SetLevel(levelUp);
                    
                    Player.me.transform.position += dir;
                    break;
                case "OI":
                    LevelColliders.SetLevel(levelDown);
                    Player.me.transform.position -= dir;
                    break;
            }
            value = "";
        }
    }
}
