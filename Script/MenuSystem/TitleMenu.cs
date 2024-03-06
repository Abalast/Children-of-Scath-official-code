using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleMenu : MonoBehaviour
{
    public GameObject titleGameObject;
    public static TitleMenu instance;

    public List<GameObject> buttons;

    public int count;

    [Header("Audio")]
    public AudioClip select;
    public AudioClip activate;

    AudioSource audioSource;

    private void Awake()
    {
        if(!instance)
            instance = this;

        titleGameObject = gameObject;

    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SelectButton(0);
    }

    public void StartGame()
    {
        GameInstance.gameInstance.CreateNewSlate();
        LevelLoader.instance.StartNewGame();
    }
    
    public void Continue()
    {

    }

    public void Setting()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectButton(int number)
    {
        EventSystem.current.SetSelectedGameObject(null);
        count += number;
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
                StartGame();
                break;
            case 1:
                Continue();
                break;
            case 2:
                Setting();
                break;
            case 3:
                QuitGame();
                break;
        }
    }
}
