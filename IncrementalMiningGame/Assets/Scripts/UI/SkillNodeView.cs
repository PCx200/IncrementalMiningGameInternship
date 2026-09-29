using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillNodeView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Skill skill;

    [SerializeField]
    private GameObject upgradeButton;

    [Header("Descriptions")]
    [SerializeField]
    private GameObject descriptionPanel;

    [SerializeField] private Transform descriptionsContainer;


    [Header("Connections")]
    [SerializeField]
    private RectTransform line;

    [SerializeField]
    private Transform connectionsContainer;

    private bool isConnected;

    private void Awake()
    {
        skill = GetComponent<Skill>();

        descriptionPanel.transform.parent = descriptionsContainer;
    }

    private void Start()
    {
        DisplaySkill();
    }

    private void OnEnable()
    {
        SkillTree.OnSkillLeveled += DisplaySkill;
    }

    private void OnDisable()
    {
        SkillTree.OnSkillLeveled -= DisplaySkill;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }

    private void DisplaySkill()
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

        if (!isConnected)
        {
            ConnectSkill();
        }

        Button button = upgradeButton.GetComponent<Button>();
        button.interactable = isUnlocked;

        upgradeButton.SetActive(true);
    }

    private void ConnectSkill()
    {
        RectTransform currentSkill = GetComponent<RectTransform>();

        var prerequisites = skill.Data.Prerequisites;

        foreach (var prerequisite in prerequisites)
        {
            Skill prerequisiteSkill = SkillTree.GetSkill(prerequisite.SkillData);

            RectTransform prerequisiteSkillRectTransform = prerequisiteSkill.GetComponent<RectTransform>();

            Vector3 start = currentSkill.position;
            Vector3 end = prerequisiteSkillRectTransform.position;

            Vector3 direction = end - start;

            float distance = direction.magnitude;

            RectTransform connection = Instantiate(line, currentSkill.parent);

            connection.parent = connectionsContainer;

            connection.position = (start + end) / 2f;
            connection.sizeDelta = new Vector2(distance, line.sizeDelta.y);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            connection.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        isConnected = true;
    }
}
