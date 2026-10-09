using UnityEngine;
using UnityEngine.UI;

public class ScrapBinView : MonoBehaviour
{
    [SerializeField]
    private Image scrapBinVisual;

    private void Awake()
    {
        scrapBinVisual.enabled = false;
    }

    public void Show()
    {
        scrapBinVisual.enabled = true;
    }

    public void Hide()
    {
        scrapBinVisual.enabled = false;
    }
}
