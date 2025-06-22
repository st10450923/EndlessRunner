using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
public class MenuManager : MonoBehaviour
{
    public static MenuManager Inst;
    public string LevelName;
    public GameObject MainMenu;
    public GameObject Leaderboard;

    private void Awake()
    {
        Inst = this;
    }

    public void OpenLeaderboard()
    {
        MainMenu.SetActive(false);
        Leaderboard.SetActive(true);
    }
    public void OpenMainMenu()
    {
        MainMenu.SetActive(true);
        Leaderboard.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene(LevelName);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
