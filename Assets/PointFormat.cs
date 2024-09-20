using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PointFormat : MonoBehaviour
{
  
    public static float Parse(string s)
    {
        string sum = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '.')
            {
                sum += ",";
            }
            else
            {
                sum += s[i];
            }
        }
        return float.Parse(sum);
    }
    public static string UnParse(string s)
    {
        string sum = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == ',')
            {
                sum += ".";
            }
            else
            {
                sum += s[i];
            }
        }
        return sum;
    }
    public static bool TryParse(string s,out float result)
    {
        string sum = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '.')
            {
                sum += ",";
            }
            else
            {
                sum += s[i];
            }
        }
        return float.TryParse(sum, out result);
    }
    void Start()
    {
     

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
