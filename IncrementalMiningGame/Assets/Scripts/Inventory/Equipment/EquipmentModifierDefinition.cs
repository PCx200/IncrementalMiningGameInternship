using System.Collections.Generic;

[System.Serializable]
public class EquipmentModifierDefinition
{
    public EquipmentStatDefinition PrimaryModifier;

    public List<EquipmentStatDefinition> SecondaryModifiers;

    public List<SecondaryModifierCountChance> SecondaryModifierCountChances;
}