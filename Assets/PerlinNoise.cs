using UnityEngine;

public class PerlinNoise 
{
   
        public float[,] Noise { get; private set; }
        public string Seed { get; private set; }
        public PerlinNoise(string seed)
        {
            this.Seed = seed;
            this.Noise = new float[1025, 1025];
        }

        public float GetXZ(VectorXZ position, int octaves, int resolution, float bias)
        {
            float fNoise = 0.0f;
            float fScale = 1.0f;
            float fScaleAcc = 0.0f;

            for (int i = 0; i < octaves; i++)
            {
                int pitch = resolution >> i;
                if (pitch <= 0) continue;

                VectorXZ sample1 = new VectorXZ(
                    (position.X / pitch) * pitch,
                    (position.Z / pitch) * pitch);

                VectorXZ sample2 = new VectorXZ(
                    (sample1.X + pitch),
                    (sample1.Z + pitch));

                float fBlendX = (float)(position.X - sample1.X) / (float)pitch;
                float fBlendZ = (float)(position.Z - sample1.Z) / (float)pitch;

                float fSampleT = (1.0f - fBlendX) * this.GetNoise(sample1) + fBlendX * this.GetNoise(new VectorXZ(sample2.X, sample1.Z));
                float fSampleB = (1.0f - fBlendX) * this.GetNoise(new VectorXZ(sample1.X, sample2.Z)) + fBlendX * this.GetNoise(sample2);

                fScaleAcc += fScale;
                fNoise += (fBlendZ * (fSampleB - fSampleT) + fSampleT) * fScale;
                fScale = fScale / bias;
            }

            return fNoise / fScaleAcc;
        }

        private float GetNoise(VectorXZ position)
        {
            if (this.Noise[position.X, position.Z] == 0.0)
            {
                this.Noise[position.X, position.Z] = this.GetXZNoise(position);
            }

            return this.Noise[position.X, position.Z];
        }
        private float GetXZNoise(VectorXZ position)
        {
            string key = position.X.ToString() + position.Z.ToString() + this.Seed;
            return (float)Hash.Jenkins(key) / (float)uint.MaxValue;
        }
    

}
class Hash
{
    public static uint Jenkins(string key)
    {
        int i = 0;
        uint hash = 0;
        while (i != key.Length)
        {
            hash += key[i++];
            hash += hash << 10;
            hash ^= hash >> 6;
        }

        hash += hash << 3;
        hash ^= hash >> 11;
        hash += hash << 15;
        return hash;
    }
}
public struct VectorXZ
{
    public int X;
    public int Z;
    public VectorXZ(int x, int z)
    {
        this.X = x;
        this.Z = z;
    }
    public VectorXZ(Vector2 vec)
    {
        this.X = (int)vec.x;
        this.Z = (int)vec.y;
    }
}
