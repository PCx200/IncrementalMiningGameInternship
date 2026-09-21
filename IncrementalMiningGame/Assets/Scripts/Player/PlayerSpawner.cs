using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] 
    private PlayerController player;

    [SerializeField]
    private TilemapManager tilemapManager;

    private GridData gridData;

    private void Start()
    {
        gridData = tilemapManager.Data;

        Spawn();
    }

    private void Spawn()
    {
        Vector3 gridCenter = new Vector3(gridData.Width / 2, player.transform.localScale.x * 2f, 0);

        Instantiate(player, gridCenter, Quaternion.identity);
    }
}
