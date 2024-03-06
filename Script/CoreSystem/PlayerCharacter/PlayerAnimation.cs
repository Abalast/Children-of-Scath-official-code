using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Sprite Colors")]
    public Color nonDashColor;
    public Color dashColor;
    public Color teleportColor;

    public bool isDashing;

    Animator animator;
    SpriteRenderer spriteRenderer;
    PlayerCharacter playerCharacter;
    PlayerLocomotion playerLoco;
    PlayerStats playerHealth;



    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCharacter = GetComponentInParent<PlayerCharacter>();
        playerLoco = GetComponentInParent<PlayerLocomotion>();
        playerHealth = GetComponentInParent<PlayerStats>();
    }

    public void HandleAnimationMovement(float movementX,float movementY)
    {
        animator.SetBool("WallSlide", playerLoco.isWallSliding);
        animator.SetBool("Grounded", playerLoco.isGrounded);
        animator.SetBool("IsDead", playerHealth.isDead);
        animator.SetFloat("MovementX", movementX);
        animator.SetFloat("MovementY", movementY);

        animator.SetBool("IsTeleporting", playerLoco.isTeleporting);

        if (((!playerLoco.isGrounded && isDashing && playerCharacter.canDashMidAir) || playerLoco.isTeleporting))
            spriteRenderer.color = dashColor;
        else
            spriteRenderer.color = nonDashColor;
    }

    public void DashAnimation()
    {
        if(playerLoco.isGrounded || playerCharacter.canDashMidAir)
        {
            isDashing = true;
            animator.SetTrigger("Dash");
        }
    }

    public void HealAnimation(bool healBool)
    {
        animator.SetBool("Healing",healBool);
    }

    public void StopDashing()
    {
        isDashing = false;
        playerHealth.Invincible();
    }

    public void TeleportAnimation()
    {
        animator.SetBool("IsTeleport", true);
        animator.SetTrigger("Teleport");
    }

    public void AttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void Castspell(string castSpell)
    {
        animator.SetTrigger(castSpell);
    }

    public void IsHurt()
    {
        animator.SetTrigger("Hurt");
    }
}
