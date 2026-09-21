using UnityEngine;

public class NoiseGenerator
{
    private float[,] noiseMap;

    public NoiseGenerator(float[,] noiseMap)
    {
        this.noiseMap = noiseMap;
    }

    public float[,] GeneratePerlinNoiseMap(int width, int height, float scale, int octaves, float persistence, float lacunarity)
    {
        noiseMap = new float[width, height];

        float offsetX = Seed.RandomFLOAT(-10000, 10000);
        float offsetY = Seed.RandomFLOAT(-10000, 10000);

        float maxNoiseHeight = 0f;

        for (int i = 0; i < octaves; i++)
        {
            maxNoiseHeight += Mathf.Pow(persistence, i);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float noiseHeight = 0f;

                for (int i = 0; i < octaves; i++)
                {
                    float frequency = Mathf.Pow(lacunarity, i);
                    float amplitude = Mathf.Pow(persistence, i);

                    float xCoord = (x + offsetX) * scale * frequency;
                    float yCoord = (y + offsetY) * scale * frequency;

                    noiseHeight += Mathf.PerlinNoise(xCoord, yCoord) * amplitude;
                }

                noiseMap[x, y] = noiseHeight / maxNoiseHeight;
            }
        }

        return noiseMap;
    }
}
