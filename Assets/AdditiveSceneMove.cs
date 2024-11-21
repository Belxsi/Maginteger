using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AdditiveSceneMove : BaseSMTO
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<string> nameScenes = new();
    public override IEnumerator UpdateScener()
    {
        while (timeout > 0)
        {
            timeout -= Time.deltaTime;
            foreach (var scene in nameScenes)
            {
                if (LoaderScene.TrySearchHash(scene, out Progress progress)) { }

                if ((timeout <= 0) & (!progress.Item1))

                {
                  

                    StartCoroutine(LoaderScene.AsuncLoadScene(scene,SceneManager.GetActiveScene().name, scene, LoadSceneMode.Additive));
                }

            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
       
           

        
    }
   
}

