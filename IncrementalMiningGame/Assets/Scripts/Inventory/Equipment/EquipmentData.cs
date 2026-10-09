using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentData : ScriptableObject
{
    [Header("Part Info")]
    public string Name;
    public Sprite Icon;

    public SlotType SlotType;

    //TODO:: Add currency after merge!

    [Header("Modifiers")]
    public EquipmentModifierDefinition EquipmentModifierDefinition;
}
