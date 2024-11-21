using UnityEngine;

[CreateAssetMenu(fileName = "GLParameters", menuName = "Scriptable Objects/GLParameters")]
public class GLParameters : ScriptableObject
{
    public float scale;
    public float sizefield, scaleperlin, lenghttonel;
    public float gate, minDistantLOD, tsc;
    public PerlinParam Perlin;
    public float timeConrol;
}
