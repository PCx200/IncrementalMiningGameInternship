using System.Collections.Generic;
using UnityEngine;


public class GridGenerator : MonoBehaviour
{
    [SerializeField] private int SEED;
    private System.Random rand;

    [SerializeField] private GridData data;
    public GridData Data => data;

    [SerializeField] List<Block> blocks;

    public BlockData[,] Grid;

    List<Layer> layers = new();


    private void Start()
    {
        GenerateSeed();

        GroupBlocksInSameLayer();

        Grid = new BlockData[data.Width, data.Depth];

        GenerateBaseGridData();

        GenerateOres();

        InstantiateGrid();
    }


    private void GenerateSeed()
    {
        if (SEED != 0)
        {
            rand = new System.Random(SEED);
            //Debug.Log(SEED);
        }
        else
        {
            System.Random tempRand = new System.Random();
            SEED = tempRand.Next(int.MinValue, int.MaxValue);
            rand = new System.Random(SEED);
            //Debug.Log(SEED);
        }

    }

    private void GenerateBaseGridData()
    {
        int layerIndex = 0;

        for (int y = 0; y < data.Depth; y++)
        {
            if (y > data.LayersDepth[layerIndex])
            {
                layerIndex++;
            }

            BlockData baseBlockData = layers[layerIndex].BaseBlock.Data;

            for (int x = 0; x < data.Width; x++)
            {
                Grid[x, y] = baseBlockData;
            }
        }
    }

    #region Ore Generation
    private void GenerateOres()
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
        int cellCount = data.Width * GetLayerHeight(layerIndex);

        int clusterCount = rand.Next(oreData.MinClusterCount,oreData.MaxClusterCount + 1);

        for (int i = 0; i < clusterCount; i++)
        {
            int x = rand.Next(0, data.Width);

            int minY = GetLayerStart(layerIndex);
            int maxY = GetLayerEnd(layerIndex);

            int y = rand.Next(minY, maxY + 1);

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
        BlockData currentBlock = Grid[x, y];

        if (currentBlock == null || currentBlock.GenerationType != BlockGenerationType.Base)
            return null;

        Grid[x, y] = oreData;

        return oreData;
    }

    private void SpreadOre(BlockData seedData, int startX, int startY)
    {
        Queue<(int x, int y)> queue = new();
        HashSet<(int x, int y)> visited = new();

        int clusterSize = rand.Next(seedData.MinClusterSize, seedData.MaxClusterSize + 1);

        var seed = (startX, startY);

        visited.Add(seed);

        queue.Enqueue(seed);

        int placed = 1;

        while (queue.Count > 0 && placed < clusterSize)
        {
            var current = queue.Dequeue();

            if (Grid[current.x, current.y] != null && Grid[current.x, current.y].GenerationType == BlockGenerationType.Base)
            {
                Grid[current.x, current.y] = seedData;
                placed++;
            }

            /*int dir = rand.Next(0, 8);

            switch (dir)
            {
                case 0:
                    TrySpread(current.x, current.y + 1, queue, visited);

                    break;
                case 1:
                    TrySpread(current.x, current.y - 1, queue, visited);

                    break;
                case 2:
                    TrySpread(current.x + 1, current.y, queue, visited);

                    break;
                case 3:
                    TrySpread(current.x - 1, current.y, queue, visited);

                    break;
                case 4:
                    TrySpread(current.x + 1, current.y - 1, queue, visited);

                    break;
                case 5:
                    TrySpread(current.x - 1, current.y + 1, queue, visited);

                    break;
                case 6:
                    TrySpread(current.x + 1, current.y + 1, queue, visited);

                    break;
                case 7:
                    TrySpread(current.x - 1, current.y - 1, queue, visited);

                    break;

                default:
                    break;
            }*/

            TrySpread(current.x, current.y + 1, queue, visited);
            TrySpread(current.x, current.y - 1, queue, visited);
            TrySpread(current.x + 1, current.y, queue, visited);
            TrySpread(current.x - 1, current.y, queue, visited);
            //TrySpread(current.x + 1, current.y - 1, queue, visited);
            //TrySpread(current.x - 1, current.y + 1, queue, visited);
            //TrySpread(current.x + 1, current.y + 1, queue, visited);
            //TrySpread(current.x - 1, current.y - 1, queue, visited);
        }
    }

    private void TrySpread(int x, int y, Queue<(int x, int y)> queue, HashSet<(int x, int y)> visited)
    {
        if (x < 0 || x >= Grid.GetLength(0) || y < 0 || y >= Grid.GetLength(1))
        {
            return;
        }

        if (!visited.Add((x, y)))
        {
            return;
        }

        if (Grid[x, y] == null)
        {
            return;
        }

        if (Grid[x, y].GenerationType != BlockGenerationType.Base)
        {
            return;
        }


        queue.Enqueue((x, y));
    }
    #endregion

    #region Layer Helpers
    private int GetLayerStart(int layerIndex)
    {
        if (layerIndex == 0)
            return 0;

        return data.LayersDepth[layerIndex - 1] + 1;
    }
    private int GetLayerEnd(int layerIndex)
    {
        return data.LayersDepth[layerIndex];
    }
    private int GetLayerHeight(int layerIndex)
    {
        return GetLayerEnd(layerIndex) - GetLayerStart(layerIndex) + 1;
    }
    #endregion

    private void InstantiateGrid()
    {
        int width = data.Width;
        int depth = data.Depth;

        Vector3 origin = transform.position;

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                BlockData blockData = Grid[x, y];

                Block prefab = GetPrefab(blockData);

                float z = (float)rand.Next(-2, 3) / rand.Next(20, 30);

                Block block = Instantiate(prefab, new Vector3(origin.y - x, origin.x - y, z), Quaternion.identity, this.transform);

                block.name = $"{prefab.name} [{x},{y}]";
            }
        }
    }
    private Block GetPrefab(BlockData blockData)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].Data == blockData)
                return blocks[i];
        }

        return null;
    }



    #region Block in Layer Organisation
    private void GroupBlocksInSameLayer()
    {
        GenerateDepthLayers();

        for (int i = 0; i < blocks.Count; i++)
        {
            Block block = blocks[i];
            List<int> blockLayers = block.Data.Layers;

            for (int j = 0; j < blockLayers.Count; j++)
            {
                int layerIndex = blockLayers[j];

                switch (block.Data.GenerationType)
                {
                    case BlockGenerationType.Base:
                        layers[layerIndex].BaseBlock = block;
                        break;

                    case BlockGenerationType.StoneVariant:
                        layers[layerIndex].StoneVariants.Add(block);
                        break;

                    case BlockGenerationType.Ore:
                        layers[layerIndex].Ores.Add(block);
                        break;
                }

                //Debug.Log($"{block.name} was added to layer with Index {layerIndex}");
            }
        }
    }

    private void GenerateDepthLayers()
    {
        layers.Clear();

        for (int i = 0; i < data.LayersDepth.Count; i++)
        {
            layers.Add(new Layer());
        }
    }
    #endregion

    private class Layer
    {
        public Block BaseBlock;
        public List<Block> StoneVariants = new();
        public List<Block> Ores = new();
    }
}
