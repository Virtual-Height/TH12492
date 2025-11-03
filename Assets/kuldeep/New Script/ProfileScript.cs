using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class ProfileScript : MonoBehaviour
{
    [Header("USER INPUT FIELDS")]
    public InputField fullNameField;
    public InputField numberInputField;
    public Dropdown genderDropdown;
    public InputField cityField;
    public InputField stateField;
    public InputField pincodeField;

    [Header("BUTTONS & POPUPS")]
    public Button signupButton;
    public Text EnterAllTxt;
    public GameObject ErrorPopup;

    string userMessageText;

    [Header("SCREEN")]
    public GameObject signinScreen;
    public GameObject verifyPopupScreen;
    public GameObject travelPopupScreen;

    [Header("OTP SCREEN")]
    public Text EnterOtpTxt;
    public Button otpVerifyBtn;
    public float otpTimeRemaining = 10.0f;
    public bool OtpstartTime = false;
    public GameObject otpResendBtn;
    public Text otpMsgText;
    public Button otpResendButton;
    public InputField otpInputFields;
    public Text verifyCodeEmailText;

    [System.Serializable]
    public class Data { }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public Data data;
    }

    private void Start()
    {
        signupButton.onClick.AddListener(Signup);
        otpVerifyBtn.onClick.AddListener(VerifyCode);
        otpResendButton.onClick.AddListener(ResendCode);

        if (genderDropdown != null && genderDropdown.options.Count > 0)
        {
            genderDropdown.value = 0;
        }

        PlayerPrefs.SetString("Gender", "other");
        PlayerPrefs.Save();
    }

    private void Signup()
    {
        string emailOrPhone = numberInputField.text;
        Regex phoneRegex = new Regex(@"^[6-9]\d{9}$");
        bool isPhone = phoneRegex.IsMatch(emailOrPhone);

        if (string.IsNullOrEmpty(fullNameField.text))
            ShowError("Please Enter Your Full Name");
        else if (!isPhone)
            ShowError("Please Enter a valid 10-digit Mobile Number");
        else if (string.IsNullOrEmpty(cityField.text))
            ShowError("Please Enter Your City");
        else if (string.IsNullOrEmpty(stateField.text))
            ShowError("Please Enter Your State");
       /* else if (!Regex.IsMatch(pincodeField.text, @"^\\d{6}$"))
            ShowError("Please Enter a valid 6-digit Pincode");*/
        else
            StartCoroutine(SignupRequest());
    }

    private void ShowError(string message)
    {
        EnterAllTxt.text = message;
        EnterAllTxt.gameObject.SetActive(true);
        ErrorPopup.SetActive(true);
    }

    private IEnumerator SignupRequest()
    {
        string firstName = fullNameField.text;
        string phone = numberInputField.text;
        string gender = genderDropdown.options[genderDropdown.value].text;
        string city = cityField.text;
        string state = stateField.text;
        string pincode = pincodeField.text;

        // ✅ Save user data locally for next scenes
        PlayerPrefs.SetString("Name", firstName);
        PlayerPrefs.SetString("Number", phone);
        PlayerPrefs.SetString("Gender", gender);
        PlayerPrefs.SetString("City", city);
        PlayerPrefs.SetString("State", state);
        PlayerPrefs.SetString("Pincode", pincode);
        PlayerPrefs.Save(); // force save immediately

        Debug.Log("✅ City Saved: " + city);

        string signupUrl = commonUrl.url + "/api/mobile/auth/visitor/send-otp";
        WWWForm form = new WWWForm();
        form.AddField("phoneNumber", phone);

        UnityWebRequest www = UnityWebRequest.Post(signupUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            var data = www.downloadHandler.text;
            Root userMessage = JsonUtility.FromJson<Root>(data);
            userMessageText = userMessage.message;
            ShowError(userMessageText);
        }
        else
        {
            verifyPopupScreen.SetActive(true);
            verifyCodeEmailText.text = "Please enter the 4 digit code sent to \n " + phone;
            otpTimeRemaining = 60f;
            OtpstartTime = true;
        }
    }

    private void ResendCode() => StartCoroutine(ResendCodeRequest());

    private IEnumerator ResendCodeRequest()
    {
        string masterId = PlayerPrefs.GetString("masterId");
        string verifyCodeUrl = commonUrl.url + "/resend-otp-signup";
        WWWForm form = new WWWForm();
        form.AddField("masterId", masterId);

        UnityWebRequest www = UnityWebRequest.Post(verifyCodeUrl, form);
        yield return www.SendWebRequest();

        if (!www.isNetworkError && !www.isHttpError)
        {
            otpTimeRemaining = 60f;
            OtpstartTime = true;
            otpMsgText.gameObject.SetActive(true);
            otpResendBtn.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (OtpstartTime)
        {
            otpTimeRemaining -= Time.deltaTime;
            if (otpTimeRemaining > 0)
                otpMsgText.text = "Resend in " + otpTimeRemaining.ToString("0") + " Secs";
            else
            {
                OtpstartTime = false;
                otpMsgText.gameObject.SetActive(false);
                otpResendBtn.gameObject.SetActive(true);
            }
        }
    }

    public void clickSignin()
    {
        signinScreen.SetActive(true);
        fullNameField.text = "";
        numberInputField.text = "";
        cityField.text = "";
        stateField.text = "";
        pincodeField.text = "";
        genderDropdown.value = 0;
    }

    public void VerifyCode()
    {
        if (otpInputFields.text.Length == 0)
            ShowError("Please enter valid code");
        else
            StartCoroutine(VerifyCodeRequest());
    }

    private IEnumerator VerifyCodeRequest()
    {
        string platform =
            Application.platform == RuntimePlatform.Android ? "mobile_app" :
            Application.platform == RuntimePlatform.WebGLPlayer ? "web_metaverse" :
            "vr_metaverse";

        string otpValue = otpInputFields.text;
        string verifyCodeUrl = commonUrl.url + "/api/mobile/auth/visitor/verify-otp";

        WWWForm form = new WWWForm();
        form.AddField("phoneNumber", PlayerPrefs.GetString("Number"));
        form.AddField("otp", otpValue);
        form.AddField("name", PlayerPrefs.GetString("Name"));
        form.AddField("platform", platform);
        form.AddField("gender", PlayerPrefs.GetString("Gender"));
        form.AddField("city", PlayerPrefs.GetString("City"));
        form.AddField("state", PlayerPrefs.GetString("State"));
        form.AddField("pincode", PlayerPrefs.GetString("Pincode"));

        UnityWebRequest www = UnityWebRequest.Post(verifyCodeUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            travelPopupScreen.SetActive(true);
        }
        else
        {
            Debug.Log("✅ Verification successful");
            verifyPopupScreen.SetActive(false);
            signinScreen.SetActive(false);
            otpInputFields.text = "";

            CityNameSetup setup = FindObjectOfType<CityNameSetup>();
            if (setup != null)
            {
                setup.RefreshCityData();
            }

            travelPopupScreen.SetActive(true);
        }
    }
}