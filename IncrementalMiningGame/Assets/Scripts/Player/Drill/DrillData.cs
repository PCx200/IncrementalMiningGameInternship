using UnityEngine;

[CreateAssetMenu(fileName = "DrillData", menuName = "Scriptable Objects/DrillData")]
public class DrillData : ScriptableObject
{
    public float AttackDamage;

    public float AttackSpeed;

    public float CriticalChance;

    [Tooltip("Critical Damage value is multiplied by 100 -> Value 1 -> 100% of the Attack Damage")]
    public float CriticalDamage;

    public float ReachDistance;

    public float FuelTankCapacity;

    public float FuelConsumptionPerSecond;
}
