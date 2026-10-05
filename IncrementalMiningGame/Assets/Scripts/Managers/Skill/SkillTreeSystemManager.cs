public class SkillTreeSystemManager
{
    private SkillTree skillTree;
    private ModifierManager modifierManager;
    private AbilityManager abilityManager;

    public SkillTreeSystemManager(SkillTree skillTree, ModifierManager modifierManager, AbilityManager abilityManager)
    {
        this.skillTree = skillTree;
        this.modifierManager = modifierManager;
        this.abilityManager = abilityManager;

        skillTree.OnSkillLeveled += Rebuild;
    }

    private void Rebuild(Skill skill)
    {
        var progression = skillTree.GetProgression();

        modifierManager.Rebuild(progression);
        abilityManager.Rebuild(progression);
    }
}
