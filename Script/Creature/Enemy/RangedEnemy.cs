using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Range Combat")]
    [SerializeField] GameObject spellBullet;
    [SerializeField] protected Transform rangeField;
    [SerializeField] float range = 5f;

    private void Start()
    {
        fallCheck = transform.Find("IsThereGroundCheck");
        wallCheck = transform.Find("WallCheck");
        groundCheck = transform.Find("IsGroundedCheck");
        rangeField = transform.Find("RangeField");

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        FOV = GetComponent<EnemyFieldOfView>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        health = maxHealth;

        target = FindObjectOfType<PlayerCharacter>().gameObject;

        StartCoroutine(FlipOverTime());
    }

    private void FixedUpdate()
    {
        GroundCheck();
        if (aiIsActive)
            EnemyBehavior();
    }

    public override void EnemyBehavior()
    {
        if (health > 0)
        {
            switch (enemyAggro)
            {
                case EnemyAggro.NoCombat:
                    ;
                    break;
                case EnemyAggro.Searching:
                    MoveTowardTarget();
                    break;
                case EnemyAggro.Combat:
                    RangeAttack();
                    break;
            }
        }
    }

    public virtual void CanRangeAttack()
    {
        if (Vector2.Distance(transform.position, target.transform.position) < range)
            RangeAttack();
    }

    public virtual void RangeAttack()
    {
        animator.SetTrigger("RangeAttack");
    }

    IEnumerator FlipOverTime()
    {
        while(enemyAggro != EnemyAggro.NoCombat) 
        {
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        Flip();
        StartCoroutine(FlipOverTime());
    }

    public virtual void SpawnSpell()
    {

        Debug.Log("Spawn Spell");

        GameObject proj = Instantiate(spellBullet, rangeField.position, rangeField.rotation);
        proj.GetComponent<SpellBullet>().owner = gameObject;

        if (facingRight)
            proj.GetComponent<SpellBullet>().direction = new Vector2(1f, 0);
        else
            proj.GetComponent<SpellBullet>().direction = new Vector2(-1f, 0);
    }
}
