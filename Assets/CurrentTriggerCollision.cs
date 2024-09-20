using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTriggerCollision : MonoBehaviour
{
    public List<Collider2D> CurrentColliders=new();
    public int state;
    public void Start()
    {
        state = Random.Range(int.MinValue, int.MaxValue);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
       
        CurrentColliders.Add(collision);
        state = Random.Range(int.MinValue,int.MaxValue);
    }
    public SearchInfo SearchBO(int startI=0)
    {
        for(int i=startI;i<CurrentColliders.Count; i++)
        {
            BodyObject bo = CurrentColliders[i].gameObject.GetComponent<BodyObject>();
            if (bo == null)
            {
                LinkForParent lfp = CurrentColliders[i].gameObject.GetComponent<LinkForParent>();
                if (lfp != null)
                {
                    bo = lfp.parent.GetComponent<BodyObject>();
                }
            }
            if (bo != null)
            {
                return new SearchInfo(bo,i);
            }
        }
        return new SearchInfo((BodyObject)null, 0) ;
    
    }
    public SearchInfo SearchEn(int startI = 0)
    {
        for (int i = startI; i < CurrentColliders.Count; i++)
        {
            Energy en = CurrentColliders[i].gameObject.GetComponent<Energy>();
           
            if (en != null)
            {
                return new SearchInfo(en, i);
            }
        }
        return new SearchInfo((Energy)null, 0);

    }
    public SearchInfo SearchWall(int startI = 0)
    {
        for (int i = startI; i < CurrentColliders.Count; i++)
        {
            if (CurrentColliders[i].gameObject.CompareTag("Wall"))
            {


                return new SearchInfo(CurrentColliders[i].gameObject, i);
            }
        }
        return new SearchInfo((GameObject)null, 0);

    }
    public class SearchInfo
    {
        public BodyObject bo;
        public Energy en;
        public int index;
        public GameObject ob;

        public SearchInfo(BodyObject bo, int index)
        {
            this.bo = bo;
            this.index = index;
        }
        public SearchInfo(Energy en, int index)
        {
            this.en = en;
            this.index = index;
        }
        public SearchInfo(GameObject ob, int index)
        {
            this.ob = ob;
            this.index = index;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        CurrentColliders.Remove(collision);
        state = Random.Range(int.MinValue, int.MaxValue);
    }
}
