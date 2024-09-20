using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class LevelColliders : MonoBehaviour
{
    public List<LevelCollider> public_levcol = new();
    public static List<LevelCollider> levcol=new();
    public static LevelCollider currentLevel;
    public void Awake()
    {
        levcol = public_levcol;
        SetLevel(1);
    }
    public static void SetLevel(int level)
    {

        LevelCollider lc = SearchAtLevel(level);
        if (lc != null)
        {
            currentLevel = lc;

        }
        else currentLevel = null;
    }
    public static LevelCollider SearchAtLevel(int level)
    {
        LevelCollider result = null;
       for(int i = 0; i < levcol.Count; i++)
        {
            if (levcol[i].level == level)
            {
                result = levcol[i];
                result.collider.enabled = true;
                result.NavMap.SetActive(true);
            }
            else
            {
                levcol[i].collider.enabled = false;
                levcol[i].NavMap.SetActive(false);
            }
        }
        return result;
    }
   
  
}
[Serializable]
public class LevelCollider{

    public TilemapCollider2D collider;
    public int level;
    public GameObject NavMap;
}
