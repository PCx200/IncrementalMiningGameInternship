using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDescriptionView : MonoBehaviour
{
    [SerializeField]
    private GameObject descriptionPanel;

    [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private GridLayoutGroup itemStatsTexts;

    private static readonly DrillStat[] statOrder =
    {
        DrillStat.AttackDamage,
        DrillStat.AttackSpeed,
        DrillStat.CriticalChance,
        DrillStat.CriticalDamage,
        DrillStat.FuelTankCapacity,
        DrillStat.FuelConsumptionPerSecond
    };

    public void Show(Equipment equipment)
    {
        itemName.text = equipment.Data.Name;

        Dictionary<DrillStat, DrillStatModifier> combinedStats = new();

        DrillStatModifier primaryModifier = equipment.PrimaryModifier;
        combinedStats[primaryModifier.Stat] = primaryModifier;

        foreach (DrillStatModifier modifier in equipment.SecondaryModifiers)
        {
            if (combinedStats.ContainsKey(modifier.Stat))
            {
                DrillStatModifier currentModifier = combinedStats[modifier.Stat];

                currentModifier.Value += modifier.Value;

                combinedStats[modifier.Stat] = currentModifier;
            }
            else
            {
                combinedStats[modifier.Stat] = modifier;
            }
        }

        for (int i = 0; i < statOrder.Length; i++)
        {
            TextMeshProUGUI statText = itemStatsTexts.transform.GetChild(i).GetComponent<TextMeshProUGUI>();

            DrillStat stat = statOrder[i];

            if (!combinedStats.ContainsKey(stat))
            {
                statText.text = "-";
                continue;
            }

            DrillStatModifier modifier = combinedStats[stat];

            if (modifier.ModifierOperation == ModifierOperation.Multiply)
            {
                statText.text = $"{modifier.Value * 100f:0.#}%";
            }
            else
            {
                statText.text = $"{modifier.Value:0.#}";
            }
        }

        descriptionPanel.SetActive(true);
    }

    public void Hide()
    {
        descriptionPanel.SetActive(false);
    }
}