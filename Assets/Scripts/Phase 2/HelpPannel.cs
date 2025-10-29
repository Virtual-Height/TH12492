using UnityEngine;
using UnityEngine.UI;

public class HelpPannel : MonoBehaviour
{
    public static HelpPannel instance;

    public Button[] options;

    private void Awake()
    {
        instance = this;
    }

    public void SetupOptions(int option)
    {
        for(int i = 0; i < options.Length; i++)
        {
            if(i == option)
            {
                options[i].onClick.AddListener(() =>
                {
                    GameManager.instance.messageText.text = "Correct";
                    GameManager.instance.popupPannel.gameObject.SetActive(true);
                    Destroy(GameManager.instance.selectedAI.gameObject, 5f);
                    GameManager.instance.selectedAI = null;
                });
            }
            else
            {
                options[i].onClick.AddListener(() =>
                {
                    GameManager.instance.messageText.text = "Incorrect";
                    GameManager.instance.popupPannel.gameObject.SetActive(true);
                    Destroy(GameManager.instance.selectedAI.gameObject, 5f);
                    GameManager.instance.selectedAI = null;
                });
            }
        }
    }
}
