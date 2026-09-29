using TMPro;
using UnityEngine;

public class SkillDescriptionView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Skill skill;

    private void Start()
    {
        ShowDescription();
    }

    private void OnEnable()
    {
        SkillTree.OnSkillLeveled += ShowDescription;
    }

    private void OnDisable()
    {
        SkillTree.OnSkillLeveled -= ShowDescription; 
    }

    private void ShowDescription()
    {
        descriptionText.text = $"{skill.GetDescription()}";
    }
}
