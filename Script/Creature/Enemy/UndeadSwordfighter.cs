using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadSwordfighter : Swordfighter
{
    public override void EnemyBehavior()
    {
        if (health > 0)
        {
            switch (enemyAggro)
            {
                case EnemyAggro.Combat:
                    if (Vector2.Distance(transform.position, target.transform.position) < attackRange && weaponTimerRemember < 0f && !isHitted)
                        MeeleAttack();
                    else if (!isHitted && playAnimation)
                        rb.velocity = Vector2.zero;
                    else if (Vector2.Distance(transform.position, target.transform.position) < attackRange && weaponTimerRemember > 0f && !isHitted)
                        MoveAwayFromTarget();
                    else if (!isHitted && !playAnimation)
                        MoveTowardTarget();
                    break;
                case EnemyAggro.Searching:
                    SearchTarget();
                    break;
                case EnemyAggro.NoCombat:
                    Move();
                    break;
            }
        }
        else
            rb.velocity = Vector2.zero;
    }

    public override void TakeDamage(float damage)
    {
        if (!isInvincible && health > 0)
        {
            animator.SetTrigger("Hurt");
            audioSource.clip = hit;
            audioSource.Play();

            float direction = damage / Mathf.Abs(damage);

            Debug.Log(direction);

            damage = Mathf.Abs(damage);

            health -= damage;

            if (health <= 0)
                StartCoroutine(RiseAgain());
            else
                StartCoroutine(Invincible());

            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direction * recoilx, recoily));

            if (facingRight && direction == 1)
                Flip();
            else if (!facingRight && direction == -1)
                Flip();

        }
    }

    protected virtual IEnumerator RiseAgain()
    {
        isInvincible = true;
        gameObject.layer = LayerMask.NameToLayer("Dash");
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(3f);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        animator.SetTrigger("Recover");

    }

    public virtual void ReadyToFight()
    {
        isInvincible = false;
        health = maxHealth;
        enemyAggro = EnemyAggro.Combat;
    }
}
