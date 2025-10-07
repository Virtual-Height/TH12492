using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class KeyboardManager : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private InputField inputField;

    public float distance = 0.5f;
    public float verticleOffSet = -0.5f;
    public Transform playerPos;

    private void Start()
    {
        inputField = GetComponent<InputField>();
    }

    // Called automatically when the InputField is selected (clicked or focused)
    public void OnSelect(BaseEventData eventData)
    {
        OpenKeyboard();
    }

    public void OpenKeyboard()
    {
        if (NonNativeKeyboard.Instance.gameObject.activeSelf)
        {
            NonNativeKeyboard.Instance.Close();
        }

        NonNativeKeyboard.Instance.InputField = inputField;
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);

        inputField.MoveTextEnd(false);
        NonNativeKeyboard.Instance.OnClosed += Instance_OnClosed;

        Vector3 direction = playerPos.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPos = playerPos.position + direction * distance + Vector3.up * verticleOffSet;

        NonNativeKeyboard.Instance.RepositionKeyboard(targetPos);
        SetCaretColor(1);
    }

    private void Instance_OnClosed(object sender, System.EventArgs e)
    {
        SetCaretColor(0);
        NonNativeKeyboard.Instance.InputField = null;
        NonNativeKeyboard.Instance.OnClosed -= Instance_OnClosed;
    }

    public void SetCaretColor(float alphaValue)
    {
        inputField.customCaretColor = true;
        Color caretColor = inputField.caretColor;
        caretColor.a = alphaValue;
        inputField.caretColor = caretColor;
        inputField.MoveTextEnd(false);
    }
}
