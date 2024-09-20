using UnityEngine;

public class FakePlayerTarget : MonoBehaviour
{
    
    void Start()
    {
        Player.me = new();
        Player.me.parameters = new(100,100,100,100);
    }

    // Update is called once per frame
    
}
