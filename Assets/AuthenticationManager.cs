using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Inst;
    public TMPro.TMP_InputField Name;
    public Button LeaderboardButton;
    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(Inst); 
        }
    }
    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void SignIn()
    {
        await SignInAnonymously();
    }

    async Task SignInAnonymously()
    {
        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            print("Sign in successful");
            print("Player ID: "+AuthenticationService.Instance.PlayerId);
            await AuthenticationService.Instance.UpdatePlayerNameAsync(Name.text.Split('#')[0]);
            EnableLeaderboardButton();
        }
        catch(System.Exception)
        {
            print("Sign in failed");
            Debug.Log("Exception");
        }
    }
    void EnableLeaderboardButton()
    {
        LeaderboardButton.interactable = true;
    }
}
