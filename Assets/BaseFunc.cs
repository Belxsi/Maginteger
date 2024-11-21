using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFunc : MonoBehaviour
{
    public static bool Pause;
    public void Awake()
    {
        IsLoad = false;
        prefabs.Clear();
        
    }
    public static AudioClip GetAudioClipPrefab(string nam)
    {
        AllLoadPrefabs();
        return (AudioClip)prefabs[nam];
    }
    public static void GetOfNull<T>(ref T t, GameObject me)
    {
        if (t == null)
        {
            t = me.GetComponent<T>();
            return;
        }

        if (object.Equals(t.ToString(), "null"))
        {
            t = me.GetComponent<T>();
            return;
        }
    }
    public static bool SmoothStep(float from,float to,float d,out float result)
    {
        result = Mathf.SmoothStep(from, to, d);
        return Equals(result, to);
    }
    public static void SetPause(bool trfl)
    {
        if (trfl)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        Pause = trfl;
    }

    public static bool IsLoad = false;
    public static Sprite ImportSprite(string name)
    {
        Texture tex = (Texture)PrefabLoader.Load("Prefabs", name);
        return Sprite.Create((Texture2D)tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }
    public static Dictionary<string, object> prefabs = new();
    public static Color SetColorA(Color basecolor,float a)
    {
        return new(basecolor.r, basecolor.g, basecolor.b, a);
    }
    public static void AddPrefab(string nam, string path = "")
    {
        if (path != "")
        {
            prefabs.Add(nam, PrefabLoader.Load("Prefabs" + "\\" + path, nam));
        }
        else
        {
            prefabs.Add(nam, PrefabLoader.Load("Prefabs", nam));
        }
    }
    public static T SearchList<T,B>(List<B> list) where T:B
    {
        return (T)list.Find(x => x.GetType() == typeof(T));
    }
    public static Vector2 GetPlayerFireDir()
    {
        if (Player.TryGetPlayer())
            return -DirPointOfMouse(GetScreenPos(Player.me.transform.position));
        return Vector2.zero;
    }
    public static GameObject GetPrefab(string nam)
    {
        AllLoadPrefabs();
        return (GameObject)prefabs[nam];
    }
    public static void VisualDamage(float damage,Vector2 pos)
    {
        GameObject obj = Instantiate(GetPrefab("DamageNum"), pos, Quaternion.identity);
        DamageVisuality dv = obj.GetComponent<DamageVisuality>();
        damage = Mathf.Round(damage);
        dv.Init( damage.ToString());
    }
    public static BodyObject GetEnemyCollision(CurrentTriggerCollision ctc,ref int state,Element Creater)       
    {
        BodyObject bo=null;
        if (ctc.state != state)
        {
            CurrentTriggerCollision.SearchInfo info = ctc.SearchBO(0);
            bo = info.bo;

           

            if (bo == Creater)
            {
                

                    bo = ctc.SearchBO(info.index + 1).bo;
                

            }
            else
            {
                if(bo!=null)
                state = ctc.state;
            }

        }
        return bo;
    }
    public static T GetScriptableObject<T>(string nam)
    {
        AllLoadPrefabs();
        return (T)prefabs[nam];
    }
    public static Sprite GetSpritePrefab(string nam)
    {
        AllLoadPrefabs();

        Texture tex = (Texture)prefabs[nam];
        return Sprite.Create((Texture2D)tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }
    public static TextAsset GetTextPrefab(string nam)
    {
        AllLoadPrefabs();
        object v = prefabs[nam];
        return (TextAsset)v;
        
    }
    protected static void AllLoadPrefabs()
    {
        if (!IsLoad)
        {
            IsLoad = true;
            AddPrefab("CardField");
            AddPrefab("SlotUI");
            AddPrefab("Energy");
            AddPrefab("CircleMagic");
            AddPrefab("NPCBars");
            AddPrefab("Grave");
            AddPrefab("Square");
            AddPrefab("IntroText");
            AddPrefab("Room");
            AddPrefab("RoomEmpty");
            AddPrefab("Tonel");
            AddPrefab("RockRoom", "RoomVariants");
            AddPrefab("ForestRoom", "RoomVariants");
            AddPrefab("Win");
            AddPrefab("NPC");
            AddPrefab("DamageNum");
            AddPrefab("BaseGL");
            AddPrefab("MagicScroll","Items");
        }
    }
    public static object GetParentComponent(Type type, GameObject obj)
    {
        if (obj != null)
        {
            if (obj.GetComponent(type) != null)
            {
                return obj.GetComponent(type);
            }
            else
            {
                if (obj.transform.parent != null)
                    return GetParentComponent(type, obj.transform.parent.gameObject);
                return null;
            }
        }
        else return null;
    }
    public static Vector3 GetScreenPos(Vector3 pos)
    {
        return Camera.main.WorldToScreenPoint(pos);
    }
    public static Vector3 GetWorldPos(Vector2 pos)
    {
        return Camera.main.ScreenToWorldPoint(pos);
    }
    public static Vector3 DirPointOfMouse(Vector3 point)
    {

        return (point - GetMousePos()).normalized;
    }
    public static Vector3 RoundVector(Vector3 vector)
    {
        return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
    }
    public static Vector3 AbsVector3(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }
    public static Vector3 MaxedVector(Vector3 bvector)
    {
        Vector3 vector = AbsVector3(bvector);
        if ((vector.x > vector.y) & (vector.x > vector.z))
        {
            return Vector3.right * bvector.x;
        }
        if ((vector.y > vector.x) & (vector.y > vector.z))
        {
            return Vector3.up * bvector.y;
        }
        if ((vector.z > vector.x) & (vector.z > vector.y))
        {
            return Vector3.forward * bvector.y;
        }
        return Vector3.zero;
    }
    public static bool AutoRealizeComponent(Type type, object com, out object ret_com, GameObject obj)
    {
        ret_com = com;
        if (com == null)
        {
            com = obj.GetComponent(type);
            if (com == null)
            {
                com = obj.AddComponent(type);
            }
        }
        if (com != null)
        {
            ret_com = com;
            return true;
        }
        return false;
    }
    public static bool AutoGetComponent(Type type, object com, out object ret_com, GameObject obj)
    {
        ret_com = com;
        if (com == null)
        {
            com = obj.GetComponent(type);

        }
        if (com != null)
        {
            ret_com = com;
            return true;
        }
        return false;
    }
    public static Vector3 GetMousePos()
    {
        return Input.mousePosition;
    }

}
public class BoxPacker<T>
{
    public T value;
    public bool SetValue(T value)
    {
        bool result = !object.Equals(this.value, value);
        if (result)
        {
            this.value = value;
        }
        return result;
    }
}
public abstract class OneSet<T>
{
    public T value;
    public bool setted;
    public abstract bool Set(T value);
    public OneSet(T value)
    {
        this.value = value;
    }
    public void ReSet(T value)
    {
        if (!this.value.Equals(value))
        {
            this.value = value;
            setted = false;
        }
    }
    public bool One()
    {
        if (!setted)
        {
            setted = true;
            return true;
        }
        else
        {
            return false;
        }
    }

}
public class OneSetAction : OneSet<Action>
{
    public OneSetAction(Action value) : base(value)
    {
        this.value = value;
    }
   
   
    public void Set()
    {
        if(One())
        value.Invoke();
    }

    public override bool Set(Action value)
    {
        throw new NotImplementedException();
    }
}
public class OneSetObject<T> : OneSet<T>
{
    public OneSetObject(T value) : base(value)
    {
        this.value = value;
        
    }
    public override bool Set(T value)
    {
        ReSet(value);
        if (One())
        {
            return true;
        }
        else return false;
    }
}
public class OneSetIEnumerator : OneSet<IEnumerator>
{
    MonoBehaviour activator;
    public OneSetIEnumerator(IEnumerator value,MonoBehaviour activator) : base(value)
    {
        this.value = value;
        this.activator = activator;
    }

    
    public void Set()
    {
        if (One())
            activator.StartCoroutine(value);
            
            
    }

    public override bool Set(IEnumerator value)
    {
        throw new NotImplementedException();
    }
}