using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GeneratorScroll : MonoBehaviour
{
    public List<string> spells = new();
    public bool trfl;
    public int maxcount;
    public static GeneratorScroll me;
    public string GetRandomSpell()
    {
        return spells[Random.Range(0, spells.Count)];
    }
    void Awake()
    {
        me = this;
       
    }
    public void CreateScrolls()
    {
        
        GameObject prefab = BaseFunc.GetPrefab("MagicScroll");
        for (int i = 0; i < spells.Count&i<maxcount; i++) {
            GameObject obj = Instantiate(prefab, new Vector2(0, i), Quaternion.identity);
            obj.GetComponent<ItemMagicScroll>().Spell = spells[i];

                }
    }
    public void Load()
    {
        string path = Application.streamingAssetsPath;
        path = Directory.GetFiles(path, "*.txt")[0];
        string content = File.ReadAllText(path);
        string sum = "";
        for(int i = 0; i < content.Length; i++)
        {
            if (content[i] != '\n')
            {
                sum += content[i];
            }
            else
            {
                spells.Add(sum);
                sum = "";
            }
        }
        if(sum!="") spells.Add(sum);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (trfl)
        {
            trfl = false;
            Load();
            CreateScrolls();
        }
    }
}
