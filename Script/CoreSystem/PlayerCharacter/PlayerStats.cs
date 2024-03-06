using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float health;
    [SerializeField] ParticleSystem healParticle;
    [SerializeField] ParticleSystem damageParticle;
    [Space]

    [Header("Mana")]
    [SerializeField] float maxMana = 100f;
    [SerializeField] float regainHealthSpeed = 20f;
    [SerializeField] float regainManaSpeed = 10f;
    [SerializeField] float mana;
    [SerializeField] float manaCooldownTime;
    float manaCooldown;
    [HideInInspector] public bool shouldRegainMana = true;
    [Space]

    [Header("Stamina -Maybe I will use it-")]
    [SerializeField] float maxStamina = 100f;
    float stamina;

    PlayerAnimation playerAnimation;
    PlayerCharacter playerCharacter;
    Rigidbody2D rb;
    public PlayerUI playerUI;

    bool isInvincible = false;
    bool isHealing = false;
    [HideInInspector] public bool isDead;

    private void Start()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerCharacter = GetComponent<PlayerCharacter>();
        rb = GetComponent<Rigidbody2D>();

        playerUI = FindObjectOfType<PlayerUI>();

        health = maxHealth;
        mana = maxMana;

        stamina = maxStamina;

        shouldRegainMana = true;

        playerUI.SetMaxHealth(maxHealth, health);
        playerUI.SetMaxMana(maxMana, mana);
        playerUI.UpdateMoney();
    }

    private void Update()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        RegainHealth();
        RegainMana();
        playerUI.SetHealth(health);
        playerUI.SetMana(mana);
    }

    #region Health

    public void RegainHealth()
    {
        if (isHealing && mana > 0 && health < maxHealth)
        {
            health += regainHealthSpeed * Time.deltaTime;
            mana -= regainHealthSpeed * Time.deltaTime;


            if (health > maxHealth)
                health = maxHealth;
        }
    }

    public void StartHealing()
    {
        isHealing = true;
        healParticle.Play();
    }

    public void StopHealing()
    {
        isHealing = false;
        healParticle.Stop();
        manaCooldown = manaCooldownTime;
    }
    #endregion

    #region Mana

    public void RegainMana()
    {
        manaCooldown -= Time.deltaTime;

        if (manaCooldown < 0f && !isHealing)
        {
            mana += regainManaSpeed * Time.deltaTime;
            
            if (mana > maxMana)
                mana = maxMana;

        }
    }

    public float GetMana()
    {
        return mana;
    }
    #endregion

    #region Damage Functions
    public void TakeDamage(float damage)
    {
        if (isInvincible == false && !isDead)
        {
            if (damageParticle)
                damageParticle.Play();

            float direction = damage / Mathf.Abs(damage);
            damage = Mathf.Abs(damage);

            playerAnimation.IsHurt();
            health -= damage;

            //playerUI.SetHealth(health);

            if (health <= 0)
                Death();
            else
                Invincible();

            //rb.velocity = Vector2.zero;
            //rb.AddForce(new Vector2(direction * 200f, 100f));

            rb.velocity = new Vector2(10f * direction, rb.velocity.y);

            // In case
            StartCoroutine(playerCharacter.Recoile(0.1f));
        }
    }

    public void TakeDamage(float damage, bool forcedRespawn)
    {
        if(isInvincible == false && !isDead)
        {
            float direction = damage / Mathf.Abs(damage);
            damage = Mathf.Abs(damage);

            playerAnimation.IsHurt();
            health -= damage;

            if (health <= 0)
                Death();
            else if (forcedRespawn)
                ForceRespawn();
            else
                Invincible();

            rb.velocity = new Vector2(10f * direction, rb.velocity.y);

            // In case
            StartCoroutine(playerCharacter.Recoile(0.1f));
        }
    }

    public void TakeManaDamage(float damage)
    {
        mana -= damage;
        manaCooldown = manaCooldownTime;
    }

    public void TakeStaminaDamage(float damage)
    {
        stamina -= damage;
    }
    #endregion

    #region Death and Invincible
    public void Death()
    {
        StartCoroutine(DeathTime());
    }

    IEnumerator DeathTime()
    {
        isDead = true;
        playerCharacter.DisablePlayerController();
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        yield return new WaitForSeconds(0.5f);
        GameInstance.gameInstance.data.money /= 2;
        GameInstance.gameInstance.data.money = Mathf.Round(GameInstance.gameInstance.data.money);
        LevelLoader.instance.Respawn();
    }

    public void Invincible()
    {
        StartCoroutine(InvincibleTime());
    }

    IEnumerator InvincibleTime()
    {
        isInvincible = true;
        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
    }

    #endregion

    #region Respawn Event
    public void Respawned()
    {
        health = maxHealth;
        isDead = false;

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        playerCharacter.EnablePlayerController();
    }

    public void ForceRespawn()
    {
        playerCharacter.DisablePlayerController();
        LevelLoader.instance.RespawnToCheckpoint();
    }
    #endregion

    #region SetandGets
    
    public void SetPlayerUI()
    {
        playerUI = FindObjectOfType<PlayerUI>();
    }

    public void SetPlayerUI(PlayerUI UI)
    {
        playerUI = UI;

        playerUI.SetMaxHealth(maxHealth, health);
        playerUI.SetMaxMana(maxMana, mana);
    }

    #endregion


}
