using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.AI;
public class GeneratorLevel : MonoBehaviour
{
    
    public static List<GameObject> WorldObject=new();
   
   
    public bool create;
   
    
    public GLParameters par;
    public static bool navmeshgen, mapbuild, generation;
    public static float randomskeep;
    public static DonwoloaderTextProgresser _dtp;
    public DonwoloaderTextProgresser dtp;
    public static List<ProgressVisitor> _aos;
    public static MapLevel _mapLevel;
    public static void Clear()
    {
        for (int i = 0; i < WorldObject.Count; i++)
        {
            Destroy(WorldObject[i]);
        }
        WorldObject.Clear();
    }
    public void Awake()
    {
        //  StartCoroutine(RepeatCreate());
        if (!generation)
        {
            mapbuild = false;
            generation = false;
            randomskeep = 0;
           
        }
        SetDTP(dtp);
    }
    public static bool Skeep()
    {
       return UnityEngine.Random.Range(0f, 1f) > randomskeep;
    }
   
    public static IEnumerator Visiting(List<ProgressVisitor> aos)
    {
        bool start = false;
        while (true)
        {
            yield return new WaitForSeconds(1);
           
            for(int i = 0; i < aos.Count; i++)
            {
               
                start = true;
                if (aos[i].IsDone)
                {
                   
                    aos.Remove(aos[i]);
                    
                }
            }
            if (aos.Count == 0&start) break;
            
        }
    }
    public static IEnumerator AOSetActualDTP()
    {
        while (generation)
        {
            
            
            if (_dtp != null)
            {
                _dtp.aos = _aos;
                _dtp.ml = _mapLevel;
                
            }
            yield return new WaitForSeconds(0);
        }
    }
    public static IEnumerator CreateWorld(MonoBehaviour mono, List<ProgressVisitor> pvs,GLParameters par)
    {
        _aos = pvs;
        
        generation = true;
        mono.StartCoroutine(AOSetActualDTP());
        Time.timeScale = 1;
        if (GameplayPublicField.me.CompareTag("Gameplay")) 
        GameplayPublicField.me.SetActive(false);
        if (_dtp != null)
        {
            _dtp.gameObject.SetActive(true);
            _dtp.aos = pvs;
        }
        Clear();
       
        MapLevel mapLevel = new MapLevel((int)par.sizefield, par.scaleperlin, par.lenghttonel, par.Perlin, par.gate);
        _mapLevel = mapLevel;
        if (_dtp != null)
        {
            _dtp.gameObject.SetActive(true);
            _dtp.aos = pvs;
            _dtp.ml = mapLevel;
        }
        mono.StartCoroutine(Visiting(pvs));
        yield return mapLevel.Generation(pvs);
        while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "TestingGenerationWorld")
        {
            yield return new WaitForSeconds(0);
        }
        ProgressVisitor realiz = new("Создание карты");
        pvs.Add(realiz);
        if (_dtp != null)
        {
            _dtp.gameObject.SetActive(true);
            _dtp.aos = pvs;
            _dtp.ml = mapLevel;
        }
        yield return Realization(realiz,mapLevel,par);
        mapbuild = true;
        Time.timeScale = 1;
        GameplayPublicField.me.SetActive(true);

