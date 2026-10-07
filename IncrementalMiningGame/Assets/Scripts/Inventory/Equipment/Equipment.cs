using System.Collections.Generic;

public class Equipment
{
    private EquipmentData data;
    public EquipmentData Data => data;

    private readonly DrillStatModifier primaryModifier;
    public DrillStatModifier PrimaryModifier => primaryModifier;

    private readonly List<DrillStatModifier> secondaryModifiers;
    public List<DrillStatModifier> SecondaryModifiers => secondaryModifiers;

    public Equipment(EquipmentData data, DrillStatModifier primaryModifier, List<DrillStatModifier> secondaryModifiers)
    {
        this.data = data;
        this.primaryModifier = primaryModifier;
        this.secondaryModifiers = secondaryModifiers;

        if (secondaryModifiers != null)
        {
            this.secondaryModifiers.AddRange(secondaryModifiers);
        }
    }

    // TEMPORARY CTOR
    public Equipment(EquipmentData data)
    {
        this.data = data;

        primaryModifier = new DrillStatModifier(data.PrimaryModifier.DrillStat, data.PrimaryModifier.ModifierOperation, data.PrimaryModifier.MaxValue);
    }
}
