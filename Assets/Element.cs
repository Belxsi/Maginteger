using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Element :MonoBehaviour, IElement
{
    public Life life;
    public Parameters parameters;
    public PhysicMove physic;
    public Visual visual;
    public Dir IsMove;
    public BodyObject meBody;
    public TargetCast tc;
    public Vector2 currentVelocity;
    public int frendlyCode;
    
    public void Init(Life life,PhysicMove physic,Visual visual)
    {
        this.life = life;
        this.physic = physic;
        this.visual = visual;
        visual.InitElement(this);
       
        //this.parameters = parameters;
    }

    public void GetMoveCharacter(Vector3 moveVector)
    {
        IsMove = Dir.none;
        if (moveVector.x != 0)       
           IsMove = Dir.X;
        if (moveVector.y > 0)
        {
            IsMove = Dir.B;
        }
        if (moveVector.y < 0)
        {
            IsMove = Dir.F;
        }
        


    }
    public Vector3 GetElementScreenPos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

}


[Serializable]
public class Life{
    public Parameters parameters;
    public float valueLife,valueEnergy,valueSpeed;
    public bool dead;
    public Life(Parameters parameters)
    {
        this.parameters = parameters;
        valueLife = parameters.life;
        valueEnergy = parameters.energy;
        valueSpeed = parameters.speed;
    }
    public void Action()
    {
        if (!dead)
        {
            if (valueEnergy < parameters.energy)
            {
                valueEnergy += Time.deltaTime*parameters.speedEnergy* Time.deltaTime;
            }
            if (valueEnergy < 0)
            {
                valueLife -= Time.deltaTime;
            }
            if (valueLife <= 0)
            {
                Dead();
            }
        }
    }
    public void Dead()
    {
        dead = true;

    }
    public void Damage(float damage)
    {
        valueLife -= damage;

    }
}
[Serializable]
public class Parameters
{
    public float life, speed, energy,speedEnergy;
    public Parameters(float life, float speed,float energy,float speedEnergy)
    {
        this.life = life;
        this.speed = speed;
        this.energy = energy;
        this.speedEnergy = speedEnergy;
    }
}
public enum Dir
{
    none,
    X,    
    F,
    B
}
public enum Anboly
{
    none,
    run,
    foward,
    back
}
