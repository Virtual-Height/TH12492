using UnityEngine;
using UnityEngine.UI;

public class AvatarSelection : MonoBehaviour
{
    public GameObject male;
    public GameObject female;

    public GameObject avatarPannel;
    public GameObject uiCamera;

    public GameObject verifyPopupScreen;

    public PathVisualizer pathVisualizer;
    public Transform maleTransform;
    public Transform femaleTransform;



    private void Start()
    {
        female.SetActive(false);
        male.SetActive(true);
        pathVisualizer.playerTransform = maleTransform;
        PlayerPrefs.SetString("Gender", "male");
    }

    public void OnMaleSelect()
    {
        female.SetActive(false);
        male.SetActive(true);
        pathVisualizer.playerTransform = maleTransform;
        PlayerPrefs.SetString("Gender", "male");
    }

    public void OnFemaleSelect()
    {
        male.SetActive(false);
        female.SetActive(true);
        pathVisualizer.playerTransform = femaleTransform;
        PlayerPrefs.SetString("Gender", "female");
    }

    public void OnSelectBtn()
    {
        avatarPannel.SetActive(false);
        uiCamera.SetActive(false);
        verifyPopupScreen.SetActive(true);
    }
}
