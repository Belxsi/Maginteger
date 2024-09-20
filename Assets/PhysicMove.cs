using UnityEngine;

public class PhysicMove : MonoBehaviour
{
    public Rigidbody2D rg;
    public Element element;
    public float alternativeSpeed;
    public bool alternative;
    public bool character;
    public CharacterController cc;
    public void OnAlternative(float speed)
    {
        alternativeSpeed = speed;
        alternative = true;
    }
    public void Awake()
    {

        AutoRealize();

    }
    public void AutoRealize()
    {
        if (!character)
        {
            if (!rg)
            {
                rg = GetComponent<Rigidbody2D>();
                if (!rg)
                {
                    rg = gameObject.AddComponent<Rigidbody2D>();
                    rg.gravityScale = 0;

                }
            }
        }
        else
        {
            if (cc==null)
            {
                cc = GetComponent<CharacterController>();
                if (cc==null)
                {
                    cc = gameObject.AddComponent<CharacterController>();


                }
            }
        }
    }
    public float GetSpeed()
    {
        if (!character)
        {
            return rg.velocity.magnitude;
        }
        else return cc.velocity.magnitude;

    }
    public void Init(Element element)
    {
        this.element = element;
    }
    public void ControlledSpeed()
    {
        if (!character)
        {
            if (rg.velocity.magnitude > element.parameters.speed)
            {
                rg.velocity = rg.velocity.normalized * element.parameters.speed;
            }
        }
        else
        {
            if (cc.velocity.magnitude > element.parameters.speed)
            {
                cc.SimpleMove(cc.velocity.normalized * element.parameters.speed);
            }
        }
    }
    public void AltolledSpeed()
    {
        if (rg != null)
            if (rg.velocity.magnitude > alternativeSpeed)
            {
                rg.velocity = rg.velocity.normalized * alternativeSpeed;
            }
    }
    public void Move(Vector2 dir, ForceMode2D forceMode)
    {
        if (!character)
        {
            rg.AddForce(dir, forceMode);
        }
        else cc.SimpleMove(dir);

    }
    public void Move(Vector2 dir)
    {



        if (!character)
        {
            rg.velocity = dir;
        }
        else cc.SimpleMove(dir);
    }
    void Update()
    {
        if (!character)
        {
            if (rg == null)
            {
                AutoRealize();
            }
        }
        else
        {
            if (!character)
            {
                if (cc == null)
                {
                    AutoRealize();
                }
            }

        }
        if (!alternative)
        {
            if (element != null)
            {
                ControlledSpeed();
            }
        }
        else
        {
            AltolledSpeed();
        }
    }
}
        
