using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public string firstSceneName = "GameScene"; // your first scene
    public static class SceneHistory
    {
        //save the current scene name its on
        public static string previousScene = "";
    }

    public GameObject settingsPanel;

    public void STARTGAME()
    {
       
        // Load the first scene
        SceneManager.LoadScene(firstSceneName);
    }

    public void SETTINGS()
    {
        //after pressing rules the rules scene will load
        SceneManager.LoadScene("Rules");
    }

    //open up settings panel
    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void OpenRules()
    {
        //save current scene
        SceneHistory.previousScene = SceneManager.GetActiveScene().name;

        //load rules scene
        SceneManager.LoadScene("Rules");
    }
    public void Back()
    {
        //go back to previous scene else start scene
        if (!string.IsNullOrEmpty(SceneHistory.previousScene))
        {
            SceneManager.LoadScene(SceneHistory.previousScene);
        }
        else
        {
            SceneManager.LoadScene("StartScreen");
        }
    }

    public void EXIT()
    {
        //pressing exit will quit the application as a whole
        Application.Quit();
        Debug.Log("Game is quitting...");
    }

    public void NEWGAME()
    {
        //destroy everything
        SceneHistory.previousScene = "";

        //restarts game
        SceneManager.LoadScene(0);
        Debug.Log("Restarting...");

    }
}
