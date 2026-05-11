using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private GameObject playButton;
    private GameObject settingsButton;
    private GameObject quitButton;
    private GameObject coOpButton;
    private GameObject localButton;
    private GameObject arenaButton;
    private GameObject backButton;




    void Start()
    {
        // Find all buttons by their tags
        playButton = GameObject.FindGameObjectWithTag("playButton");
        settingsButton = GameObject.FindGameObjectWithTag("settingsButton");
        quitButton = GameObject.FindGameObjectWithTag("quitButton");
        coOpButton = GameObject.FindGameObjectWithTag("coOpButton");
        localButton = GameObject.FindGameObjectWithTag("localButton");
        arenaButton = GameObject.FindGameObjectWithTag("changearenaButton");
        backButton = GameObject.FindGameObjectWithTag("backButton");

        // Initial setup: Show play, settings, quit buttons and hide coop button
        ShowInitialButtons();

        // Add click listener to play button
        if (playButton != null)
        {
            Button playButtonComponent = playButton.GetComponent<Button>();
            if (playButtonComponent != null)
            {
                playButtonComponent.onClick.AddListener(OnPlayButtonClick);
            }
            else
            {
                Debug.LogError("Play button doesn't have a Button component!");
            }
        }
        else
        {
            Debug.LogError("Play button not found!");
        }

        if (settingsButton != null)
        {
            Button settingComponent = settingsButton.GetComponent<Button>();
            if (settingComponent != null)
            {
                settingComponent.onClick.AddListener(OnSettingbuttonClick);
            }
            else
            {
                Debug.LogError("Play button doesn't have a Button component!");
            }
        }
        else
        {
            Debug.LogError("Play button not found!");
        }

        if (quitButton != null)
        {
            Button quitComponent = quitButton.GetComponent<Button>();
            if (quitComponent != null)
            {
                quitComponent.onClick.AddListener(OnQuitButtonClick);
            }
            else
            {
                Debug.LogError("Quit button doesn't have a Button component!");
            }
        }
        else
        {
            Debug.LogError("Quit button not found!");
        }
        //backbutton
        if (backButton != null)
        {
            Button backComponent = backButton.GetComponent<Button>();
            if (backComponent != null)
            {
                backComponent.onClick.AddListener(OnBackButtonClick);
            }
            else
            {
                Debug.LogError("Back button doesn't have a Button component!");
            }
        }
        else
        {
            Debug.LogError("Back button not found!");
        }

        // Add local button listener
        if (localButton != null)
        {
            Button localComponent = localButton.GetComponent<Button>();
            if (localComponent != null)
            {
                localComponent.onClick.AddListener(OnLocalButtonClick);
            }
            else
            {
                Debug.LogError("Local button doesn't have a Button component!");
            }
        }
        else
        {
            Debug.LogError("Local button not found!");
        }
    }

    void ShowInitialButtons()
    {
        // Show play, settings, and quit buttons
        if (playButton) playButton.SetActive(true);
        if (settingsButton) settingsButton.SetActive(true);
        if (quitButton) quitButton.SetActive(true);
        // Hide coop button
        if (coOpButton) coOpButton.SetActive(false);
        if (localButton) localButton.SetActive(false);
        if (arenaButton) arenaButton.SetActive(false);
        if (backButton) backButton.SetActive(false);
    }

    void OnPlayButtonClick()
    {
        // Hide all buttons except coop
        if (playButton) playButton.SetActive(false);
        if (settingsButton) settingsButton.SetActive(false);
        if (quitButton) quitButton.SetActive(false);
        if (arenaButton) arenaButton.SetActive(false);
        // Show coop button
        if (coOpButton) coOpButton.SetActive(true);
        if (localButton) localButton.SetActive(true);
        if (backButton) backButton.SetActive(true);

    }

    void OnLocalButtonClick()
    {
        // Hide all buttons except back button
        if (playButton) playButton.SetActive(false);
        if (settingsButton) settingsButton.SetActive(false);
        if (quitButton) quitButton.SetActive(false);
        if (coOpButton) coOpButton.SetActive(false);
        if (localButton) localButton.SetActive(false);
        if (arenaButton) arenaButton.SetActive(false);

        // Show only back button
        if (backButton) backButton.SetActive(true);

        // Here you would add any additional code to start the local game mode
        Debug.Log("Starting local game mode");
    }

    void OnSettingbuttonClick()
    {
        if (playButton) playButton.SetActive(false);
        if (settingsButton) settingsButton.SetActive(false);
        if (quitButton) quitButton.SetActive(false);
        if (coOpButton) coOpButton.SetActive(false);
        if (localButton) localButton.SetActive(false);


        if (arenaButton) arenaButton.SetActive(true);
        if (backButton) backButton.SetActive(true);
    }

    void OnQuitButtonClick()
    {
        // Quit the application (works in built game, not in editor)
        Debug.Log("Quitting application");
        Application.Quit();

        // For testing in Unity Editor (since Application.Quit() doesn't work in editor)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //on back button
    void OnBackButtonClick()
    {
        // Return to the home screen
        // Hide all game mode and settings buttons
        if (coOpButton) coOpButton.SetActive(false);
        if (localButton) localButton.SetActive(false);
        if (arenaButton) arenaButton.SetActive(false);
        if (backButton) backButton.SetActive(false);

        // Show home screen buttons
        if (playButton) playButton.SetActive(true);
        if (settingsButton) settingsButton.SetActive(true);
        if (quitButton) quitButton.SetActive(true);
    }
}