using System.Collections.Generic;
using UnityEngine;


public class TilemapManager : MonoBehaviour
{
    [SerializeField] private int SEED;

    [SerializeField] private GridData data;
    public GridData Data => data;

    [SerializeField] List<BlockData> blocks;

    private TilemapGenerator<BlockData> tilemapGenerator;

    private BlockDataTilemap blockDataTilemap;

    [SerializeField] private List<LayerData> layers = new();

    //float[,] noiseMap;

    private void Start()
    {
        if (SEED == 0)
        {
            SEED = Seed.GenerateSeed();
        }
        else
        {
            Seed.PickSeed(SEED);
        }

        tilemapGenerator = new TilemapGenerator<BlockData>(new BlockData[data.Width, data.Depth]);

        blockDataTilemap = new BlockDataTilemap(data, tilemapGenerator, layers);
        blockDataTilemap.GenerateBaseGridData();


        OreGenerator oreGenerator = new OreGenerator(data, layers, tilemapGenerator.Tilemap);
        oreGenerator.GenerateOres();

        InstantiateGrid();

        //NoiseGenerator noise = new NoiseGenerator(noiseMap);

        //noise.GeneratePerlinNoise(data.Width, data.Depth, 0.001f, 3, 0.5f, 3.0f);
    }

    private void InstantiateGrid()
    {
        int width = data.Width;
        int depth = data.Depth;

        Vector3 origin = transform.position;

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                BlockData blockData = tilemapGenerator.Tilemap[x, y];

                Block prefab = blockData.Prefab;

                float z = (float)Seed.RandomINT(-2, 3) / Seed.RandomINT(20, 30);

                Block block = Instantiate(prefab, new Vector3(origin.y - x, origin.x - y, z), Quaternion.identity, this.transform);

                block.name = $"{prefab.name} [{x},{y}]";
            }
        }
    }
}
