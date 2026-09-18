using UnityEngine;

public class TilemapGenerator<T>
{
    public readonly T[,] Tilemap;

    public TilemapGenerator(T[,] tilemap)
    {
        this.Tilemap = tilemap;
    }

    public T[,] Generate(int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tilemap[x, y] = default;
            }
        }

        return Tilemap;
    }
}
