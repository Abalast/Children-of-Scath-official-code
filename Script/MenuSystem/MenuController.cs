using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    public InputMaster menuControl;

    public static MenuController instance;

    private void Awake()
    {
        if (instance)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        EnableControl();

        //playerCharacter = FindObjectOfType<PlayerCharacter>();

        menuControl.MenuController.MenuControls.performed += menuC => SelectNewButton(menuC.ReadValue<float>());
        menuControl.MenuController.Submit.performed += sub => EnterButton();

    }

    public void EnableControl()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        if (menuControl == null)
        {
            menuControl = new InputMaster();
        }
        menuControl.Enable();
    }

    public void DisableControl()
    {
        OnDisable();
    }

    private void OnDisable()
    {
        menuControl.Disable();
    }

    public void SelectNewButton(float newValue)
    {
        int newNumber = (int)newValue;
        TitleMenu.instance.SelectButton(newNumber);
    }

    public void EnterButton()
    {
        TitleMenu.instance.ActivateButton();
    }
}
