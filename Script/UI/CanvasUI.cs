using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUI : MonoBehaviour
{
    public static CanvasUI UI_Instance;
    public PlayerUI playerUI;
    public Animator transiton;
    
    
    private void Awake()
    {
        if (UI_Instance == null)
        {
            UI_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else

        {
            Destroy(gameObject);
        }
    }
    

    private void Start()
    {
        /*
        if (UI_Instance == null)
        {
            UI_Instance = this;
        }
        else
            Destroy(gameObject);
        */

        playerUI = FindObjectOfType<PlayerUI>();
    }
}
