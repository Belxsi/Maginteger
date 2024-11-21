using UnityEngine;
using System.Collections.Generic;
public class DontDestroyMono : MonoBehaviour
{
    public static DontDestroyMono mono;
    public List<ProgressVisitor> aos;
    void Start()
    {
        mono = this;
        DontDestroyOnLoad(gameObject);
    }
    

    // Update is called once per frame

}
