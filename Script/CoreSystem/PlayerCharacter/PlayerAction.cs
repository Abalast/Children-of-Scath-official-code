using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("CloseCombat")]
    [SerializeField] float weaponDamage = 10f;
    [SerializeField] float attackRange = 0.1f;
    [SerializeField] float weaponTimer = 0.2f;
    [SerializeField] float recoile = 10f;
    [SerializeField] float weaponTimerRemember = 0f;
    [Space]

    [Header("ThrowSpear")]
    [SerializeField] GameObject spearObject;
    [SerializeField] float manaCost = 30f;
    [SerializeField] float spearDamage = 5f;
    [SerializeField] float spearSpeed = 500f;
    [SerializeField] float distanceToReturn = 10f;
    [SerializeField] Vector3 spearSpawnOffset = new Vector3(0f,0.3f,0f);
    [HideInInspector] public GameObject thrownSpear;
    [HideInInspector] public bool possessSpear = true;
    [Space]

    [Header("Spells")]
    [SerializeField] List<PlayerSpells> playerSpells = new List<PlayerSpells>(3);
    [SerializeField] public PlayerSpells selectedSpell;
    int spellCount = 0;

    [Header("CollisionCheck")]
    [SerializeField] public Transform attackField;
    [SerializeField] LayerMask enemyMask;
    [Space]

    [Header("Camera")]
    [SerializeField] public FollowCamera cam;
    [Space]

    [Header("Healing")]
    public bool isHealing;
    [Space]

    PlayerLocomotion playerLoco;
    PlayerAnimation playerAnimation;
    PlayerCharacter playerCharacter;
    PlayerStats playerStatus;
    SoundEffects soundEffects;


    private void Start()
    {
        cam = FindObjectOfType<FollowCamera>();
        playerStatus = GetComponent<PlayerStats>();
        playerLoco = GetComponent<PlayerLocomotion>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        soundEffects = GetComponentInChildren<SoundEffects>();
        playerCharacter = GetComponent<PlayerCharacter>();
        attackField = transform.Find("AttackField");

        if(playerSpells.Count > 0)
            selectedSpell = playerSpells[spellCount];
    }

    private void Update()
    {
        CanAttack();

    }

    private void FixedUpdate()
    {
        if (thrownSpear)
            if (Vector2.Distance(transform.position, thrownSpear.transform.position) > distanceToReturn)
                ThrowSpear();
    }

    public void Attack()
    {
        if (weaponTimerRemember < 0 && possessSpear && !isHealing)
        {
            soundEffects.AudioAttack();
            playerAnimation.AttackAnimation();
            weaponTimerRemember = weaponTimer;
        }

    }

    public void CanAttack()
    {
        weaponTimerRemember -= Time.deltaTime;
    }

    public void ThrowSpear()
    {
        if(possessSpear && !isHealing && playerStatus.GetMana() > manaCost)
        {
            playerStatus.TakeManaDamage(manaCost);
            thrownSpear = Instantiate(spearObject, attackField.position + spearSpawnOffset, attackField.rotation);
            thrownSpear.GetComponent<Spear>().User = gameObject;
            thrownSpear.GetComponent<Spear>().speed = spearSpeed;
            thrownSpear.GetComponent<Spear>().weaponDamage = spearDamage;

            playerLoco.spear = thrownSpear;

            if (playerLoco.facingRight)
                thrownSpear.GetComponent<Spear>().direction = new Vector2(1f,0);
            else
                thrownSpear.GetComponent<Spear>().direction = new Vector2(-1f, 0);

            thrownSpear.name = "Gae Bolg";
            possessSpear = false;
        }
        else if(!playerLoco.isTeleporting && thrownSpear != null )
        {
            thrownSpear.GetComponent<Spear>().ReturnToWielder();
        }
    }

    public void CastSpell()
    {
        if(playerStatus.GetMana() > selectedSpell.requiredMana)
        {
            playerStatus.TakeManaDamage(selectedSpell.requiredMana);
            playerAnimation.Castspell(selectedSpell.triggerAnimationName);
        }
    }

    public void SelectSpell()
    {
        spellCount += 1;

        if (spellCount >= playerSpells.Count)
            spellCount = 0;

        selectedSpell = playerSpells[spellCount];
    }

    public void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackField.position, attackRange, enemyMask);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.transform.position.x - transform.position.x < 0)
            {
                enemy.gameObject.SendMessage("TakeDamage", weaponDamage * -1f);
                playerLoco.rigidBody.velocity = new Vector2(recoile, 0);
            }
            else
            {
                enemy.gameObject.SendMessage("TakeDamage", weaponDamage);
                playerLoco.rigidBody.velocity = new Vector2(recoile * -1, 0);
            }
            cam.GetComponent<FollowCamera>().ShakeCamera();
            StartCoroutine(playerCharacter.Recoile(0.2f));
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackField != null)
            Gizmos.DrawWireSphere(attackField.position, attackRange);
    }
}
