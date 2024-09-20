using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class VolumeMoveTextureBloomNoise : MonoBehaviour
{
    public Volume volume;
    public Bloom bloom;
    public int sum;
    public Texture2D texture1;
   
    void Start()
    {
        if (volume == null) volume.GetComponent<Volume>();
        //Bloom bloom;
        var list = volume.profile.components;
        bloom = (Bloom)list[0];
        

    }
  
    PerlinNoise perlin = new ("12");
    public Texture2D GeneratorNoise()
    {
        int size = 128;
        Texture2D texture = new(size, size, TextureFormat.RGBA32,true);
        
        for (int x=0;x< size; x++)
            for (int y = 0; y < size; y++)
            {
                float xCoord = (float)x / size;
                float yCoord = (float)y / size;
                
                float Sample =(float) Mathf.PerlinNoise(xCoord*10,(yCoord+sum/(float)size)*4);
                texture.SetPixel(x, y, Color.white*Sample );
            }
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                float sample=0;
                int count=0;
                bool Xrer(float n)
                {
                    return n > 0 & n < size;
                }
                void OldStep(int bx,int by){
                    if (Xrer(bx) & Xrer(by))
                    {
                        sample += texture.GetPixel(bx, by).grayscale;
                        count++;
                    }
                }
                OldStep(x - 1, y);
                OldStep(x + 1, y);
                OldStep(x - 1, y-1);
                OldStep(x - 1, y+1);
                OldStep(x + 1, y-1);
                OldStep(x + 1, y+1);
                OldStep(x , y+1);
                OldStep(x, y - 1);
                texture.SetPixel(x, y, Color.white * (sample/count));


            }
                texture.filterMode = FilterMode.Trilinear;
        texture.Apply();
        return texture;
    }
    void Update()
    {
        sum++;
        texture1 = GeneratorNoise();
        bloom.dirtTexture.Override(texture1);


    }
}
