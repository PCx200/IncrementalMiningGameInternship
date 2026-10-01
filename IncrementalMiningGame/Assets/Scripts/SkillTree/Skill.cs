using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField]
    private SkillData data;
    public SkillData Data => data;

    public int CurrentLevel => GameManager.Instance.SkillTree.GetLevel(data);

    private void Start()
    {
        GameManager.Instance.SkillTree.RegisterSkill(this);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SkillTree.UnregisterSkill(this);
        }
    }

    public void LevelUp()
    {
        GameManager.Instance.SkillTree.TryLevelUp(this);
    }

    public int GetCost()
    {
        return GameManager.Instance.SkillTree.GetCost(this);
    }

    public bool IsUnlocked()
    {
       return GameManager.Instance.SkillTree.IsUnlocked(this);
    }

    public bool HasAnyUnlockedPrerequisite()
    {
        return GameManager.Instance.SkillTree.HasAnyUnlockedPrerequisite(this);
    }

    public string GetDescription()
    {
        // + add the stat increase based on the stat modifier data after the description

        return data.Description;
    }
}
