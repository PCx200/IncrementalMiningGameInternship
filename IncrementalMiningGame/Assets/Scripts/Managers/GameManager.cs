using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private SkillTree skillTree = new SkillTree();
    public SkillTree SkillTree => skillTree;

    private ModifierManager modifierManager = new ModifierManager();
    public ModifierManager ModifierManager => modifierManager;

    private SkillIntegrationManager skillIntegrationManager = new SkillIntegrationManager();
    public SkillIntegrationManager SkillIntegrationManager => skillIntegrationManager;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

}
