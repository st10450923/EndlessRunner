using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Authentication;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    private PlayerControls playerControls;
    public GameObject LeaderboardParent;
    public Transform LeaderboardContentParent;
    public Transform leaderboardPrefab;

    private static string LeaderboardID = "Leaderboard";
    private async Task Start()
    {
        await UnityServices.InitializeAsync();
        if(!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardID,0);
        LeaderboardParent.SetActive(false);
    }
    private void OnEnable()
    {
        UpdateLeaderboard();
    }

    private async void UpdateLeaderboard()
    {
        while (Application.isPlaying && LeaderboardParent.activeInHierarchy)
        {
            LeaderboardScoresPage leaderboardScoresPage = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardID);

            foreach(Transform t in LeaderboardContentParent)
            {
                Destroy(t.gameObject);
            }
            
            foreach (Unity.Services.Leaderboards.Models.LeaderboardEntry entry in leaderboardScoresPage.Results)
            {
                Transform leaderboardItem = Instantiate(leaderboardPrefab, LeaderboardContentParent);
                leaderboardItem.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.Rank.ToString();
                leaderboardItem.GetChild(1).GetComponent<TextMeshProUGUI>().text = entry.PlayerName;
                leaderboardItem.GetChild(2).GetComponent<TextMeshProUGUI>().text = entry.Score.ToString();
            }
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardID);

        }
    }
}
