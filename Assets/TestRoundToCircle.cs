using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoundToCircle : MonoBehaviour
{
    public Transform a, b;
    public float speed;

    public Vector2 RotationTo()
    {
        Vector2 perpen, to_a, move_vector;
      
        to_a = (a.position - b.position).normalized;
        perpen = (Quaternion.Euler(0, 0, 90) * to_a * speed).normalized;
        //perpen = Vector2.Perpendicular((to_a*speed).normalized);
        move_vector = (perpen + to_a).normalized;
        return move_vector;
    }
    void Update()
    {
        Vector2 perpen, to_a,move_vector;
        float d = Vector2.Distance(a.position, b.position);
        to_a = (a.position - b.position).normalized;
        perpen=  (Quaternion.Euler(0, 0, 90) * to_a*speed).normalized;
        //perpen = Vector2.Perpendicular((to_a*speed).normalized);
        move_vector = (perpen + to_a * (d - 3)).normalized*Mathf.Abs (speed);
        b.position +=(Vector3) move_vector * Time.deltaTime;
    }
}
