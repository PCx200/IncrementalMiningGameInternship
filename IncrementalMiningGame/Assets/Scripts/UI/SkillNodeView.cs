using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillNodeView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Skill skill;

    [SerializeField]
    private GameObject descriptionPanel;

    [SerializeField]
    private GameObject upgradeButton;

    private void Awake()
    {
        skill = GetComponent<Skill>();
    }

    private void Start()
    {
        Render();
    }

    private void OnEnable()
    {
        SkillTree.OnSkillLeveled += Render;
    }

    private void OnDisable()
    {
        SkillTree.OnSkillLeveled -= Render;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }

    private void Render()
    {
        // If there are no prerequisites unlocked, hide the button
        // If there is some unlocked, make it visible, but disable the interaction.
        // If all conditions are met, be able to upgrade the skill.
        bool hasUnlockedPrerequisite = skill.HasAnyUnlockedPrerequisite();
        bool isUnlocked = skill.IsUnlocked();

        if (!hasUnlockedPrerequisite)
        {
            upgradeButton.SetActive(false);
            return;
        }


        Button button = upgradeButton.GetComponent<Button>();
        button.interactable = isUnlocked;

        upgradeButton.SetActive(true);
    }
}
