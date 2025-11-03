using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main Transport Buttons")]
    public Button flightButton;
    public Button railButton;
    public Button busButton;
    public Button nextButton;

    [Header("Popup Panels")]
    public GameObject flightPopup;
    public GameObject railPopup;
    public GameObject busPopup;

   

    private string selectedTransport = null;
   

    private void Start()
    {
        // Step 1 - Initialize
        nextButton.interactable = false;
        

        flightPopup.SetActive(false);
        railPopup.SetActive(false);
        busPopup.SetActive(false);
        
        // Transport selections
        flightButton.onClick.AddListener(() => OnTransportSelected("Flight"));
        railButton.onClick.AddListener(() => OnTransportSelected("Rail"));
        busButton.onClick.AddListener(() => OnTransportSelected("Bus"));

        nextButton.onClick.AddListener(OnNextClicked);

     
        
    }

    private void OnTransportSelected(string type)
    {
        selectedTransport = type;
        nextButton.interactable = true;
        Debug.Log("Selected Transport: " + selectedTransport);
    }

    private void OnNextClicked()
    {
        if (string.IsNullOrEmpty(selectedTransport)) return;

        // Hide all popups
        flightPopup.SetActive(false);
        railPopup.SetActive(false);
        busPopup.SetActive(false);
        

        // Show popup for selected transport
        switch (selectedTransport)
        {
            case "Flight":
                flightPopup.SetActive(true);
                break;
            case "Rail":
                railPopup.SetActive(true);
                break;
            case "Bus":
                busPopup.SetActive(true);
                break;
        }

        Debug.Log("Opened popup for: " + selectedTransport);
    }
   
}
