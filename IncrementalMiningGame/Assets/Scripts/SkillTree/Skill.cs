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
        GameManager.Instance.SkillTree.UnregisterSkill(this);
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
        return data.Description;
    }
}