        if (_dtp != null)
        {
            _dtp.gameObject.SetActive(true);
            _dtp.aos = pvs;
            _dtp.ml = mapLevel;
            _dtp.gameObject.SetActive(false);
        }
        generation = false;
    }

    public static Quaternion DirToRotation(Vector2Int dir)
    {

        switch (dir.ToString())
        {
            case "(1, 0)":
                return Quaternion.identity;
            case "(-1, 0)":
                return Quaternion.Euler(0, 0, 0);

            case "(0, 1)":
                return Quaternion.Euler(0, 0, 90);

            case "(0, -1)":
                return Quaternion.Euler(0, 0, -90);


        }
        return Quaternion.identity;
    }
    public static IEnumerator Realization(ProgressVisitor pv,MapLevel mapLevel,GLParameters par)
    {
        Vector2Int offset = mapLevel.first.pos * (int)par.scale;
        for (int x = 0; x < mapLevel.sizefield; x++)
            for (int y = 0; y < mapLevel.sizefield; y++)
            {
                pv.active = true;
                pv.progress = (par.sizefield * (float)x + y) / (par.sizefield *(float)par.sizefield);
                if (mapLevel.map[x, y] != null)
                {
                    GameObject obj = null ;
                    switch (mapLevel.map[x, y].type)
                    {
                        case "room":
                            
                            obj = Instantiate(BaseFunc.GetPrefab("RoomEmpty"), new Vector2(x * par.scale, y * par.scale)-offset, Quaternion.identity,GameplayPublicField.BigFather());
                            Room room=obj.GetComponent<Room>();
                            room.SetChunck(mapLevel.map[x, y]);
                            room.Init();
                            room.SetCurrentSubVariant();
                            break;
                        case "tonel":
                            obj = Instantiate(BaseFunc.GetPrefab("Tonel"), new Vector2(x * par.scale, y * par.scale) - offset, DirToRotation(mapLevel.map[x, y].dir), GameplayPublicField.BigFather());
                            Tonel tonel = obj.GetComponent<Tonel>();
                            tonel.SetChunck(mapLevel.map[x, y]);
                            tonel.Init();
                            break;
                    }
                    WorldObject.Add(obj);
                }
                if (GeneratorLevel.Skeep())
                    yield return null;
            }
        pv.IsDone = true;
    }
    // Update is called once per frame
    public IEnumerator RepeatCreate()
    {
        while (true)
        {
            create = true;
            yield return new WaitForSeconds(0.25f);
        }
    }
    public static void SetDTP(DonwoloaderTextProgresser dtp)
    {
        _dtp = dtp;
    }
    void Update()
    {

        Mattery.offsettimeout = par.timeConrol;
        Mattery.timeslowcast = par.tsc;
        
        if (create&!generation)
        {
            create = false;
            
            StartCoroutine( CreateWorld(this,new List<ProgressVisitor>(),par));

        }
        else
        {
           
            create = false;
        }
        if (generation)
        {
            if (GameplayPublicField.me.CompareTag("Gameplay"))
                GameplayPublicField.me.SetActive(false);
        }
        else
        {
            if (GameplayPublicField.me.CompareTag("Gameplay"))
                GameplayPublicField.me.SetActive(true);
        }
       
       
    }
    void LateUpdate()
    {
        if(mapbuild&!create)
        for (int i = 0; i < WorldObject.Count; i++)
        {
            Vector2 pos = WorldObject[i].transform.position;
            WorldObject[i].SetActive((Vector2.Distance(pos, Camera.main.transform.position) < par.minDistantLOD));



        }
    }
}
[Serializable]
public class PerlinParam
{
    public float o, l, p;

    public PerlinParam(float o, float l, float p)
    {
        this.o = o;
        this.l = l;
        this.p = p;
    }
}
public class MapLevel
{
    public MapChunck[,] map;
    public int sizefield;
    public float scale = 1;
    public float MaxLenghtTonel = 3;
    public int seedX, seedY;
    public PerlinParam pp;
    public float gate;

    public MapLevel(int sizefield, float scale, float maxLenghtTonel, PerlinParam pp, float gate)
    {

        this.sizefield = sizefield;
        this.scale = scale;
        MaxLenghtTonel = maxLenghtTonel;
        this.pp = pp;
        this.gate = gate;
        seedX = UnityEngine.Random.Range(-100, 100);
        seedY = UnityEngine.Random.Range(-100, 100);
        Init();
    }

    public void Init()
    {
        map = new MapChunck[sizefield, sizefield];

    }
    
