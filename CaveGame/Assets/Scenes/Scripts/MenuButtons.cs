using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public string firstSceneName = "GameScene"; // your first scene
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    public GameObject controlsPanel;
   

    public static class SceneHistory
    {
        //save the current scene name its on
        public static string previousScene = "";
    }

    void Update()
    {
        // Only check ESC if a pause menu exists
        if (pauseMenuPanel != null && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void STARTGAME()
    {
        // Make sure time is running and cursor is locked
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Load the first scene
        SceneManager.LoadScene("Advice");
    }

    //open up settings panel
    public void ToggleSettingsPanel()
    {
        if (settingsPanel == null) return;

        // Toggle settings panel visibility
        settingsPanel.SetActive(!settingsPanel.activeSelf);

        // If a pause menu exists and is active, hide it
        if (pauseMenuPanel != null && pauseMenuPanel.activeSelf)
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    public void TogglePauseMenu()
    {
        // If pause menu exists
        if (pauseMenuPanel == null) return;

        // If pause menu is currently active, close both pause menu and settings panel
        if (pauseMenuPanel.activeSelf)
        {
            pauseMenuPanel.SetActive(false);

            if (settingsPanel != null)
                settingsPanel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
        else
        {
            // Pause menu is not active, open it
            pauseMenuPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }
    //open up settings panel
    public void ToggleControlsPanel()
    {
        if (controlsPanel == null) return;

        // Toggle settings panel visibility
        controlsPanel.SetActive(!controlsPanel.activeSelf);

        // If a pause menu exists and is active, hide it
        if (pauseMenuPanel != null && pauseMenuPanel.activeSelf)
        {
            pauseMenuPanel.SetActive(false);
        }
    }
    public void RETURN()
    {
        // Hide settings panel
        settingsPanel.SetActive(false);

        // Show pause menu again
        pauseMenuPanel.SetActive(true);
    }

    public void EXIT()
    {
        //pressing exit will quit the application as a whole
        Application.Quit();
        Debug.Log("Game is quitting...");
    }

    public void STARTOVER()
    {
        // Reset everything
        Time.timeScale = 1f;
       

        // Clear previous scene history
        SceneHistory.previousScene = "";

        // Load first scene (restart game)
        SceneManager.LoadScene(0);

        Debug.Log("Restarting...");

    }
}
