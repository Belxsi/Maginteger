using UnityEngine;
public class AutoWall : MonoBehaviour
{
    public GameObject[] walls;
    public MapChunck chunck;
    public void SetActiveWall(bool trfl)
    {
        if (chunck.udlr.x == 1)
        {
            walls[0].SetActive(trfl);
        }
        else walls[0].SetActive(false);
        if (chunck.udlr.y == 1)

        {
            walls[1].SetActive(trfl);
        }
        else walls[1].SetActive(false);
        if (chunck.udlr.z == 1)
        {
            walls[2].SetActive(trfl);
        }
        else walls[2].SetActive(false);
        if (chunck.udlr.w == 1)
        {
            walls[3].SetActive(trfl);
        }
        else walls[3].SetActive(false);
    }
}

