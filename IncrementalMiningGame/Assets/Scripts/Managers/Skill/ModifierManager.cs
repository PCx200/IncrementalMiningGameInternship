using System.Collections.Generic;
using UnityEngine;

public class ModifierManager
{
    private Dictionary<string, List<ActiveModifier>> modifiers = new();

    public ModifierManager()
    {

    }

    public void Rebuild(IReadOnlyDictionary<SkillData, int> skillLevels)
    {
        modifiers.Clear();

        foreach (var pair in skillLevels)
        {
            SkillData skillData = pair.Key;
            int level = pair.Value;

            if (skillData == null || level <= 0)
            {
                continue;
            }

            if (skillData.SkillOutput is not SkillModifier modifier)
            {
                continue;
            }

            if (!modifiers.TryGetValue(modifier.StatID, out List<ActiveModifier> activeModifiers))
            {
                activeModifiers = new List<ActiveModifier>();
                modifiers.Add(modifier.StatID, activeModifiers);
            }

            activeModifiers.Add(new ActiveModifier
            {
                Modifier = modifier,
                Level = level
            });
        }
    }

    public float GetValue(string statID, float baseValue)
    {
        if (!modifiers.TryGetValue(statID, out List<ActiveModifier> list))
        {
            return baseValue;
        }

        float value = baseValue;

        foreach (ActiveModifier modifier in list)
        {
            float modifierValue = modifier.Modifier.GetValue(modifier.Level);

            switch (modifier.Modifier.ModifierOperation)
            {
                case ModifierOperation.Add:
                    value += modifierValue;
                    break;

                case ModifierOperation.Multiply:
                    value *= 1f + modifierValue;
                    break;
            }
        }

        return value;
    }
}
