using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public TypeBehaviour type;
    public Vector2 vectorMovement;
    public NPC npc;
    public NB_Base nb_Base;
    public NPCAutoWalk autoWalk;
    public bool IsAutoWalk;
    public Vector2 up, down, left, rigth;
    public Transform u, d, l, r;
    public ContactFilter2D contact;
    public float avoidingWall, avoiding, avoidPerpen, Gausine;
    public List<float> dist;
    public int fis = 1;
    public static List<string> baseTag = new()
    {
        "Wall"


    };
    
    public void SetFace(Vector2 pos)
    {
        if (Player.TryGetPlayer())
        {
            dist.Add(Vector2.Distance(transform.position, pos));


            if (dist.Count > 5)
            {
                dist.RemoveAt(0);
            }
            if (dist[0] > dist[^1])
            {
                fis = -1;
            }
            else { fis = 1; }
        }
    }

    public bool Raycast(Vector2 vector2, out Vector2 point)
    {
        point = Vector2.zero;
        RaycastHit2D[] hit = new RaycastHit2D[1];
        ContactFilter2D contactFilter2D = new();
        contactFilter2D.maxDepth = 25;
        contactFilter2D.layerMask = new();
        LayerMask layer = new();
        layer.value = 1 << 9;
        contactFilter2D.SetLayerMask(layer);

        Physics2D.Raycast(transform.position, vector2, contactFilter2D, hit, 50);

        if (hit[^1].collider != null)
        {
            if (vector2 == Vector2.up)
            {

            }
            point = hit[^1].point;
            Debug.DrawLine(transform.position, point + vector2, Color.red);
            return true;
        }
        return false;
    }
    public bool NotMe(GameObject game)
    {
        LinkForParent lfp = game.GetComponent<LinkForParent>();
        if (lfp != null)
            if (lfp.parent == gameObject)
            {
                return false;
            }
        if (game == gameObject)
        {
            return false;
        }
        return true;

    }
    public bool RaycastReflection(Vector2 origin, Vector2 dir, out Vector2 point, out RaycastHit2D hit, out bool reflect)
    {
        point = Vector2.zero;
        RaycastHit2D[] hits = new RaycastHit2D[1];
        ContactFilter2D contactFilter2D = new();
        contactFilter2D.maxDepth = 25;
        hit = default;
        reflect = false;



        hits = Physics2D.RaycastAll(origin, dir);

        for (int i = 0; i < hits.Length; i++)
        {
          
            if (hits[i].collider != null)
                if (baseTag.Contains(hits[i].collider.tag))
                    if (NotMe(hits[i].collider.gameObject))
                    {
                        switch (hits[i].collider.tag)
                        {
                            case "Energy":
                            case "Player":
                                reflect = true;
                                break;
                        }

                        point = hits[i].point;
                        hit = hits[i];
                        //Debug.DrawLine(origin, point, Color.cyan);
                        return true;
                    }
        }
        return false;
    }
    public RaycastHit2D Path(int count, Ray2D ray, Vector2 factdir, List<string> exeptions, float fs)
    {
        Vector2 point = ray.origin;
        Vector2 fact = factdir;
        RaycastHit2D hit = default, save = default;
        for (int i = 0; i < count & i < 6; i++)
        {


            bool reflect = false;
            RaycastReflection(point + fact, fact, out point, out hit, out reflect);
            if (hit != default) save = hit;
            // fact = ray.direction;
            // if (!exeptions.Contains(hit.collider.tag)) break;
            if (reflect)
            {
                count++;
            }
            else
            {

            }
            if (hit == default)
            {
                return default;
            }
            switch (hit.collider.tag)
            {
                case "Energy":
                case "Player":
                    fact = Vector2.Reflect(Quaternion.Euler(0, 0, 360 * fs) * factdir, hit.normal);

                    break;
                default:
                    return hit;
                    fact = Vector2.Reflect(factdir, hit.normal);
                    break;
            }
        }
        if (hit == default)
        {
            return save;
        }
        if (exeptions.Contains(hit.collider.tag))
        {
            return save;
        }
        return save;

    }
    public RaycastHit2D GoodingPath(RaycastHit2D A, RaycastHit2D B, Vector2 point)
    {
        if (Vector2.Distance(A.point, point) > Vector2.Distance(B.point, point))
        {
            return A;
        }
        else return B;
    }
    public Vector2 GoodingPath(Vector2 A, Vector2 B, Vector2 point)
    {
        if (Vector2.Distance(A, point) > Vector2.Distance(B, point))
        {
            return A;
        }
        else return B;
    }
    public bool PathSimple(Ray2D ray, out RaycastHit2D hit)
    {
        Vector2 point = Vector2.zero;

        bool reflect = false;


        return RaycastReflection(ray.origin, Quaternion.Euler(0, 0, Random.Range(-90, 90)) * -ray.direction, out point, out hit, out reflect);

    }
    public bool PathSimple(Ray2D ray, out RaycastHit2D hit, float angle)
    {
        Vector2 point = Vector2.zero;

        bool reflect = false;


        return RaycastReflection(ray.origin, Quaternion.Euler(0, 0, angle) * -ray.direction, out point, out hit, out reflect);

    }
    public Vector2 LongestPath(Ray2D ray, Vector2 factdir, List<string> exeptions,out RaycastHit2D element)
    {
        List<RaycastHit2D> list = new();
        bool upAngle = false;
        element = default;
        if (Vector2.Distance(ray.origin, transform.position) < 1.5f) upAngle = true;
        for (int i = 0; i < 5; i++)
            if (upAngle)
            {
                if (PathSimple(ray, out RaycastHit2D hit))
                {
                  
                    list.Add(hit);
                }
            }
            else
            {
                if (PathSimple(ray, out RaycastHit2D hit, Random.Range(-120, 120)))
                {

                    list.Add(hit);
                }

            }
        RaycastHit2D good = default;

        for (int i = 1; i < list.Count; i++)
        {
            good = GoodingPath(list[i - 1], list[i], Player.me.transform.position);
            list[i - 1] = good;
        }
        element = good;
        return good.point;


    }
   
    public void GetWallOffsets()
    {
        if (Raycast(Vector2.up, out up))
        {


        }
        if (Raycast(Vector2.right, out rigth))
        {



        }
        if (Raycast(Vector2.down, out down))
        {


        }
        if (Raycast(Vector2.left, out left))
        {

        }

    }
    public void FollowToPlayer()
    {
        if (Player.TryGetPlayer())
        {
            Vector2 dir = (Player.me.transform.position - transform.position).normalized;
            vectorMovement = dir;
        }
    }
    public void FollowToPlayer(float targetDist)
    {

        Vector2 dir = (Player.me.transform.position - transform.position).normalized;
        float dist = Vector2.Distance(Player.me.transform.position, transform.position);
        int i = 1;
        if (dist > targetDist) i = -1;
        vectorMovement = dir * i;
    }
    public void AI_FTP()
    {
        autoWalk.tcs = new TargetAutoWalk(Player.me.transform.position);
    }
    public void WalkTo(Vector2 dir, Vector2 target)
    {
        if (autoWalk.tcs != null)
        {
            Vector2 result = GoodingPath(autoWalk.tcs.pos, dir, target);

            if (result == dir)
            {
                autoWalk.tcs = new TargetAutoWalk(dir);
                vectorMovement = dir;
            }
        }
        else
        {
            autoWalk.tcs = new TargetAutoWalk(dir);
            vectorMovement = dir;
        }

    }
    public void Walk(Vector2 dir)
    {
            autoWalk.tcs = new TargetAutoWalk(dir);
            vectorMovement = dir;
        

    }
    public float Cicloid(float t)
    {
        if ((t >= 0) & (t <= 1))
        {
            return (1 - Mathf.Cos(t * 2 * Mathf.PI)) - 1;
        }
        else
        {

            if (t > 0) return 1;
            if (t < 0) return -1;
            return 0;
        }
    }
    public float InRange(float current, float target, float lenght)
    {
        float t = current / lenght - target / lenght;
        if (t > 1) return 1;
        if (t < -1) return -1;
        return t;
    }
    public void IsNearedWall(ref Vector2 neared, ref float d, Vector2 point)
    {

        Vector2 result = point;
        float b = Vector2.Distance(transform.position, result);
        if (b < d)
        {
            neared = result;
            d = b;
        }
    }
    public Vector2 NearWall()
    {
        float d = 10000;
        Vector2 neared = Vector2.zero;

        IsNearedWall(ref neared, ref d, up);
        IsNearedWall(ref neared, ref d, down);
        IsNearedWall(ref neared, ref d, left);
        IsNearedWall(ref neared, ref d, rigth);
        return neared;
    }
    public Vector2 NearWall(out float d)
    {
        d = 10000;
        Vector2 neared = Vector2.zero;

        IsNearedWall(ref neared, ref d, up);
        IsNearedWall(ref neared, ref d, down);
        IsNearedWall(ref neared, ref d, left);
        IsNearedWall(ref neared, ref d, rigth);
        return neared;
    }
    public void Avoid(Vector2 near, Vector2 dirv, Energy energy)
    {

        npc.life.valueSpeed = npc.parameters.speed * 2;
        float d = Vector2.Distance(transform.position, near);
        Vector2 localpos = transform.InverseTransformPoint(near);
        Vector2 perpen, move_vector;


        perpen = (Quaternion.Euler(0, 0, 90) * dirv).normalized;
        //perpen = Vector2.Perpendicular((to_a*speed).normalized);
        move_vector = (perpen + dirv).normalized;
        float t = energy.transform.localScale.magnitude / d + 1;
        Vector2 dir = -localpos.normalized * avoiding * t * t;
        dir += move_vector * avoidPerpen * t * t;
        float dist = 0;
        if (Player.TryGetPlayer())
            dist = Vector2.Distance(Player.me.transform.position, transform.position);
        dir = 3 * (dir * (dist - 3)).normalized;
        dir -= avoidingWall * Vector2.Distance(up, transform.position) * ((Vector2)transform.position - up).normalized;
        dir -= avoidingWall * Vector2.Distance(down, transform.position) * ((Vector2)transform.position - down).normalized;
        dir -= avoidingWall * Vector2.Distance(left, transform.position) * ((Vector2)transform.position - left).normalized;
        dir -= avoidingWall * Vector2.Distance(rigth, transform.position) * ((Vector2)transform.position - rigth).normalized;
        dir += new Vector2((Gaussiane(0, 1) * 2f - 1f) / 2f, (Gaussiane(0, 1) * 2f - 1f) / 2f) * Gausine;

        Debug.DrawRay(transform.position, dir, Color.blue);
        // Vector2 pos = (Vector2)transform.position + dir;
        autoWalk.tcs = new TargetAutoWalk(dir.normalized * t);

    }
    public void Gauside()
    {
        npc.life.valueSpeed = npc.parameters.speed * 1;
        TargetCast tc = new(TypeTargetCast.Player, transform);
        Vector2 dir = tc.GetTarget(), pos;
        float dist = 0;


        dir += new Vector2((Gaussiane(0, 1) * 2f - 1f) / 2f, (Gaussiane(0, 1) * 2f - 1f) / 2f);
        if (Player.TryGetPlayer())
            dist = Vector2.Distance(Player.me.transform.position, transform.position);
        Vector2 wall = NearWall();




        dir = 3 * (dir * (dist - 3)).normalized;
        dir -= avoidingWall * 0.25f * Vector2.Distance(up, transform.position) * ((Vector2)transform.position - up).normalized;
        dir -= avoidingWall * 0.25f * Vector2.Distance(down, transform.position) * ((Vector2)transform.position - down).normalized;
        dir -= avoidingWall * 0.25f * Vector2.Distance(left, transform.position) * ((Vector2)transform.position - left).normalized;
        dir -= avoidingWall * 0.25f * Vector2.Distance(rigth, transform.position) * ((Vector2)transform.position - rigth).normalized;
        //pos = (Vector2)transform.position + dir;
        // RaycastHit2D hit= Physics2D.Raycast(transform.position, dir);
        /*  if (hit.collider != null)
          {
              if(Vector2.Distance(transform.position,pos)>hit)
              pos = hit.point;

          }
        */
        autoWalk.tcs = new TargetAutoWalk(dir.normalized);
    }

    public float Gaussiane(double mean, double stdDev)
    {
        System.Random rand = new(); //reuse this if you are generating many
        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) *
                     System.Math.Sin(2.0 * System.Math.PI * u2); //random normal(0,1)
        return System.Convert.ToSingle((mean + stdDev * randStdNormal)); //random normal(mean,stdDev^2)
    }

    public void Init(NB_Base nb_Base)
    {
        this.nb_Base = nb_Base;
    }



}
public enum TypeBehaviour
{
    none,
    followToPlayer,
    attack,
    support,
    unrun,
    target
}
