using UnityEngine;

public class NextButtonHandler : MonoBehaviour
{
    public void OnNextButtonClicked()
    {
        CityNameSetup setup = FindObjectOfType<CityNameSetup>();
        if (setup != null)
            setup.RefreshCityData();
    }
}
