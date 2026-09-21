using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float Health;

    public float MovementSpeed;

    public float CarryingStrength;

    public DrillData drillData;

    //Add Inventory property later;
}
