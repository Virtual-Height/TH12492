using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

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
    public class Data
    {  

    }

    [System.Serializable]
    public class Root
    {
        public string message;
        public string phoneNumber;
        public string expiresIn;
    }


    private void Start()
    {
        signupButton.onClick.AddListener(Signup);
        otpVerifyBtn.onClick.AddListener(VerifyCode);
        otpResendButton.onClick.AddListener(ResendCode);
       
        PlayerPrefs.SetString("Gender", "other");
        PlayerPrefs.Save();
    }

    private void Signup()
    {
        string emailOrPhone = numberInputField.text;
        Regex phoneRegex = new Regex(@"^[6-9]\d{9}$");
        bool isPhone = phoneRegex.IsMatch(emailOrPhone);
        Regex alphanumericRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[@#$%^&+=!])");
        Regex spaceRegex = new Regex(@"\s");

        if (fullNameField.text.Length == 0 || spaceRegex.IsMatch(fullNameField.text))
        {
           ShowError("Please Enter Your Full Name");
        }
        /* else if (!isPhone|| spaceRegex.IsMatch(numberInputField.text))
         {
             ShowError("Please Enter a valid 10-digit Mobile Number");
         }*/

        else if (string.IsNullOrEmpty(numberInputField.text))
        {
            ShowError("Please Enter Your Mobile Number");
        }
        else if (spaceRegex.IsMatch(numberInputField.text))
        {
            ShowError("Mobile Number should not contain spaces");
        }
        else if (!isPhone)
        {
            ShowError("Please Enter a valid 10-digit Mobile Number");
        }
        else if (genderDropdown == null || genderDropdown.value == 0)
        {
            ShowError("Please Select Your Gender");
        }

        else if (string.IsNullOrEmpty(cityField.text) || spaceRegex.IsMatch(cityField.text))
        {
            ShowError("Please Enter Your City");
        }
        else if (string.IsNullOrEmpty(stateField.text) || spaceRegex.IsMatch(stateField.text))
        {
            ShowError("Please Enter Your State");
        }

        else if (string.IsNullOrEmpty(pincodeField.text))
        {
            ShowError("Please Enter Your Pincode");
        }
        else if (spaceRegex.IsMatch(pincodeField.text))
        {
            ShowError("Pincode should not contain spaces");
        }
        // <-- Fixed pincode regex here -->
        else if (!Regex.IsMatch(pincodeField.text, @"^\d{6}$"))
        {
            ShowError("Please Enter a valid 6-digit Pincode");
        }

        else
        {
            StartCoroutine(SignupRequest());
        }
    }

    private void ShowError(string message)
    {
        EnterAllTxt.text = message;
        EnterAllTxt.gameObject.SetActive(true);
        ErrorPopup.SetActive(true);
    }

    private IEnumerator SignupRequest()
    {

        Debug.Log("SignupRequest is calll ....");

        string firstName = fullNameField.text;
        string phone = numberInputField.text;
        string gender = genderDropdown.options[genderDropdown.value].text.ToLower();
        string city = cityField.text;
        string state = stateField.text;
        string pincode = pincodeField.text;

        string platform;
        if (Application.platform == RuntimePlatform.Android)
        {
            platform = "mobile_app";
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            platform = "web_metaverse";
        }
        else
        {
            platform = "vr_metaverse";
        }

        Debug.Log(firstName);
        Debug.Log(phone);
        Debug.Log(gender);
        Debug.Log(city);
        Debug.Log(state);
        Debug.Log(pincode);
        Debug.Log(platform);


        // ✅ Save user data locally for next scenes
        PlayerPrefs.SetString("Name", firstName);
        PlayerPrefs.SetString("Number", phone);
        PlayerPrefs.SetString("Gender", gender);
        PlayerPrefs.SetString("City", city);
        PlayerPrefs.SetString("State", state);
        PlayerPrefs.SetString("Pincode", pincode);
        PlayerPrefs.SetString("Platform", platform);
        PlayerPrefs.Save(); // force save immediately

        Debug.Log("✅ City Saved: " + city);

        //string signupUrl = commonUrl.url + "/api/mobile/auth/visitor/enhanced/send-otp";
        string signupUrl = "https://ujjain-admin.appworkdemo.com/api/mobile/auth/visitor/enhanced/send-otp";

        

        Debug.Log(signupUrl);

        WWWForm form = new WWWForm();

        form.AddField("name", firstName);
        form.AddField("phoneNumber", phone);
        form.AddField("gender", gender);
        form.AddField("city", city);
        form.AddField("state", state);
        form.AddField("pincode", pincode);
        form.AddField("platform", platform);

        UnityWebRequest www = UnityWebRequest.Post(signupUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("registerError=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
            Root userMessage = JsonUtility.FromJson<Root>(data);
            userMessageText = userMessage.message;
            ShowError(userMessageText);
        }
        else
        {
            Debug.Log(www.result);
            var data = www.downloadHandler.text;
            Debug.Log("data=" + data);
            Root userData = JsonUtility.FromJson<Root>(data);

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
        string otpValue = otpInputFields.text;
        //string verifyCodeUrl = commonUrl.url + "/api/mobile/auth/visitor/verify-otp";

        string verifyCodeUrl = "https://ujjain-admin.appworkdemo.com/api/mobile/auth/visitor/enhanced/verify-otp";

        WWWForm form = new WWWForm();

        form.AddField("phoneNumber", PlayerPrefs.GetString("Number"));
        form.AddField("otp", otpValue);

        UnityWebRequest www = UnityWebRequest.Post(verifyCodeUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //travelPopupScreen.SetActive(true);

            Debug.LogError(www.error);
            Debug.Log("verify=" + www.downloadHandler.text);
            EnterOtpTxt.text = "Invalid Code";
            EnterOtpTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
            StartCoroutine(EnterProperOtp());

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

    IEnumerator EnterProperOtp()
    {
        yield return new WaitForSeconds(3);
        EnterOtpTxt.gameObject.SetActive(false);
    }
}