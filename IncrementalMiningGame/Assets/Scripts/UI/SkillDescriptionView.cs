using TMPro;
using UnityEngine;

public class SkillDescriptionView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Skill skill;

    private void Start()
    {
        nameText.text = skill.Data.Name;

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

        ShowDescription();
    }

    private void ShowDescription()
    {
        SkillModifier modifier =
         skill.Data.SkillOutput as SkillModifier;

        if (modifier == null)
        {
            descriptionText.text = skill.GetDescription();
            return;
        }

        int nextLevel = skill.CurrentLevel + 1;
    

        if (nextLevel <= skill.Data.MaxLevel)
        {
            float nextValue = modifier.GetValue(nextLevel);
            descriptionText.text = $"{skill.GetDescription()} +{nextValue * 100f:0.#}%";
        }
        else
        {
            float maxLevelValue = modifier.GetValue(skill.CurrentLevel);
            descriptionText.text = $"{skill.GetDescription()} +{maxLevelValue * 100f:0.#}%";
        }
    }
}
