using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;


public class Layer
{
    public List<Block> Blocks;

    public Layer()
    {
        Blocks = new();
    }
}

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private int SEED;
    private System.Random rand;

    [SerializeField] private GridData data;
    public GridData Data => data;

    [SerializeField] List<Block> blocks;

    public Block[,] Grid;

    List<Layer> layers = new();




    private void Start()
    {
        GenerateSeed();
        GroupBlocksInSameLayer();

        Grid = new Block[data.Width, data.Depth];

        int layerIndex = 0;
        int blockIndex = 0;

        for (int i = 0; i < data.Depth; i++)
        {
            for (int j = 0; j < data.Width; j++)
            {
                if (i > data.LayersDepth[layerIndex])
                {
                    layerIndex++;
                }

                blockIndex = rand.Next(0, layers[layerIndex].Blocks.Count);

                Block prefab = layers[layerIndex].Blocks[blockIndex];
                Block block = Instantiate(prefab, new Vector3(transform.position.y - j, transform.position.x - i, 0), Quaternion.identity, this.transform);
                
                block.name = $"{prefab.name} [{i},{j}]";
            }
        }
    }

    private void GenerateSeed()
    {
        if (SEED != 0)
        {
            rand = new System.Random(SEED);
            Debug.Log(SEED);
        }
        else
        {
            SEED = Random.Range(int.MinValue, int.MaxValue);
            rand = new System.Random(SEED);
            Debug.Log(SEED);
        }

    }



    private void GroupBlocksInSameLayer()
    {
        List<Layer> layers = GenerateDepthLayers();

        for (int i = 0; i < blocks.Count; i++)
        {
            for (int j = 0; j < layers.Count; j++)
            {
                for (int k = 0; k < blocks[i].Data.Layers.Count; k++)
                {
                    if (blocks[i].Data.Layers[k] == j)
                    {
                        layers[j].Blocks.Add(blocks[i]);
                        Debug.Log($"{blocks[i].name} was added to layer with Index {j}");
                    }
                }

            }
        }
    }

    private List<Layer> GenerateDepthLayers()
    {
        for (int i = 0; i < data.LayersDepth.Count; i++)
        {
            Layer layer = new Layer();

            layers.Add(layer);
        }

        return layers;
    }
}
