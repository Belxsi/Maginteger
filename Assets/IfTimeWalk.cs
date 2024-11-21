using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class IfTimeWalk : MonoBehaviour
{
    public GameObject parent;
    public Exposuring exposuring;
    public float speed;
    public SceneMoveTimeOut smto;
    public IEnumerator StartGen()
    {
        yield return new WaitForSeconds(0.1f);
        GLParameters glp = BaseFunc.GetScriptableObject<GLParameters>("BaseGL");
        DontDestroyMono.mono.aos = new List<ProgressVisitor>();
        DontDestroyMono.mono.StartCoroutine(GeneratorLevel.CreateWorld(DontDestroyMono.mono, DontDestroyMono.mono.aos, glp));
    }
    void Start()
    {
       StartCoroutine( StartGen());
       
    }
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            smto.work = false;
            if (!LoaderScene.TrySearchHash(smto.nameScene, out Progress progress))
            {
                smto.LoadScene();
                
            }
        }

        parent.SetActive(true);
        new OneSetIEnumerator(exposuring.Blacking(speed), this).Set();
        new OneSetIEnumerator(exposuring.Viging(-speed), this).Set();
       

    }
}
