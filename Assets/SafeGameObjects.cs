using System.Collections.Generic;
using UnityEngine;
public class SafeGameObjects : MonoBehaviour
{
    public static SafeGameObjects me;
    public static Dictionary<string, SafeElement> safe;
    public static bool inited;
    public static void Save(GameObject obj, string nam,string features)
    {
        obj.transform.SetParent(me.transform);
        nam += features;
        obj.SetActive(false);
        DontDestroyOnLoad(obj);
        if (safe.TryAdd(nam, new(obj, nam)))
        {

        }
        else
        {
            
            if (safe.TryGetValue(nam, out SafeElement el))
            {

                el.Add(obj);
            }
        }

    }
    public static void Clear()
    {
        foreach(var item in safe)
        {
            if (!item.Value.dontclear)
            {
                foreach (var s in item.Value.objs)
                {
                    Destroy(s);
                }
                safe.Remove(item.Key);
            }
        }
       
    }
    public static GameObject Load(string nam,string features)
    {

        nam += features;
        if (safe.TryGetValue(nam, out SafeElement el))
        {
            if (el.Sub(out GameObject obj))
            {
                obj.transform.SetParent(GameplayPublicField.BigFather());
                obj.SetActive(true);
                return obj;
            }
        }
        return null;




    }
    public void Start()
    {
        if (!inited)
        {
            me = this;
            safe = new();
            inited = true;
           
            DontDestroyOnLoad(gameObject);
        }else
        {
          //  Clear();
            Destroy(gameObject);
        }

    }
}
public class SafeElement
{
    public List<GameObject> objs=new();
    public int count;
    public string name;
    public bool dontclear;
    public SafeElement(GameObject obj, string name)
    {
        objs.Add(obj);
        count = 1;
        this.name = name;
    }
    public void Add(GameObject obj)
    {
        count++;
        objs.Add(obj);
    }
    public bool Sub(out GameObject obj)
    {
        obj = objs[^1];
        count--;
        objs.Remove(objs[^1]);
        if (count == 0)
        {
            SafeGameObjects.safe.Remove(name);
            
        }
        if(count<0) return false;
        return true;
    }
}
