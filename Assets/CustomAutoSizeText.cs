using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CustomAutoSizeText : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public float offsetSize;

    // Update is called once per frame
    void Update()
    {
        if(tmp.text.Length!=0)
        tmp.fontSize = offsetSize / tmp.text.Length;
    }
}
