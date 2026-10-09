using UnityEngine;
using UnityEngine.UI;

public class FuelView : MonoBehaviour
{
    [SerializeField]
    private Image fuelImage;

    private DrillController drill;

    private void OnEnable()
    {
        DrillRegistry.Instance.OnDrillRegistered += RegisterDrill;

        if (DrillRegistry.Instance.CurrentDrill != null)
        {
            RegisterDrill(DrillRegistry.Instance.CurrentDrill);
        }
    }

    private void OnDisable()
    {
        DrillRegistry.Instance.OnDrillRegistered -= RegisterDrill;
    }

    private void RegisterDrill(DrillController drillController)
    {
        this.drill = drillController;
    }

    private void Update()
    {
        fuelImage.fillAmount = drill.CurrentFuel / drill.Data.FuelTankCapacity;
    }
}
