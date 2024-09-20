using UnityEngine;

public class ReflectiveShield : MonoBehaviour
{
    public CurrentTriggerCollision ctc;
    public float strongh;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ctc.CurrentColliders.Count;i++)
        {
            CurrentTriggerCollision.SearchInfo info = ctc.SearchEn(i);
            if (info.en != null)
            {
                Vector2 dir = (info.en.transform.position - transform.position).normalized;
                info.en.subdir = dir * strongh;
            }
        }

    }
}
