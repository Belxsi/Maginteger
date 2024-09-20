using UnityEngine;

public class LogoLogic : MonoBehaviour
{
    public float time;
    float timeout;
    void Start()
    {
        timeout = time;
    }

    
    void Update()
    {
        timeout -= Time.deltaTime;
        if ((timeout <= 0)&(!LoaderScene.progress.Item1))

        {
            timeout = time;
            StartCoroutine(LoaderScene.AsuncLoadScene("Menu"));
        }
        Debug.Log(LoaderScene.progress.ToString());
    }
}
