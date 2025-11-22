using PlayFab.ClientModels;
using PlayFab;

public class DeviceLogin : ILogin
{
    private string deviceID;
   
    public DeviceLogin(string deviceID)
    {
        this.deviceID = deviceID;
    }

    public void Login(System.Action<LoginResult> onSuccess, System.Action<PlayFabError> onFailure)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = deviceID,
            CreateAccount = true //creat account if not exists
        };
        PlayFabClientAPI.LoginWithCustomID(request, onSuccess, onFailure);
    }
}