    public IEnumerator Rooming(ProgressVisitor pv)
    {
        for (int x = 0; x < sizefield; x++)
            for (int y = 0; y < sizefield; y++)
            {
                pv.active = true;
                pv.progress = (sizefield * (float)x + y) / (sizefield * (float)sizefield);
                float xCoord = (float)x / sizefield * scale + seedX;
                float yCoord = (float)y / sizefield * scale + seedY;
                double Sample = new Perlin().NoiseOctaves(xCoord, yCoord, 0.5f, (int)pp.o, pp.l, pp.p);
                if (Sample > gate)
                    map[x, y] = new("room", new(x, y));
                if(GeneratorLevel.Skeep())
                yield return null;
            }
        pv.IsDone=true;
    }
    public int sum_rooms, sum_tonel;
    public IEnumerator Toneling(ProgressVisitor pv)
    {
        
        for (int x = 0; x < sizefield; x++)
            for (int y = 0; y < sizefield; y++)
            {
               
                if (map[x, y] != null)
                    if (map[x, y].type == "room")
                    {
                        if (Ray(new(x, y), new(1, 0)) || Ray(new(x, y), new(-1, 0)) || Ray(new(x, y), new(0, 1)) || Ray(new(x, y), new(0, -1)))
                        {
                            sum_rooms++;
                        }
                        else
                        {
                            map[x, y] = null;
                        }



                    }
                pv.active = true;
                pv.progress = (sizefield * (float)x + y) / (sizefield * (float)sizefield);
                if (GeneratorLevel.Skeep())
                    yield return null;
            }
        pv.IsDone = true;
    }
    public bool IsBound(Vector2Int pos)
    {
        return ((pos.x >= sizefield || pos.x < 0) || (pos.y >= sizefield || pos.y < 0));

    }
    public bool IsType(Vector2Int pos, string type)
    {
        if (!IsBound(pos))
        {
            if (map[pos.x, pos.y] != null)
                return map[pos.x, pos.y].type == type;
            return false;
        }
        return false;
    }
    public bool IsAroundType(Vector2Int pos, string type)
    {
        if (IsType(pos + new Vector2Int(1, 0), type))
            return true;
        if (IsType(pos + new Vector2Int(-1, 0), type))
            return true;
        if (IsType(pos + new Vector2Int(0, 1), type))
            return true;
        if (IsType(pos + new Vector2Int(0, -1), type))
            return true;
        return false;

    }
    public bool IsAroundType(Vector2Int pos, string[] types)
    {
        for (int i = 0; i < types.Length; i++)
        {
            if (IsType(pos + new Vector2Int(1, 0), types[i]))
                return true;
            if (IsType(pos + new Vector2Int(-1, 0), types[i]))
                return true;
            if (IsType(pos + new Vector2Int(0, 1), types[i]))
                return true;
            if (IsType(pos + new Vector2Int(0, -1), types[i]))
                return true;
        }
        return false;

    }

