using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoader 
{
    public static Object Load(string Folder,string nam)
    {
        Object obj;
        string path= Folder +"\\"+ nam;
        obj = Resources.Load(path);
        

        return obj;
    }
}
