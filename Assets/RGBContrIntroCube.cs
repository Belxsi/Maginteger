using UnityEngine;

public class RGBContrIntroCube : MonoBehaviour
{
    public TransformConfiguration r, g, b;
    public RotationHis R,G,B;
    public float speedRotate,PowColor,slowWave,step;
    public void Awake()
    {
        r.rotation = R.Clone();
        g.rotation = G.Clone();
        b.rotation = B.Clone();
        r.AddRotation();
        g.AddRotation();
        b.AddRotation();
     
    }
    public void PowingColor(GameObject m,float offset)
    {
        Renderer rn = m.GetComponent<Renderer>();
        rn.material.SetFloat("_PowColor", PowColor*Mathf.Abs(Mathf.Tan((Time.time / slowWave + offset) )) * Mathf.Abs(Mathf.Tan((Time.time / slowWave + offset) )));
    }
    public void Update()
    {

        PowingColor(r.gameObject,Mathf.Pow(1, step));
        PowingColor(g.gameObject, Mathf.Pow(2, step));
        PowingColor(b.gameObject, Mathf.Pow(3, step));
        r.rotation.SetAngle(R.angle * speedRotate);
        g.rotation.SetAngle(G.angle * speedRotate);
        b.rotation.SetAngle(B.angle * speedRotate);
    }

}
