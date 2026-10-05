using UnityEngine;

public abstract class EquipmentData : ScriptableObject
{
    [Header("Part Info")]
    public string Name;
    public Sprite Icon;

    //TODO:: Add currency after merge!

    [Header("Stat Modifiers")]
    public float AttackDamage;
    public float AttackSpeed;
    public float CriticalChance;
    public float CriticalDamage;
    public float FuelTankCapacity;
    public float FuelConsumptionPerSecond;
}
