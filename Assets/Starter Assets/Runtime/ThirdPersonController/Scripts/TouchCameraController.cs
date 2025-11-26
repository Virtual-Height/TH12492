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

    // Rotation limits

    public float minVerticalAngle = -60f; // Min vertical angle

    public float maxVerticalAngle = 60f;  // Max vertical angle

    private float currentVerticalAngle = 0f; // To track the current vertical angle

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

                        // Calculate the touch delta position

                        Vector2 delta = touch.deltaPosition * sensitivity * deviceMultiplier;

                        // Invert the y-axis to make it work properly

                        delta.y = -delta.y;

                        // Update horizontal rotation (no limits for horizontal)

                        _input.look.x = delta.x;

                        // Update vertical rotation and clamp it to the defined range

                        currentVerticalAngle += delta.y;

                        // Clamp the vertical rotation within the defined limits

                        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);

                        // Apply the clamped vertical rotation to the input look

                        _input.look.y = currentVerticalAngle;

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

