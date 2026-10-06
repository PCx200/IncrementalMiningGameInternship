using System.Collections.Generic;
using UnityEngine;

public enum SlotType { 
    ImpactAmplifier,
    Head,
    GyroModule,
    FuelTank,
    FuelInjector,
    DriveMotor
}

public abstract class EquipmentData : ScriptableObject
{
    [Header("Part Info")]
    public string Name;
    public Sprite Icon;

    public SlotType SlotType;

    //TODO:: Add currency after merge!

    [Header("Primary Modifier")]
    public EquipmentModifierDefinition PrimaryModifier;

    [Header("Secondary Modifier Pool")]
    public List<EquipmentModifierDefinition> SecondaryModifiers;

    public List<SecondaryModifierCountChance> SecondaryModifierCountChances;
}
