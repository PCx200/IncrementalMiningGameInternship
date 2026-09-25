using System.Collections;
using TMPro;
using UnityEngine;

public class BlocksCollectedView : MonoBehaviour
{
    private Inventory inventory;

    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TextMeshProUGUI blocksText;

    private void Awake()
    {
        blocksText.text = "";
    }

    private void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();

        RoundManager.Instance.OnRoundEnd += ShowCollectedBlocks;
    }

    private void OnDisable()
    {
        RoundManager.Instance.OnRoundEnd -= ShowCollectedBlocks;
    }

    private void ShowCollectedBlocks()
    {
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        panel.SetActive(true);
        blocksText.text = "";

        foreach (var block in inventory.GetBlocks())
        {
            yield return new WaitForSeconds(0.3f);
            //Debug.Log($"{block.Key.name} x{block.Value}");
            blocksText.text += $"{block.Key.name} x{block.Value}\n";

        }
    }
}
