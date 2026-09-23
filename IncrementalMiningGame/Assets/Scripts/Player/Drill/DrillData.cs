using UnityEngine;

[CreateAssetMenu(fileName = "DrillData", menuName = "Scriptable Objects/DrillData")]
public class DrillData : ScriptableObject
{
    public float AttackDamage;

    public float AttackSpeed;

    public float FuelTankCapacity;

    public float FuelConsumptionPerSecond;

    public float ReachDistance;
}
