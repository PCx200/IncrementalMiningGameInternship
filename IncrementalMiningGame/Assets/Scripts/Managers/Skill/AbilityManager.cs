using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    private readonly HashSet<string> unlockedAbilities = new();

    public void Rebuild(IReadOnlyDictionary<SkillData, int> skillLevels)
    {
        unlockedAbilities.Clear();

        foreach (var pair in skillLevels)
        {
            SkillData skillData = pair.Key;
            int level = pair.Value;

            if (skillData == null || level <= 0)
            {
                continue;
            }

            if (skillData.SkillOutput is SkillUnlock unlock)
            {
                unlockedAbilities.Add(unlock.AbilityID);
            }
        }
    }

    public bool IsUnlocked(string abilityID)
    {
        return unlockedAbilities.Contains(abilityID);
    }
}
