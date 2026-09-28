using System;
using UnityEngine;

public class Skill
{
    [SerializeField]
    private SkillData data;
    public SkillData Data => data;

    private int currentLevel;
    public int CurrentLevel => currentLevel;

    public event Action OnLevelUp;

    public Skill(SkillData data)
    {
        this.data = data;
        currentLevel = 0;
    }

    public bool IsMaxed()
    {
        return currentLevel >= data.MaxLevel;
    }

    public void LevelUp(PlayerController player)
    {
        if (IsMaxed())
        {
            return;
        }
        
        currentLevel++;

        data.Effect?.Apply(player, this);

        OnLevelUp?.Invoke();
    }

    public int GetCost()
    {
        return Mathf.RoundToInt(data.Cost * Mathf.Pow(data.CostMultiplier, currentLevel));
    }

    public float GetMultiplier()
    {
        return data.MultiplierPerLevel[currentLevel];
    }

    public bool IsUnlocked()
    {
        foreach (var skillPrerequisite in data.Prerequisites)
        {
            Skill requiredSkill = SkillTree.GetSkill(skillPrerequisite.SkillData);

            if (requiredSkill.CurrentLevel < skillPrerequisite.RequiredLevel)
            {
                return false;
            }
        }

        return true;
    }

    public string GetDescription()
    {
        string description;

        description = $"{data.Description} +{GetMultiplier()}%";

        return description;
    }
}
