using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static PauseMenu instance;

    public GameObject pauseMenuGameObject;
    public List<GameObject> buttons;

    [Header("Audio")]
    public AudioClip select;
    public AudioClip activate;

    AudioSource audioSource;

    int count = 0;

    private void Awake()
    {
        if (!instance)
            instance = this;

        if (!pauseMenuGameObject)
            pauseMenuGameObject = gameObject;
    }

    private void Start()
    {
        Resume();
        audioSource = GetComponent<AudioSource>();
    }

    public void Resume()
    {
        GameIsPaused = false;
        pauseMenuGameObject.SetActive(false);
        Time.timeScale = 1f;


        if (GameInstance.gameInstance.playerObject != null)
        {
            GameInstance.gameInstance.playerObject.GetComponent<PlayerCharacter>().EnablePlayerController();
            GameInstance.gameInstance.playerObject.GetComponent<PlayerCharacter>().SetMenuState(false);
        }

    }

    public void Pause()
    {
        if(GameInstance.gameInstance.playerObject != null)
        {
            pauseMenuGameObject.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
            SelectButton(0);
        }

    }

    public void Setting()
    {
        Debug.Log("Setting");
    }

    public void LoadTitle()
    {
        Resume();
        GameInstance.gameInstance.playerUI.enabled = false;
        LevelLoader.instance.LoadLevel("TitleScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectButton(int number)
    {

        EventSystem.current.SetSelectedGameObject(null);
        count += number;
        Debug.Log(count);
        audioSource.clip = select;
        audioSource.Play();

        if (count >= buttons.Count)
            count = 0;
        else if (count < 0)
            count = buttons.Count - 1;

        EventSystem.current.SetSelectedGameObject(buttons[count]);
    }

    public void ActivateButton()
    {
        audioSource.clip = activate;
        audioSource.Play();

        switch (count)
        {
            case 0:
                Resume();
                break;
            case 1:
                Setting();
                break;
            case 2:
                LoadTitle();
                break;
            case 3:
                QuitGame(); 
                break;
        }


    }
}
