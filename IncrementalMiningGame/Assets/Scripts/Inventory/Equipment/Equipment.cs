using System.Collections.Generic;

public class Equipment
{
    private EquipmentData data;
    public EquipmentData Data => data;

    private readonly DrillStatModifier primaryModifier;
    public DrillStatModifier PrimaryModifier => primaryModifier;

    private readonly List<DrillStatModifier> secondaryModifiers = new();
    public List<DrillStatModifier> SecondaryModifiers => secondaryModifiers;

    public Equipment(EquipmentData data, DrillStatModifier primaryModifier, List<DrillStatModifier> secondaryModifiers)
    {
        this.data = data;
        this.primaryModifier = primaryModifier;
        this.secondaryModifiers.AddRange(secondaryModifiers);
    }

    // TEMPORARY CTOR
    public Equipment(EquipmentData data)
    {
        this.data = data;

        primaryModifier = new DrillStatModifier(data.EquipmentModifierDefinition.PrimaryModifier.DrillStat, data.EquipmentModifierDefinition.PrimaryModifier.ModifierOperation, data.EquipmentModifierDefinition.PrimaryModifier.MaxValue);
    }
}
