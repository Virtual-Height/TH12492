using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class Counseller : MonoBehaviour
{
    public TextMeshPro chatPopup;
    public string[] dialogues;
    int a;
    bool isTalking;

    public InputActionReference dialogue_Input;

    private void Awake()
    {
        this.dialogue_Input.action.started += this.ContinueDialogue;
    }
    private void Start()
    {
        a = 0;
    }
    public IEnumerator SpawnChatPopUp(string sentence)
    {
        chatPopup.text = null;
        foreach(char letter in sentence.ToCharArray())
        {
            chatPopup.text += letter;
            yield return null;
        }
    }

    public void StartDialogue()
    {
        StartCoroutine(SpawnChatPopUp(dialogues[a]));
        isTalking = true;
        GetComponent<Animator>().SetBool("IsTalking", true);
        a++;
    }

    public void ContinueDialogue(InputAction.CallbackContext context)
    {
        if (isTalking)
        {
            this.StartCoroutine(SpawnChatPopUp(dialogues[a]));
            this.GetComponent<Animator>().SetBool("IsTalking", true);
            a++;
            if (a >= dialogues.Length)
            {
                //end
                this.isTalking= false;
                this.GetComponent<Animator>().SetBool("IsTalking", false);
                this.dialogue_Input.action.started -= this.ContinueDialogue;
                StartCoroutine(ClearPopup());
            }
        }
    }
    IEnumerator ClearPopup()
    {
        yield return new WaitForSeconds(5f);
        this.chatPopup.text = null;
    }
}