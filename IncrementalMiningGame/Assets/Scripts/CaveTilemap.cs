using System.Collections.Generic;
using UnityEngine;

public class BlockDataTilemap
{
    private GridData data;
    private TilemapGenerator<BlockData> tilemapGenerator;

    [SerializeField] 
    private List<LayerData> layers = new();


    public BlockDataTilemap(GridData data, TilemapGenerator<BlockData> tilemapGenerator, List<LayerData> layers)
    {
        this.data = data;
        this.tilemapGenerator = tilemapGenerator;
        this.layers = layers;
    }

    public void GenerateBaseGridData()
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
                tilemapGenerator.Tilemap[x, y] = baseBlockData;
            }
        }
    }
}