    public bool Ray(Vector2Int pos, Vector2Int dir)
    {
        List<Vector2Int> tonelPos = new();
        for (int i = 0; i < MaxLenghtTonel; i++)
        {
            Vector2Int nextpos = pos + dir * (i + 1);
            if (!IsBound(nextpos))
            {
                if (map[nextpos.x, nextpos.y] != null)
                {
                    if (map[nextpos.x, nextpos.y].type == "room")
                    {
                        
                        for (int c = 0; c < tonelPos.Count; c++)
                        {
                            map[tonelPos[c].x, tonelPos[c].y] = new("tonel", tonelPos[c], dir);
                            sum_tonel++;
                        }
                        if (tonelPos.Count > 0) return true;
                        return false;
                    }
                    else
                    {
                        if (map[nextpos.x, nextpos.y].type != "tonel")
                        {
                            if (!IsAroundType(nextpos, "tonel"))
                            {
                                tonelPos.Add(nextpos);
                            }
                            else return false;
                        }
                        else
                        {
                            return (Mathf.Abs(map[nextpos.x, nextpos.y].dir.x) == Mathf.Abs(dir.x));


                        }
                    }
                }
                else
                {


                    if (!IsAroundType(nextpos, "tonel"))
                    {
                        tonelPos.Add(nextpos);
                    }
                    else return false;

                }
            }
            else
            {
                return false;
            }
        }
        return false;

    }
    public void DeleteSingleRooms()
    {
        for (int x = 0; x < sizefield; x++)
            for (int y = 0; y < sizefield; y++)
                if (!IsAroundType(new(x, y), new string[] { "tonel", "room" }))
                {
                    map[x, y] = null;
                }
    }
    public MapChunck last,first;
    public int deepReflection;
    public void AddUDLR(MapChunck mc, Vector2 dir)
    {
        float u = mc.udlr.x, d = mc.udlr.y, l = mc.udlr.z, r = mc.udlr.w;
        switch (dir.x)
        {
            case -1:
                l = 1;
                break;
            case 1:
                r = 1;
                break;
        }
        switch (dir.y)
        {
            case -1:
                d = 1;
                break;
            case 1:
                u = 1;
                break;
        }
        mc.udlr = new(u, d, l, r);
    }
    public IEnumerator Verify(Vector2Int pos,MapChunck me,ProgressVisitor pv)
    {
       
        me.deepreflect++;
        if (!IsBound(pos))
            if (map[pos.x, pos.y] != null)
            {
             
                if (map[pos.x, pos.y].state == "none")
                {
                    MapChunck other = map[pos.x, pos.y];
                    Vector2Int factdir = (other.pos - me.pos);
                    if (other.type == "tonel")
                    {
                        if (!(Mathf.Abs(factdir.x) == Mathf.Abs(other.dir.x) & Mathf.Abs(factdir.y) == Mathf.Abs(other.dir.y)))
                        {
                           
                        }
                        else
                        {
                            pv.active = true;
                            map[pos.x, pos.y].state = "verify";
                            AddUDLR(me, factdir);
                            AddUDLR(other, -factdir);
                            other.deepreflect = me.deepreflect;
                            pv.progress++;
                            yield return AroundVerify(pos, other,pv);
                        }
                    }
                    else
                    {
                        if (me.type == other.type)
                        {
                            map[pos.x, pos.y].state = "verify";
                            pv.active = true;
                            AddUDLR(me, factdir);
                            AddUDLR(other, -factdir);
                            other.deepreflect = me.deepreflect;
                            pv.progress++;
                            yield return AroundVerify(pos, other,pv);

                        }
                        else
                        {
                            if (!(Mathf.Abs(factdir.x) == Mathf.Abs(me.dir.x) & Mathf.Abs(factdir.y) == Mathf.Abs(me.dir.y)))
                            {

                            }
                            else
                            {
                                map[pos.x, pos.y].state = "verify";

                                pv.active = true;
                                AddUDLR(me, factdir);
                                AddUDLR(other, -factdir);
                                other.deepreflect = me.deepreflect;
                                pv.progress++;
                                yield return AroundVerify(pos, other,pv);

                            }
                        }
                    }
                }
            }
        
        
    }
    public bool IsAxeX(Vector2Int dir)
    {
        if (dir.x != 0) return true;
        if (dir.y != 0) return false;
        return false;
    }
    public IEnumerator AroundVerify(Vector2Int pos, MapChunck me,ProgressVisitor pv)
    {

       yield return Verify(pos + new Vector2Int(1, 0), me,pv);

        yield return Verify(pos + new Vector2Int(-1, 0), me, pv);

        yield return Verify(pos + new Vector2Int(0, 1), me, pv);

        yield return Verify(pos + new Vector2Int(0, -1), me, pv);
        
       

    }
    public IEnumerator Verifycation(ProgressVisitor pv)
    {
        MapChunck spawn = null;
        for (int i = 0; i < sizefield * sizefield; i++)
        {
          
            MapChunck buffer = map[UnityEngine.Random.Range(0, sizefield), UnityEngine.Random.Range(0, sizefield)];
            if (buffer != null)
                if (buffer.type == "room")
                {
                    spawn = buffer;
                    buffer.state = "spawn";
                    first = spawn;
                    break;
                }
            
        }

        if (spawn != null)
        {
            yield return AroundVerify(spawn.pos, spawn,pv);
            pv.IsDone = true;
           
        }
    }
    public IEnumerator DeleteNotVerify(ProgressVisitor pv)
    {
        
        for (int x = 0; x < sizefield; x++)
            for (int y = 0; y < sizefield; y++)
            {
                pv.active = true;
                pv.progress = (sizefield * (float)x + y) / (sizefield * (float)sizefield);
                if (map[x, y] != null)
                    if (map[x, y].state == "none")
                    {
                        map[x, y] = null;
                    }
                    else
                    {
                        if (map[x, y].deepreflect > deepReflection)
                        {
                            deepReflection = map[x, y].deepreflect;
                            last = map[x, y];
                        }
                        if(map[x, y].type == "room")
                        {
                            map[x, y].InVariant();
                        }
                    }
                if (GeneratorLevel.Skeep())
                    yield return null;
            }
        if (last != null)
            last.state = "final";
        first.variant = new("start_room", "room", "StartRoom", 1f);
        pv.IsDone = true;
    }


