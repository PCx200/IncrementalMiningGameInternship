using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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
        Initialize();
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            SEED = 0;
            Initialize();
        }
    }

    private void Initialize()
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


        //ClusterOreGenerator clusterOreGenerator = new ClusterOreGenerator(data, layers, tilemapGenerator.Tilemap);
        //clusterOreGenerator.GenerateOres();

        PerlinOreGenerator perlinOreGenerator = new PerlinOreGenerator(data, layers, tilemapGenerator.Tilemap);
        perlinOreGenerator.GenerateOres();

        //NoiseGenerator noiseGenerator = new NoiseGenerator(noiseMap);

        //noiseMap = noiseGenerator.GeneratePerlinNoiseMap(data.Width, data.Depth, 0.01f, 3, 0.5f, 2.0f);

        InstantiateGrid();
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

                //float zz = noiseMap[x, y];

                Block block = Instantiate(prefab, new Vector3(origin.y - x, origin.x - y, z), Quaternion.identity, this.transform);

                block.name = $"{prefab.name} [{x},{y}]";
            }
        }
    }
}
