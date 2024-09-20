using UnityEngine;
using TMPro;
using System.Collections;

public class DamageVisuality : MonoBehaviour
{
    public TextMeshPro txt;
    public void Init(string text)
    {
        txt.text = text;
        StartCoroutine(AutoDie(1f));
    }

    public IEnumerator AutoDie(float ofSecond)
    {
        float current = ofSecond;
        while (current>0)
        {
            current -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            txt.color = new(1, 0, 0,Mathf.Clamp01( current / ofSecond)*2);

        }
        Destroy(gameObject);
    }

}
