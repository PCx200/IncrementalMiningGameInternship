using TMPro;
using UnityEngine;

public class SkillLevelView : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private Skill skill;

    private void Start()
    {
        OnSkillLeveled(skill);
    }

    private void OnEnable()
    {
        GameManager.Instance.SkillTree.OnSkillLeveled += OnSkillLeveled;
    }

    private void OnDisable()
    {
        GameManager.Instance.SkillTree.OnSkillLeveled -= OnSkillLeveled;
    }

    private void OnSkillLeveled(Skill leveledSkill)
    {
        if (leveledSkill != skill)
        {
            return;
        }

        UpdateLevelText();
    }

    private void UpdateLevelText()
    {
        levelText.text = $"{skill.CurrentLevel}/{skill.Data.MaxLevel}";
    }


}
