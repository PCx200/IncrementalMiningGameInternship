[System.Serializable]
public struct DrillStatModifier
{
    public DrillStat Stat;
    public ModifierOperation ModifierOperation;
    public float Value;

    public DrillStatModifier(DrillStat stat, ModifierOperation modifierOperation, float value)
    {
        this.Stat = stat;
        this.ModifierOperation = modifierOperation;
        this.Value = value;
    }
}
