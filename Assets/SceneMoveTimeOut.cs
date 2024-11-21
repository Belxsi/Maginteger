using System.Collections;
using UnityEngine;

public class SceneMoveTimeOut : BaseSMTO
{





}
public abstract class BaseSMTO : MonoBehaviour
{
    public float time;
    public float timeout;

    public string nameScene;
    public bool work = true;
    public void OnActive()
    {
        work = true;
        timeout = time;
    }
    public virtual void StartScener()
    {
        timeout = time;
    }
    public virtual void LoadScene()
    {
        StartCoroutine(LoaderScene.AsuncLoadScene(nameScene, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, nameScene));

    }
    public virtual IEnumerator UpdateScener()
    {
        while (timeout > 0)
        {
            timeout -= Time.deltaTime;
            if (LoaderScene.TrySearchHash(nameScene, out Progress progress))
            {

                if ((timeout <= 0) & (!progress.Item1))

                {
                    LoadScene();
                }
            }
            else
            {
                if (timeout <= 0)

                {

                    LoadScene();
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }



    }
    public virtual void Start()
    {
        StartScener();
    }


    public virtual void Update()
    {
        if (work)
        {
            StartCoroutine(UpdateScener());
            work = false;
        }
    }
}
