using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<BlockData, int> blocks = new();

    [SerializeField]
    private BlockDestroyChannel blockDestroyChannel;

    private void OnEnable()
    {
        blockDestroyChannel.Raised += AddBlock;
    }

    private void OnDisable()
    {
        blockDestroyChannel.Raised -= AddBlock;
    }

    private void AddBlock(BlockData data)
    {
        if (blocks.TryGetValue(data, out int count))
        {
            blocks[data] = count + 1;
        }
        else
        {
            blocks.Add(data, 1);
        }
    }

    public IEnumerable<KeyValuePair<BlockData, int>> GetBlocks()
    {
        return blocks;
    }
}
