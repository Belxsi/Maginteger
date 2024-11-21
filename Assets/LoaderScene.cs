using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoaderScene : MonoBehaviour
{
    public string namescene;
    public bool load;
    
    public static List<Progress> progresss=new();
    void Start()
    {
        
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public static Progress SearchHash(string hash)
    {
        return progresss.Find(x => x.Item4 == hash);
    }
    public static bool TrySearchHash(string hash,out Progress result)
    {
        result= progresss.Find(x => x.Item4 == hash);
        return result != null;
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
    public static IEnumerator AsuncLoadScene(string nam,string start,string hash,LoadSceneMode mode=LoadSceneMode.Single)
    {
       
        AsyncOperation ao= SceneManager.LoadSceneAsync(nam,mode);
        ao.allowSceneActivation = false;
        Progress progress =new();
        progress.Item1 = true;
        progress.Item2 = ao.progress;
        progress.Item3 = ao.isDone;
        progress.Item4 = hash;
        progresss.Add(progress);
        while (ao.progress<0.9f)
        {

            progress.Item1 = true;
            progress.Item2 = ao.progress;
            progress.Item3 = ao.isDone;
            progress.Item4 = hash;
            yield return new WaitForSeconds(0);

        }
        progress.Item1 = true;
        progress.Item2 = ao.progress;
        progress.Item3 = ao.isDone;
        progress.Item4 = hash;
        SceneManager.UnloadSceneAsync(start);
       
        ao.allowSceneActivation = true;
        
        progresss.Remove(progress);
        
    }
    public static IEnumerator AsuncNotLoadScene(string nam, string hash,BoxPacker<bool> load, LoadSceneMode mode = LoadSceneMode.Single)
    {

        AsyncOperation ao = SceneManager.LoadSceneAsync(nam, mode);
        ao.allowSceneActivation = false;
        Progress progress = new();

        progresss.Add(progress);
        while (ao.progress < 0.9f)
        {

            progress.Item1 = true;
            progress.Item2 = ao.progress;
            progress.Item3 = ao.isDone;
            progress.Item4 = hash;
            yield return new WaitForSeconds(0);

        }
        progress.Item1 = true;
        progress.Item2 = ao.progress;
        progress.Item3 = ao.isDone;
        progress.Item4 = hash;
       
        while (true)
        {
            ao.allowSceneActivation = load.value;
            yield return new WaitForSeconds(0);
            if (load.value) break;
        }

        //progresss.Remove(progress);

    }


}
public class Progress
{
    public bool Item1;
    public float Item2;
    public bool Item3;
    public string Item4;

}
