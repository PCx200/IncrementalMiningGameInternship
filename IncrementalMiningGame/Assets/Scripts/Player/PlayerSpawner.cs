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
        Vector3 gridCenter = new Vector3(Mathf.Floor(gridData.Width / 2.0f), player.transform.localScale.x * 2.0f, 0.0f);

        Instantiate(player, gridCenter, Quaternion.identity);
    }
}
