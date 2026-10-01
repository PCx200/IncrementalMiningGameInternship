using System.Collections.Generic;
using UnityEngine;

public class ModifierManager
{
    private List<SkillModifier> modifiers = new();

    public void AddModifier(SkillModifier modifier)
    {
        modifiers.Add(modifier);
    }

    public ModifierManager()
    {
            
    }
}
