using System.Collections.Generic;

public abstract class OreGenerator
{
    protected readonly GridData data;
    protected readonly List<LayerData> layers;
    protected readonly BlockData[,] grid;


    public OreGenerator(GridData data, List<LayerData> layers, BlockData[,] grid)
    {
        this.data = data;
        this.layers = layers;
        this.grid = grid;
    }

    public virtual void GenerateOres()
    {
       
    }

    #region Layer Helpers
    protected int GetLayerStart(int layerIndex)
    {
        if (layerIndex == 0)
        { 
            return 0;
        }

        return data.LayersDepth[layerIndex - 1] + 1;
    }

    protected int GetLayerEnd(int layerIndex)
    {
        return data.LayersDepth[layerIndex] - 1;
    }
    #endregion
}
