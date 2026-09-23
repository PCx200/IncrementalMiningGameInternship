using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float Health;

    public float MovementSpeed;

    public float CarryingStrength;

    public DrillData DrillData;

    //Add Inventory property later;
}
