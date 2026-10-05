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

        descriptionPanel.transform.SetParent(descriptionsContainer);

        // Prevents the button from being active the first frame after loading
        Button button = upgradeButton.GetComponent<Button>();
        button.interactable = false;
        upgradeButton.SetActive(false);
    }

    private void Start()
    {
        DisplaySkill();
    }

    private void OnEnable()
    {
        GameManager.Instance.SkillTree.OnSkillLeveled += OnSkillLeveled;
    }   
    

    private void OnDisable()
    {
         GameManager.Instance.SkillTree.OnSkillLeveled -= OnSkillLeveled;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }

    private void OnSkillLeveled(Skill leveledSkill)
    {
        DisplaySkill();
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


        if (skill.Data.Prerequisites == null ||
            skill.Data.Prerequisites.Count == 0)
        {
            isConnected = true;
            return;
        }

        foreach (var prerequisite in prerequisites)
        {
            if (!GameManager.Instance.SkillTree.TryGetSkill(prerequisite.SkillData, out Skill prerequisiteSkill))
            {
                continue;
            }

            RectTransform prerequisiteSkillRectTransform = prerequisiteSkill.GetComponent<RectTransform>();

            Vector3 start = currentSkill.anchoredPosition;
            Vector3 end = prerequisiteSkillRectTransform.anchoredPosition;

            Vector3 direction = end - start;

            float distance = direction.magnitude;

            RectTransform connection = Instantiate(line, currentSkill.parent);

            connection.SetParent(connectionsContainer);

            connection.anchoredPosition = (start + end) / 2f;
            connection.sizeDelta = new Vector2(distance, line.sizeDelta.y);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            connection.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        isConnected = true;
    }
}
