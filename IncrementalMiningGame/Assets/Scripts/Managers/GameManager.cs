using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private SkillTree skillTree;
    public SkillTree SkillTree => skillTree;

    private ModifierManager modifierManager;
    public ModifierManager ModifierManager => modifierManager;
    private AbilityManager abilityManager;
    public AbilityManager AbilityManager => abilityManager;

    private SkillIntegrationManager skillIntegrationManager;
    public SkillIntegrationManager SkillIntegrationManager => skillIntegrationManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        skillTree = new SkillTree();
        modifierManager = new ModifierManager();
        abilityManager = new AbilityManager();

        skillIntegrationManager = new SkillIntegrationManager(skillTree, modifierManager, abilityManager);
    }

    private void Start()
    {
    }

}
