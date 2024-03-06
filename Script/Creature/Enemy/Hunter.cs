using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { MoveTowardEnemy, Dash, RangeAttack, }

public class Hunter : Swordfighter
{
    [Header("Range Combat")]
    [SerializeField] GameObject spellBullet;
    [SerializeField] Transform rangeField;
    [SerializeField] float range = 5f;
    [Space]

    [Header("AdditionalMovement")]
    [SerializeField] float dashForce = 10f;
    [SerializeField] float jumpForce = 4f;

    float randomDecision;
    bool doOnce;
    bool isDashing;

    private void Start()
    {
        fallCheck = transform.Find("IsThereGroundCheck");
        wallCheck = transform.Find("WallCheck");
        groundCheck = transform.Find("IsGroundedCheck");

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        health = maxHealth;

        target = FindObjectOfType<PlayerCharacter>().gameObject;
        attackField = transform.Find("AttackField");
        rangeField = transform.Find("RangeField");
    }

    private void Update()
    {
        weaponTimerRemember -= Time.deltaTime;
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        EnemyBehavior();
        GroundCheck();
    }

    public override void EnemyBehavior()
    {
        if(health > 0)
        {
            switch(enemyAggro)
            {
                case EnemyAggro.Combat:
                    if (isDashing)
                    {
                        if (facingRight)
                            rb.velocity = Vector2.right * dashForce;
                        else
                            rb.velocity = Vector2.left * dashForce;
                    }
                    else if (Vector2.Distance(transform.position, target.transform.position) < attackRange && weaponTimerRemember < 0f && !isHitted)
                    {
                        MeeleAttack();
                    }
                    else if (!isHitted && !playAnimation)
                    {
                        distToPlayer = target.transform.position.x - transform.position.x;
                        distToPlayerY = target.transform.position.y - transform.position.y;

                        if ((distToPlayer > 0 && !facingRight) || (distToPlayer < 0 && facingRight))
                            Flip();

                        if (randomDecision < 0.4f)
                            MoveTowardTarget();
                        else if (randomDecision >= 0.4f && randomDecision < 0.6f)
                            Jump();
                        else if (randomDecision >= 0.6f && randomDecision < 0.8f)
                            CanDash();
                        else if (randomDecision >= 0.8f && randomDecision < 0.95f)
                            CanRangeAttack();
                        else
                            Idle();
                    }
                        break;
                case EnemyAggro.Searching:
                    if (isDashing)
                    {
                        if (facingRight)
                            rb.velocity = Vector2.right * dashForce;
                        else
                            rb.velocity = Vector2.left * dashForce;
                    }
                    else if (!isHitted && !playAnimation)
                    {
                        distToPlayer = target.transform.position.x - transform.position.x;
                        distToPlayerY = target.transform.position.y - transform.position.y;

                        if ((distToPlayer > 0 && !facingRight) || (distToPlayer < 0 && facingRight))
                            Flip();

                        if (randomDecision < 0.4f)
                            MoveTowardTarget();
                        else if (randomDecision >= 0.4f && randomDecision < 0.6f)
                            Jump();
                        else if (randomDecision >= 0.6f && randomDecision < 0.8f)
                            CanDash();
                        else if (randomDecision >= 0.8f && randomDecision < 0.95f)
                            CanRangeAttack();
                        else
                            Idle();
                    }
                    break;
                case EnemyAggro.NoCombat:
                    Move();
                    break;
            }
            /*
            if (aggro)
            {
                if (isDashing)
                {
                    if (facingRight)
                        rb.velocity = Vector2.right * dashForce;
                    else
                        rb.velocity = Vector2.left * dashForce;
                }
                else if (Vector2.Distance(transform.position, target.transform.position) < attackRange && weaponTimerRemember < 0f && !isHitted)
                {
                    MeeleAttack();
                }
                else if (!isHitted && !playAnimation)
                {
                    distToPlayer = target.transform.position.x - transform.position.x;
                    distToPlayerY = target.transform.position.y - transform.position.y;

                    if ((distToPlayer > 0 && !facingRight) || (distToPlayer < 0 && facingRight))
                        Flip();

                    if (randomDecision < 0.4f)
                        MoveTowardTarget();
                    else if (randomDecision >= 0.4f && randomDecision < 0.6f)
                        Jump();
                    else if (randomDecision >= 0.6f && randomDecision < 0.8f)
                        CanDash();
                    else if (randomDecision >= 0.8f && randomDecision < 0.95f)
                        CanRangeAttack();
                    else
                        Idle();

                }

                //if (Vector2.Distance(transform.position, target.transform.position) > attackRange && !isHitted && !playAnimation)
                //    MoveTowardTarget();
            }
            else
            {
                Move();
            }
            */
        }

    }

    public virtual void EndAction()
    {
        randomDecision = Random.Range(0.0f, 1.0f);
    }

    public virtual IEnumerator NextAction(float time)
    {
        doOnce = true;
        yield return new WaitForSeconds(time);
        EndAction();
        doOnce = false;
    }

    public override void MoveTowardTarget()
    {
        bool thereIsGround = Physics2D.OverlapCircle(fallCheck.position, fallCheckRadius, checkCollisionMask);
        bool thereIsWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, checkCollisionMask);

        distToPlayer = target.transform.position.x - transform.position.x;
        distToPlayerY = target.transform.position.y - transform.position.y;

        if ((distToPlayer > 0 && !facingRight) || (distToPlayer < 0 && facingRight))
            Flip();

        if (!thereIsWall && thereIsGround && !isDashing)
        {
            rb.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * aggroMovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if (!isHitted && !isGrounded)
            rb.velocity = new Vector2(0f, rb.velocity.y);
        else if (!isHitted)
            rb.velocity = Vector2.zero;

        if (!doOnce)
            StartCoroutine(NextAction(0.5f));
    }

    public override void Jump()
    {
        Vector2 targetVelocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * aggroMovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
        Vector2 velocity = Vector2.zero;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
        if(!doOnce)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            StartCoroutine(NextAction(1f));
        }
    }

    public virtual void CanDash()
    {
        if (!doOnce)
            StartCoroutine(Dash());
    }

    public virtual IEnumerator Dash()
    {
        doOnce = true;
        isDashing = true;
        animator.SetBool("IsDashing", true);
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        animator.SetBool("IsDashing", false);
        doOnce = false;
        EndAction();
    }

    public virtual void CanRangeAttack()
    {
        if (Vector2.Distance(transform.position, target.transform.position) < range)
            RangeAttack();
        else
            EndAction();
    }

    public virtual void RangeAttack()
    {
        if (!doOnce)
        {
            animator.SetTrigger("RangeAttack");
            StartCoroutine(NextAction(0.5f));
        }
    }

    public virtual void SpawnSpell()
    {

            Debug.Log("Spawn Spell");

            GameObject proj = Instantiate(spellBullet, rangeField.position, attackField.rotation);
            proj.GetComponent<SpellBullet>().owner = gameObject;

            if (facingRight)
                proj.GetComponent<SpellBullet>().direction = new Vector2(1f,0);
            else
                proj.GetComponent<SpellBullet>().direction = new Vector2(-1f, 0);
    }

    public virtual void Idle()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        if(!doOnce)
        {
            StartCoroutine(NextAction(0.2f));
        }
    }

    public override void HandleAnimation()
    {
        animator.SetBool("Grounded", isGrounded);
        base.HandleAnimation();
        animator.SetFloat("MovementY", rb.velocity.y);
    }
}
