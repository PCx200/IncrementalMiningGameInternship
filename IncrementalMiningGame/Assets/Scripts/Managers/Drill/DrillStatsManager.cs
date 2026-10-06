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
        if (!modifiers.TryGetValue(source, out List<DrillStatModifier> list))
        {
            list = new List<DrillStatModifier>();
            modifiers[source] = list;
        }

        list.Clear();

        if (newModifiers != null)
        {
            list.AddRange(newModifiers);
        }

        Recalculate();

        OnStatsChanged?.Invoke();
    }

    public void ClearModifiers(DrillModifierSource source)
    {
        if (!modifiers.TryGetValue(source, out List<DrillStatModifier> list))
        {
            return;
        }

        list.Clear();

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

                    if (modifier.ModifierOperation == ModifierOperation.Add)
                    {
                        value += modifier.Value;
                    }
                }
            }

            foreach (var source in modifiers.Values)
            {
                foreach (DrillStatModifier modifier in source)
                {
                    if (modifier.Stat != stat)
                    {
                        continue;
                    }

                    if (modifier.ModifierOperation == ModifierOperation.Multiply)
                    {
                        value *= 1f + modifier.Value;
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