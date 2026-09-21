using System.Collections.Generic;
using UnityEngine;

public class PerlinOreGenerator : OreGenerator
{
    private float[,] noiseMap;
    NoiseGenerator noiseGenerator;

    public PerlinOreGenerator(GridData data, List<LayerData> layers, BlockData[,] grid) : base(data, layers, grid)
    {
        noiseGenerator = new NoiseGenerator(noiseMap);
    }

    public override void GenerateOres()
    {
        noiseMap = noiseGenerator.GeneratePerlinNoiseMap(data.Width, data.Depth, 0.01f, 3, 0.5f, 2.0f);

        for (int layerIndex = 0; layerIndex < layers.Count; layerIndex++)
        {
            List<Block> ores = layers[layerIndex].Ores;

            for (int i = 0; i < ores.Count; i++)
            {
                Block ore = ores[i];

                GenerateOreClusters(ore.Data, layerIndex);
            }
        }
    }

    private void GenerateOreClusters(BlockData oreData, int layerIndex)
    {
        int clusterCount = Seed.RandomINT(oreData.MinClusterCount, oreData.MaxClusterCount + 1);

        List<Vector2Int> seeds = FindClusterSeeds(clusterCount, layerIndex, oreData);

        for (int i = 0; i < seeds.Count; i++)
        {
            Vector2Int seed = seeds[i];

            GenerateOreCluster(oreData, seed.x, seed.y);
        }
    }

    private List<Vector2Int> FindClusterSeeds(int clusterCount, int layerIndex, BlockData oreData)
    {
        List<Vector2Int> seeds = new();

        int minY = GetLayerStart(layerIndex);
        int maxY = GetLayerEnd(layerIndex);

        // Preventing generating clusters on top of eachother
        int minDistance = Mathf.Max(2, Mathf.RoundToInt(Mathf.Sqrt(oreData.MaxClusterSize)));

        while (seeds.Count < clusterCount)
        {
            int x = Seed.RandomINT(0, grid.GetLength(0));
            int y = Seed.RandomINT(minY, maxY + 1);

            if (!CanPlaceOre(x, y))
            {
                continue;
            }

            Vector2Int candidate = new Vector2Int(x, y);

            bool isTooClose = false;

            foreach (var seed in seeds)
            {
                float distance = Vector2Int.Distance(candidate, seed);

                if (distance < minDistance)
                {
                    isTooClose = true;
                    break;
                }
            }

            if (isTooClose)
            {
                continue;
            }

            seeds.Add(candidate);
        }

        return seeds;
    }

    private void GenerateOreCluster(BlockData oreData, int startX, int startY)
    {
        if (!CanPlaceOre(startX, startY))
        {
            return;
        }

        int clusterSize = Seed.RandomINT(oreData.MinClusterSize, oreData.MaxClusterSize + 1);

        Queue<Vector2Int> queue = new();
        HashSet<Vector2Int> visited = new();

        Vector2Int seed = new Vector2Int(startX, startY);

        float seedNoiseValue = noiseMap[startX, startY];

        queue.Enqueue(seed);
        visited.Add(seed);

        int placed = 0;

        while (queue.Count > 0 && placed < clusterSize)
        {
            var current = queue.Dequeue();

            if (!CanPlaceOre(current.x, current.y))
            {
                continue;
            }

            grid[current.x, current.y] = oreData;
            placed++;

            List<Vector2Int> neighbours = GetNeighbours(current.x, current.y);

            neighbours.Sort((a, b) =>
            {
                float aDifference = Mathf.Abs(noiseMap[a.x, a.y] - seedNoiseValue);

                float bDifference = Mathf.Abs(noiseMap[b.x, b.y] - seedNoiseValue);

                return aDifference.CompareTo(bDifference);
            });

            foreach (var neighbour in neighbours)
            {
                if (!visited.Add(neighbour))
                {
                    continue;
                }

                if (!CanPlaceOre(neighbour.x, neighbour.y))
                {
                    continue;
                }

                queue.Enqueue(neighbour);
            }
        }
    }

    private List<Vector2Int> GetNeighbours(int x, int y)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        AddNeighbour(neighbours, x + 1, y);
        AddNeighbour(neighbours, x - 1, y);
        AddNeighbour(neighbours, x, y - 1);
        AddNeighbour(neighbours, x, y + 1);

        AddNeighbour(neighbours, x + 1, y + 1);
        AddNeighbour(neighbours, x - 1, y - 1);
        AddNeighbour(neighbours, x + 1, y - 1);
        AddNeighbour(neighbours, x - 1, y + 1);

        return neighbours;
    }

    private void AddNeighbour(List<Vector2Int> neighbours, int x, int y)
    {
        if (!CanPlaceOre(x, y))
        {
            return;
        }

        neighbours.Add(new Vector2Int(x, y));
    }

    private bool CanPlaceOre(int x, int y)
    {
        if (x < 0 || x >= grid.GetLength(0))
        {
            return false;
        }

        if (y < 0 || y >= grid.GetLength(1))
        {
            return false;
        }

        BlockData block = grid[x, y];

        if (block == null)
        {
            return false;
        }

        return block.GenerationType == BlockGenerationType.Base;
    }
}
