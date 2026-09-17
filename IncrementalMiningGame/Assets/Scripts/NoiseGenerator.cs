using UnityEngine;

public static class NoiseGenerator
{
    private static float[,] noiseMap;

    public static float[,] GenerateNoise(int width, int height, float scale, int octaves, float persistence, float lacunarity)
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
