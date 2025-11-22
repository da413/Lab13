using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayFabManager: MonoBehaviour 
{
    private LoginManager loginManager;
    private string savedEmailKey = "SavedEmail";
    private string userEmail;

    [SerializeField] TMP_InputField email;
    [SerializeField] TMP_InputField password;
   

    void Start()
    {
        loginManager = new LoginManager();
        //check if email is saved
        if (PlayerPrefs.HasKey(savedEmailKey))
        {
            string savedEmail = PlayerPrefs.GetString(savedEmailKey);
            //Auto-login with saved email
            EmailLoginButtonClicked();
        }
        
    }

    //Example method for triggering email login
    public void EmailLoginButtonClicked()
    {
        
        userEmail = email.ToString();
        loginManager.SetLoginMethod(new EmailLogin(email.text, password.text));
        loginManager.Login(OnLoginSuccess, OnLoginFailure);
    }

    public void DeviceIDLoginButtonClicked(string deviceID)
    {
        loginManager.SetLoginMethod(new DeviceLogin(deviceID));
        loginManager.Login(OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successful");
        if (!string.IsNullOrEmpty(userEmail))
        {
            PlayerPrefs.SetString(savedEmailKey, userEmail);
            LoadPlayerData(result.PlayFabId);
        }
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed: " + error.ErrorMessage);
    }

    private void LoadPlayerData(string playFabId)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = playFabId
        };
        PlayFabClientAPI.GetUserData(request, OnDataSuccess, OnDataFailure);
    }

    private void OnDataSuccess(GetUserDataResult result)
    {
        Debug.Log("Player data loaded successfully");
    }

    private void OnDataFailure(PlayFabError error)
    {
        Debug.LogError("Failed to load player data: " + error.ErrorMessage);
    }
}
