using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Player Component that are required")]
    public PlayerStats playerStats;
    public PlayerAction playerAction;
    public PlayerLocomotion playerLoco;
    public PlayerAnimation playerAnimation;
    public PlayerController playerController;
    public SoundEffects soundEffects;
    [Space]

    [Header("PlayerCanDoSkills")]
    public bool canMove = true;
    public bool canWallJump = false;
    public bool canDashMidAir = false;
    public bool canTeleport = false;

    FollowCamera cam;

    bool menuIsOpen;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerAction = GetComponent<PlayerAction>();
        playerLoco = GetComponent<PlayerLocomotion>();
        playerController = GetComponent<PlayerController>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        soundEffects = GetComponentInChildren<SoundEffects>();

        cam = FindObjectOfType<FollowCamera>();

        canMove = true;
        canWallJump = GameInstance.gameInstance.data.canWallJump;
        canDashMidAir = GameInstance.gameInstance.data.canDashInAir;
    }

    private void Update()
    {
        playerAnimation.HandleAnimationMovement(playerLoco.rigidBody.velocity.x, playerLoco.rigidBody.velocity.y);

        SetPlayerLayer();
    }

    private void FixedUpdate()
    {
        if (!playerStats.isDead)
        {
            if (canMove)
            {
                playerLoco.Move(playerController.GetMovementX() * Time.fixedDeltaTime, playerController.GetMovementY() * Time.fixedDeltaTime);
            }
            else
            {
                playerLoco.Move(0f,0f) ;
            }

            if (playerController.GetMovementX() == 0f && playerLoco.isGrounded)
            {
                cam.AdjustYCamera(playerController.GetMovementY());
            }
            else
                cam.AdjustYCamera(0f);

            playerLoco.GroundAndWallCheck();
        }

    }

    #region Player Controllers
    public void EnablePlayerController()
    {
        if(playerController)
            playerController.EnableControl();
    }

    public void DisablePlayerController()
    {
        playerController.DisableControl();
    }
    #endregion

    #region Player Movement

    public void PlayerDash()
    {
        if (playerLoco.CanDash())
        {
            playerAnimation.DashAnimation();
            playerLoco.Dash();
        }
    }

    public void SetPlayerLayer()
    {
        if (playerAnimation.isDashing)
        {
            gameObject.layer = LayerMask.NameToLayer("Dash");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    public void PlayerTeleport()
    {
        if (!playerAction.possessSpear && playerLoco.isTeleporting == false && canTeleport)
        {
            playerLoco.isTeleporting = true;
            playerAnimation.TeleportAnimation();
            playerLoco.rigidBody.gravityScale = 0f;
        }
        else if (!playerAction.possessSpear && playerLoco.isTeleporting == true)
        {
            playerLoco.isTeleporting = false;
            playerLoco.rigidBody.gravityScale = 1f;
        }
    }

    public void PlayerJump()
    {
        if (!menuIsOpen)
            playerLoco.Jump();
    }

    public void PlayerStopJump()
    {
        playerLoco.CancelJump();
    }

    public IEnumerator Recoile(float recoilTime)
    {
        canMove = false;
        yield return new WaitForSeconds(recoilTime);
        canMove = true;
    }
    #endregion

    #region Player Actions
    public void PlayerAttack()
    {
        if(!menuIsOpen) 
            playerAction.Attack();
    }

    public void PlayerThrow()
    {
        if (!menuIsOpen)
            playerAction.ThrowSpear();
    }

    public void PlayerCastSpell()
    {
        if (!menuIsOpen)
            playerAction.CastSpell();
    }

    public void SelectSpell()
    {
        if (!menuIsOpen)
            playerAction.SelectSpell();
    }

    public void PlayerHeal()
    {
        if (!menuIsOpen)
        {
            playerAnimation.HealAnimation(true); ;

            playerStats.StartHealing();
            playerAction.isHealing = true;
            canMove = false;
        }

    }

    public void PlayerStopHeal()
    {
        playerAnimation.HealAnimation(false); ;

        playerStats.StopHealing();
        playerAction.isHealing = false;
        canMove = true;
    }
    #endregion

    #region Menu System
    public void PauseMenuManager()
    {
        if (!PauseMenu.GameIsPaused)
        {
            menuIsOpen = true;
            PauseMenu.instance.Pause(); 
        }
        else
        {
            menuIsOpen = false;
            PauseMenu.instance.Resume();
        }
    }
    
    public void SelectNewButton(float newValue)
    {
        if(PauseMenu.GameIsPaused)
        {
            int newNumber = (int)newValue;
            Debug.Log(newNumber);
            PauseMenu.instance.SelectButton(newNumber);
        }
    }

    public void EnterButton()
    {
        if(PauseMenu.GameIsPaused)
        {
            PauseMenu.instance.ActivateButton();
        }
    }
    #endregion

    public bool GetMenuState()
    {
        return menuIsOpen;
    }
    
    public void SetMenuState(bool newMenuState)
    {
        menuIsOpen = newMenuState;
    }
}
