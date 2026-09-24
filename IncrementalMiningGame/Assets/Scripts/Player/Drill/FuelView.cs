using UnityEngine;
using UnityEngine.UI;

public class FuelView : MonoBehaviour
{
    [SerializeField]
    private Image fuelImage;

    [SerializeField]
    private DrillController drill;

    private void Update()
    {
        fuelImage.fillAmount = drill.CurrentFuel / drill.Data.FuelTankCapacity;
    }
}