    public IEnumerator Generation(List<ProgressVisitor> pvs)
    {
        ProgressVisitor room=new("Генерация комнат"), tonel=new("Генерация тонелей"), verify = new("Верификация"), delete= new("Экспертиза");
        pvs.Add(room);
        pvs.Add(tonel);
        pvs.Add(verify);
        pvs.Add(delete);
        yield return Rooming(room);
        yield return Toneling(tonel);
        yield return Verifycation(verify);
        yield return DeleteNotVerify(delete);
    }
}
[Serializable]
public class ProgressVisitor{
    public float progress;
    public bool IsDone;
    public string name;
    public bool active;
    public ProgressVisitor(string name)
    {
        this.name = name;
    }
}
public class MapChunck
{
    public string type;
    public Vector2Int pos, dir;
    public string state = "none";
    public int deepreflect;
    public Vector4 udlr = Vector4.zero;
    public LevelElementVariant variant;
    public MapChunck(string type, Vector2Int pos)
    {
        this.type = type;
        this.pos = pos;
    }
    public MapChunck(string type, Vector2Int pos, Vector2Int dir)
    {
        this.type = type;
        this.pos = pos;
        this.dir = dir;
    }
    public void InVariant()
    {
        variant = new VariantDungeos().RandomRoom();
    }
}
public class LevelElementVariant
{
    public string name, type;
    public string namePrefab;
    public float veroat;
    public GameObject prefab;
    public LevelElementVariant(string name, string type, string namePrefab, float veroat)
    {
        this.name = name;
        this.type = type;
        this.namePrefab = namePrefab;
        this.veroat = veroat;
        Init();
    }
    public void Init()
    {
        prefab = BaseFunc.GetPrefab(namePrefab);
    }
    public int GetLenght()
    {
        return(int) (1f / (1f - veroat));
    }
}

public class VariantDungeos{
    public static bool init;
    public static List<LevelElementVariant> variants =new()
       {
        new("forest_room","room","ForestRoom",0.99f),
        new("rock_room", "room", "RockRoom", 0.01f)
    };
    public static List<LevelElementVariant> stripe;
    public void Init()
    {
        
        if (!init)
        {
            stripe = new();
            init = true;
            float sum = 0;
            foreach (LevelElementVariant vr in variants)
            {
                sum += vr.veroat;
            }
            foreach (LevelElementVariant vr in variants)
            {
                int count = (int)(1f / (1f - (vr.veroat / sum)));
                for (int i = 0; i < count; i++)
                    stripe.Add(vr);
            }
        }

    }
   
    public VariantDungeos()
    {
        Init();
    }
    public LevelElementVariant RandomRoom()
    {
        
        return stripe[UnityEngine.Random.Range(0, stripe.Count)];
    }
}
