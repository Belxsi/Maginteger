using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoaderScene : MonoBehaviour
{
    public string namescene;
    public bool load;
    
    public static (bool,float, bool ) progress;
    void Start()
    {
        
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
    // Update is called once per frame
    void Update()
    {
        if (load)
        {
            load = false;
            SceneManager.LoadScene(namescene);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ReloadScene();
        }
    }
    public static IEnumerator AsuncLoadScene(string nam)
    {
        AsyncOperation ao= SceneManager.LoadSceneAsync(nam);
        ao.allowSceneActivation = false;
        while (ao.progress>=0.9f)
        {

            progress.Item1 = true;
            progress.Item2 = ao.progress;
            progress.Item3 = ao.isDone;
            yield return new WaitForSeconds(0);

        }
        progress.Item1 = true;
        progress.Item2 = ao.progress;
        progress.Item3 = ao.isDone;
        ao.allowSceneActivation = true;
    }

    
}
