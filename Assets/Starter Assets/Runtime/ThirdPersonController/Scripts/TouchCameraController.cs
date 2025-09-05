using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class TouchCameraController : MonoBehaviour
{
    public float sensitivity = 1f;
    private StarterAssetsInputs _input;

    private bool isDragging;

    public Slider rotSensitivitySlider;
    public Text sensitivityValueText;

    private float deviceMultiplier = 10f;

    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();

#if UNITY_EDITOR
        deviceMultiplier = 0.1f;
#elif UNITY_IOS
        deviceMultiplier = 0.1f;
#elif UNITY_ANDROID
        string model = SystemInfo.deviceModel.ToLower();
        if (model.Contains("samsung"))
        {
            deviceMultiplier = 0.1f;
        }
#endif

        if (rotSensitivitySlider != null)
        {
            sensitivity = rotSensitivitySlider.value;
            if (sensitivityValueText != null)
                sensitivityValueText.text = sensitivity.ToString("F2");

            rotSensitivitySlider.onValueChanged.AddListener((float value) =>
            {
                sensitivity = value;
                if (sensitivityValueText != null)
                    sensitivityValueText.text = value.ToString("F2");
            });
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2f)
                {
                    isDragging = true;
                }

                if (isDragging && touch.position.x > Screen.width / 2f)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 delta = touch.deltaPosition * sensitivity * deviceMultiplier;

                        delta.y = -delta.y;

                        _input.look = delta;
                    }
                    else
                    {
                        _input.look = Vector2.zero;
                    }

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        isDragging = false;
                        _input.look = Vector2.zero;
                    }
                }
            }
        }
        else
        {
            _input.look = Vector2.zero;
        }
    }
}
