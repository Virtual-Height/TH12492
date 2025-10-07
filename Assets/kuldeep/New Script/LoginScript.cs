using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class LoginScript : MonoBehaviour
{
    public InputField fullNameField;
    public InputField numberInputField;
    public Button signupButton;

    public Text EnterAllTxt;
    public GameObject ErrorPopup;

    string userIDText;
    string userMessageText;

    [Header("SCREEN")]

    public GameObject signinScreen;
    public GameObject verifyPopupScreen;
    public GameObject avatarSelection;

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
        public int status;
        public string message;
        public Data data;
    }


    private void Start()
    {
        signupButton.onClick.AddListener(signup);
        otpVerifyBtn.onClick.AddListener(VerifyCode);
        otpResendButton.onClick.AddListener(ResendCode);
        PlayerPrefs.SetString("Gender", "other");
    }

    private void signup()
    {
        string emailOrPhone = numberInputField.text;
        Regex phoneRegex = new Regex(@"^[6-9]\d{9}$");
        bool isPhone = phoneRegex.IsMatch(emailOrPhone);
        Regex alphanumericRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[@#$%^&+=!])");
        Regex spaceRegex = new Regex(@"\s");

        if (fullNameField.text.Length == 0)
        {
            EnterAllTxt.text = "Please Enter Your Full Name";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else if (!isPhone)
        {
            EnterAllTxt.text = "Please Enter a valid Email or 10-digit Mobile Number";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else
        {
            StartCoroutine(signupRequest());
        }

    }

    private IEnumerator signupRequest()
    {
        string firstName = fullNameField.text;
        string emailOrPhone = numberInputField.text;

        PlayerPrefs.SetString("Name", firstName);
        PlayerPrefs.SetString("Number", emailOrPhone);
        
        string signupUrl = commonUrl.url + "/api/mobile/auth/visitor/send-otp";
       
        WWWForm form = new WWWForm();

        form.AddField("phoneNumber", PlayerPrefs.GetString("Number"));

        UnityWebRequest www = UnityWebRequest.Post(signupUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("registerError=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
            Root userMessage = JsonUtility.FromJson<Root>(data);

            userMessageText = userMessage.message;
            EnterAllTxt.text = userMessageText;
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else
        {
            Debug.Log(www.result);
            var data = www.downloadHandler.text;
            Debug.Log("data=" + data);

            Root userData = JsonUtility.FromJson<Root>(data);

            //verifyPopupScreen.SetActive(true);
            avatarSelection.SetActive(true);
            verifyCodeEmailText.text = "please enter the 4 digit code sent to \n " + emailOrPhone;
            otpTimeRemaining = 60f;
            OtpstartTime = true;
        }

    }

    private void ResendCode()
    {
        StartCoroutine(ResendCodeRequest());
    }

    private IEnumerator ResendCodeRequest()
    {
        string masterId = PlayerPrefs.GetString("masterId");
        Debug.Log(masterId + ".............");

        string verifyCodeUrl = commonUrl.url + "/resend-otp-signup";
        WWWForm form = new WWWForm();
        form.AddField("masterId", masterId);

        UnityWebRequest www = UnityWebRequest.Post(verifyCodeUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            Debug.Log("resend error=" + www.downloadHandler.text);
        }
        else
        {
            otpTimeRemaining = 60f;
            OtpstartTime = true;
            otpMsgText.gameObject.SetActive(true);
            otpResendBtn.gameObject.SetActive(false);
            Debug.Log("resend =" + www.downloadHandler.text);

        }
    }

    void Update()
    {
        if (OtpstartTime) //timer
        {
            if (otpTimeRemaining > 0)
            {
                otpTimeRemaining -= Time.deltaTime;
                otpMsgText.text = "Resend in " + otpTimeRemaining.ToString("0") + " Secs";
            }

            if (otpTimeRemaining < 0)
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

    }
    IEnumerator EnterProperOtp()
    {
        yield return new WaitForSeconds(3);
        EnterOtpTxt.gameObject.SetActive(false);
    }


    public void VerifyCode()
    {
        if (otpInputFields.text.Length == 0)
        {
            EnterAllTxt.text = "Please enter valid code";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else
        {
            StartCoroutine(VerifyCodeRequest());
        }

    }

    private IEnumerator VerifyCodeRequest()
    {
        string platform;
        if(Application.platform == RuntimePlatform.Android)
        {
            platform = "mobile_app";
        }
        else if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            platform = "web_metaverse";
        }
        else
        {
            platform = "vr_metaverse";
        }
        string otpValue = otpInputFields.text;
        Debug.Log("otp=" + otpValue);
        string verifyCodeUrl = commonUrl.url + "/api/mobile/auth/visitor/verify-otp";

        WWWForm form = new WWWForm();

        form.AddField("phoneNumber", PlayerPrefs.GetString("Number"));
        form.AddField("otp", otpValue);
        form.AddField("name", PlayerPrefs.GetString("Name"));
        form.AddField("platform", platform);
        form.AddField("gender", PlayerPrefs.GetString("Gender"));

        UnityWebRequest www = UnityWebRequest.Post(verifyCodeUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            Debug.Log("verify=" + www.downloadHandler.text);
            EnterOtpTxt.text = "Invalid Code";
            EnterOtpTxt.gameObject.SetActive(true);
            StartCoroutine(EnterProperOtp());
        }
        else
        {
            Debug.Log("Verification successful");
            otpVerifyBtn.interactable = false;
            fullNameField.text = "";
            numberInputField.text = "";


            otpInputFields.text = "";

            verifyPopupScreen.SetActive(false);
            signinScreen.SetActive(false);
            otpVerifyBtn.interactable = true;
            otpTimeRemaining = 10.0f;
        }
    }

    public void backbtn()
    {
        otpInputFields.text = "";
        verifyPopupScreen.SetActive(false);
        signinScreen.SetActive(false);

        otpTimeRemaining = 10.0f;
        OtpstartTime = false;
        otpMsgText.gameObject.SetActive(true);
        otpResendBtn.gameObject.SetActive(false);
    }

}