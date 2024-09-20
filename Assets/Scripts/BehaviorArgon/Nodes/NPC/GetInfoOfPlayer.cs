using UnityEngine;

public class GetInfoOfPlayer : NodeBeh
{
    public NodeParameter pos,
        dir,distance,velocity,mypos,energyCast;
    
    public override void Init(params object[] vs)
    {
        pos = new(Player.me.transform.position, Vector2TypePS, this);
        dir = new(Player.me.transform.position - gameObject.transform.position.normalized, Vector2TypePS, this);
        distance = new(0f,FloatTypePS,this);
        energyCast = new(0f, FloatTypePS, this);
        velocity = new(Player.me.physic.rg.velocity, Vector2TypePS, this);
        mypos = new(transform.position, Vector2TypePS, this); 

    }

    public override void OnStart()
    {
        pos.SetValue((Vector2)Player.me.transform.position);
        dir.SetValue((Vector2)(Player.me.transform.position - transform.position).normalized);
        distance.SetValue(Vector2.Distance( Player.me.transform.position,transform.position));
        velocity.SetValue(Player.me.physic.rg.velocity);
        mypos.SetValue((Vector2)transform.position);
        CreaterMagic cm = GetComponent<CreaterMagic>();
        energyCast.SetValue(cm.CloseEnergyToLimit(0.413496671566344f));
    }

    public override void OnUpdate()
    {
        return;
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
}
