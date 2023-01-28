using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private GameObject currentDisplay;
    [SerializeField] private GameObject mainMenuDisplay;
    [SerializeField] private GameObject tutorialDisplay;
    [SerializeField] private GameObject optionsDisplay;
    [SerializeField] private GameObject levelSelectDisplay;
    [SerializeField] private GameObject[] tutorialScreens;
    private MusicPlayer musicPlayer;


    // Start is called before the first frame update
    void Awake()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>();
        musicPlayer.SwitchToMenuMusic();
        mainMenuDisplay.SetActive(true);
        currentDisplay = mainMenuDisplay;
    }

    public void LoadLevel(int level)
    {
        musicPlayer.SwitchToBattleMusic();
        SceneManager.LoadScene(level);
    }

    public void OpenMainMenu()
    {
        currentDisplay.SetActive(false);
        mainMenuDisplay.SetActive(true);
        currentDisplay = mainMenuDisplay;
    }

    public void OpenLevelSelect()
    {
        currentDisplay.SetActive(false);
        levelSelectDisplay.SetActive(true);
        currentDisplay = levelSelectDisplay;
    }

    public void OpenTutorial()
    {
        currentDisplay.SetActive(false);
        tutorialDisplay.SetActive(true);
        currentDisplay = tutorialDisplay;
    }

    public void OpenOptions()
    {
        currentDisplay.SetActive(false);
        optionsDisplay.SetActive(true);
        currentDisplay = optionsDisplay;
    }

    public void OpenTutorialScreen(int screenNumber)
    {
        foreach (GameObject screen in tutorialScreens)
        {
            screen.SetActive(false);
        }
        tutorialScreens[screenNumber - 1].SetActive(true);
    }
}
