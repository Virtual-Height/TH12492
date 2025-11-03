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
        for (int i = 0; i < options.Length; i++)
        {
            options[i].onClick.RemoveAllListeners();

            if (i == option)
            {
                options[i].onClick.AddListener(() =>
                {
                    GameManager.instance.messageText.text = "Correct";
                    GameManager.instance.popupPannel.SetActive(true);
                    if(GameManager.instance.selectedAI != null)
                    {
                        Destroy(GameManager.instance.selectedAI.gameObject, 5f);
                    }
                    GameManager.instance.selectedAI = null;
                });
            }
            else
            {
                options[i].onClick.AddListener(() =>
                {
                    GameManager.instance.messageText.text = "Incorrect";
                    GameManager.instance.popupPannel.SetActive(true);
                    if (GameManager.instance.selectedAI != null)
                    {
                        Destroy(GameManager.instance.selectedAI.gameObject, 5f);
                    }
                    GameManager.instance.selectedAI = null;
                });
            }
        }

        if(option == 2)
        {
            options[2].onClick.AddListener(() =>
            {
                GameManager.instance.popupPannel.SetActive(false);
                GameManager.instance.firePannel.SetActive(true);
            });
        }
    }
}