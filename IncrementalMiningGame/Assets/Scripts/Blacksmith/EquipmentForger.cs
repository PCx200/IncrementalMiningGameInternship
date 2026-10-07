using System.Collections.Generic;
using UnityEngine;

public static class EquipmentForager
{
    public static Equipment Forge(EquipmentData equipmentData)
    {
        EquipmentModifierDefinition definition = equipmentData.EquipmentModifierDefinition;

        DrillStatModifier primaryModifier = RollPrimaryModifier(definition.PrimaryModifier);

        List<DrillStatModifier> secondaryModifiers = RollSecondaryModifiers(definition);

        return new Equipment(equipmentData, primaryModifier, secondaryModifiers);
    }

    private static List<DrillStatModifier> RollSecondaryModifiers(EquipmentModifierDefinition equipmentModifierDefinition)
    {
        List<DrillStatModifier> modifiers = new();

        if (equipmentModifierDefinition.SecondaryModifiers.Count == 0)
        {
            return modifiers;
        }

        int secondaryModifierCount = RollSecondaryModifierCount(equipmentModifierDefinition.SecondaryModifierCountChances);

        for (int i = 0; i < secondaryModifierCount; i++)
        {
            int randomStatIndex = Random.Range(0, equipmentModifierDefinition.SecondaryModifiers.Count);

            EquipmentStatDefinition pickedStatDefinition = equipmentModifierDefinition.SecondaryModifiers[randomStatIndex];

            float statValue = Random.Range(pickedStatDefinition.MinValue, pickedStatDefinition.MaxValue);
            statValue = (float)System.Math.Round(statValue, 4);

            DrillStatModifier modifier = new DrillStatModifier(pickedStatDefinition.DrillStat, pickedStatDefinition.ModifierOperation, statValue);

            modifiers.Add(modifier);
        }

        return modifiers;
    }

    private static DrillStatModifier RollPrimaryModifier(EquipmentStatDefinition primaryModifier)
    {
        float statValue = Random.Range(primaryModifier.MinValue, primaryModifier.MaxValue);
        statValue = (float)System.Math.Round(statValue, 4);

        return new DrillStatModifier(primaryModifier.DrillStat, primaryModifier.ModifierOperation, statValue);
    }

    private static int RollSecondaryModifierCount(List<SecondaryModifierCountChance> secondaryModifierCountChances)
    {
        float totalWeight = 0f;

        foreach (var chance in secondaryModifierCountChances)
        {
            totalWeight += chance.Weight;
        }

        float roll = Random.Range(0f, totalWeight);

        float accumulatedWeight = 0f;

        foreach (SecondaryModifierCountChance chance in secondaryModifierCountChances)
        {
            accumulatedWeight += chance.Weight;

            if (roll <= accumulatedWeight)
            {
                return chance.Count;
            }
        }

        return 0;
    }
}
