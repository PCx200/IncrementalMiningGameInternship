using System;
using System.Collections.Generic;

public class DrillStatManager
{
    private readonly DrillData baseData;

    private readonly Dictionary<DrillModifierSource, List<DrillStatModifier>> modifiers = new();

    private readonly Dictionary<DrillStat, float> values = new();

    public event Action OnStatsChanged;

    public DrillStatManager(DrillData baseData)
    {
        this.baseData = baseData;

        Recalculate();
    }

    public float GetValue(DrillStat stat)
    {
        return values.TryGetValue(stat, out float value) ? value : GetBaseValue(stat);
    }

    public void SetModifiers(DrillModifierSource source, IEnumerable<DrillStatModifier> newModifiers)
    {
        if (!modifiers.ContainsKey(source))
        {
            modifiers[source] = new List<DrillStatModifier>();
        }

        modifiers[source].Clear();


        if (newModifiers != null)
        {
            modifiers[source].AddRange(newModifiers);
        }

        Recalculate();

        OnStatsChanged?.Invoke();
    }

    public void ClearModifiers(DrillModifierSource source)
    {
        if (!modifiers.ContainsKey(source))
        {
            return;
        }

        modifiers[source].Clear();

        Recalculate();

        OnStatsChanged?.Invoke();
    }

    private void Recalculate()
    {
        foreach (DrillStat stat in Enum.GetValues(typeof(DrillStat)))
        {
            float value = GetBaseValue(stat);

            foreach (var source in modifiers.Values)
            {
                foreach (DrillStatModifier modifier in source)
                {
                    if (modifier.Stat != stat)
                    {
                        continue;
                    }

                    switch (modifier.ModifierOperation)
                    {
                        case ModifierOperation.Add:
                            value += modifier.Value;
                            break;
                        case ModifierOperation.Multiply:
                            value *= 1f + modifier.Value;
                            break;
                        default:
                            break;
                    }
                }
            }

            values[stat] = value;
        }
    }

    private float GetBaseValue(DrillStat stat)
    {
        return stat switch
        {
            DrillStat.AttackDamage => baseData.AttackDamage,

            DrillStat.AttackSpeed => baseData.AttackSpeed,

            DrillStat.CriticalChance => baseData.CriticalChance,

            DrillStat.CriticalDamage => baseData.CriticalDamage,

            DrillStat.FuelTankCapacity => baseData.FuelTankCapacity,

            DrillStat.FuelConsumptionPerSecond => baseData.FuelConsumptionPerSecond,

            //default
            _ => 0f
        };
    }
}