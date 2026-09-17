using UnityEngine;

public class NoiseGenerator
{
    private float[,] noiseMap;

    public NoiseGenerator(float[,] noiseMap)
    {
        this.noiseMap = noiseMap;
    }

    public float[,] GeneratePerlinNoise(int width, int height, float scale, int octaves, float persistence, float lacunarity)
    {
        noiseMap = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int i = 0; i < octaves; i++)
                {
                    float frequency = Mathf.Pow(lacunarity, i);
                    float amplitude = Mathf.Pow(persistence, i);

                    float xCoord = (x + Seed.RandomINT(int.MinValue, int.MaxValue)) * scale;
                    float yCoord = (y + Seed.RandomINT(int.MinValue, int.MaxValue)) * scale;
                    noiseMap[x, y] = Mathf.PerlinNoise(xCoord * frequency, yCoord * frequency) * amplitude;
                }
            }
        }

        return noiseMap;
    }
}
