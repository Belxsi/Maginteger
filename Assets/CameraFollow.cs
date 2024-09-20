using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static GameObject Follower;
    public PhysicMove physic;
    public float intensive;
   
    public bool trfl;
    public List<DistInvisibility> raycasting = new(), buffer = new();

    public static StoperIEnumerator smoothSize;
    void Awake()
    {
        physic = gameObject.AddComponent<PhysicMove>();
        Application.runInBackground = true;
    }

    public static IEnumerator SetSmoothSize(float intensive, float sizeOrto)
    {
        smoothSize = new StoperIEnumerator(true);
        int hash = smoothSize.GetHashCode();
        float orto = sizeOrto; while (smoothSize.active)

        {
            if (smoothSize.GetHashCode() != hash) break;
            yield return new WaitForSeconds(Time.deltaTime);
            Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, orto, intensive);
        }
    }
    public void InvisAllNotPlayer()
    {
        if (Player.TryGetPlayer())
        {
            Vector3 pos = BaseFunc.GetScreenPos(Player.me.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(pos);
            List<RaycastHit2D> results = new();
            raycasting.Clear();
            Physics2D.Raycast(ray.origin, ray.direction, new ContactFilter2D(), results);
            foreach (RaycastHit2D item in results)
            {
                if (item.collider.gameObject.TryGetComponent(out DistInvisibility component))
                {
                    buffer.Remove(component);
                    raycasting.Add(component);
                    component.ishide = true;
                }
            }
            for (int i = 0; i < buffer.Count; i++)
            {
                DistInvisibility item = buffer[i];
                item.ishide = false;
                buffer.Remove(item);
            }

        }
    }
    // Update is called once per frame
    public void UpdateInvis()
    {
        foreach (DistInvisibility item in raycasting)
        {
            buffer.Add(item);
        }

    }
    void LateUpdate()
    {
        UpdateInvis();
        InvisAllNotPlayer();
        if (Follower != null)
        {

            transform.position = new(Mathf.SmoothStep(transform.position.x, Follower.transform.position.x, intensive), Mathf.SmoothStep(transform.position.y, Follower.transform.position.y, intensive));
        }
    }
}
public class StoperIEnumerator
{
    public bool active;

    public StoperIEnumerator(bool active)
    {
        this.active = active;
    }
}
