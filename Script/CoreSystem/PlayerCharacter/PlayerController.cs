using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public InputMaster controls;

    PlayerCharacter playerCharacter;

    float movementX;
    float movementY;

    private void Start()
    {
        OnEnable();

        playerCharacter = GetComponent<PlayerCharacter>();

        controls.Player.Movement.performed += mov => SetLeftRight(mov.ReadValue<Vector2>());
        controls.Player.LookDirection.performed += look => SetUpDown(look.ReadValue<Vector2>());
        controls.Player.LookDirection.canceled += look => SetUpDown(look.ReadValue<Vector2>());
        controls.Player.Jump.performed += jump => playerCharacter.PlayerJump();
        controls.Player.Jump.canceled += jump => playerCharacter.PlayerStopJump();
        controls.Player.SideStep.performed += dash => playerCharacter.PlayerDash();
        controls.Player.Attack.performed += atk => playerCharacter.PlayerAttack();
        controls.Player.Throw.performed += spr => playerCharacter.PlayerThrow();
        controls.Player.CastSpell.started += spell => playerCharacter.PlayerCastSpell();
        controls.Player.SelectSpell.performed += select => playerCharacter.SelectSpell();
        controls.Player.ShadowTeleport.performed += tele => playerCharacter.PlayerTeleport();
        controls.Player.Heal.performed += heal => playerCharacter.PlayerHeal();
        controls.Player.Heal.canceled += cheal => playerCharacter.PlayerStopHeal();
        controls.Player.Menu.performed += menu => playerCharacter.PauseMenuManager();

        controls.MenuController.MenuControls.performed += menuC => playerCharacter.SelectNewButton(menuC.ReadValue<float>());
        controls.MenuController.Submit.performed += sub => playerCharacter.EnterButton();

        //Debug Commands. Needs to be deleted if not needed anymore
        //controls.Player.Debug.started += ui => FindObjectOfType<CanvasUI>().gameObject.SetActive(false);

    }

    void SetLeftRight(Vector2 direction)
    {
        movementX = direction.x;
    }

    void SetUpDown(Vector2 direction)
    {
        movementY = direction.y;
    }

    public float GetMovementX()
    {
        return movementX;
    }

    public float GetMovementY()
    {
        return movementY;
    }

    public void EnableControl()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new InputMaster();
        }
        controls.Enable();
    }

    public void DisableControl()
    {
        OnDisable();
        movementX = 0f;
    }

    private void OnDisable()
    {
        controls.Disable();
    }


    private void DebugUI()
    {
        if (CanvasUI.UI_Instance.playerUI.gameObject.activeSelf)
            CanvasUI.UI_Instance.playerUI.gameObject.SetActive(false);
        else
            CanvasUI.UI_Instance.playerUI.gameObject.SetActive(true);
    }
}
