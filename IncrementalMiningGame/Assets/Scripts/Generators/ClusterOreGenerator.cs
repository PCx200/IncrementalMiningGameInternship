using System.Collections.Generic;
using UnityEngine;

public class ClusterOreGenerator : OreGenerator
{
    public ClusterOreGenerator(GridData data, List<LayerData> layers, BlockData[,] grid) : base(data, layers, grid)
    {
    }

    public override void GenerateOres()
    {
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

        for (int i = 0; i < clusterCount; i++)
        {
            int x = Seed.RandomINT(0, grid.GetLength(0));

            int minY = GetLayerStart(layerIndex);
            int maxY = GetLayerEnd(layerIndex);

            int y = Seed.RandomINT(minY, maxY + 1);

            GenerateOreCluster(oreData, x, y);
        }
    }

    private void GenerateOreCluster(BlockData oreData, int startX, int startY)
    {
        BlockData oreSeedData = TryPlaceOreSeed(oreData, startX, startY);

        if (oreSeedData != null)
        {
            SpreadOre(oreSeedData, startX, startY);
        }
    }

    private BlockData TryPlaceOreSeed(BlockData oreData, int x, int y)
    {
        BlockData currentBlock = grid[x, y];

        if (currentBlock == null || currentBlock.GenerationType != BlockGenerationType.Base)
        {
            return null;
        }

        grid[x, y] = oreData;

        return oreData;
    }

    private void SpreadOre(BlockData seedData, int startX, int startY)
    {
        Queue<Vector2Int> queue = new();
        HashSet<Vector2Int> visited = new();

        int clusterSize = Seed.RandomINT(seedData.MinClusterSize, seedData.MaxClusterSize + 1);

        var seed = new Vector2Int(startX, startY);

        visited.Add(seed);

        queue.Enqueue(seed);

        int placed = 1;

        while (queue.Count > 0 && placed < clusterSize)
        {
            var current = queue.Dequeue();

            if (grid[current.x, current.y] != null && grid[current.x, current.y].GenerationType == BlockGenerationType.Base)
            {
                grid[current.x, current.y] = seedData;
                placed++;
            }

            TrySpread(current.x, current.y + 1, queue, visited);
            TrySpread(current.x, current.y - 1, queue, visited);
            TrySpread(current.x + 1, current.y, queue, visited);
            TrySpread(current.x - 1, current.y, queue, visited);
        }
    }

    private void TrySpread(int x, int y, Queue<Vector2Int> queue, HashSet<Vector2Int> visited)
    {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1))
        {
            return;
        }

        if (!visited.Add(new Vector2Int(x, y)))
        {
            return;
        }

        if (grid[x, y] == null)
        {
            return;
        }

        if (grid[x, y].GenerationType != BlockGenerationType.Base)
        {
            return;
        }

        queue.Enqueue(new Vector2Int(x, y));
    }
}
