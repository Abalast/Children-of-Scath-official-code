using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerSpell/ShadowBurst")]
public class ShadowBurst : PlayerSpells
{
    [Header("ShadowBurst")]
    public float attackRange = 1f;
    public float shadowBurstDamage = 20f;
    public LayerMask enemyMask;

    public override void SpellEffect(PlayerAction action)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(action.transform.position, attackRange, enemyMask);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.transform.position.x - action.transform.position.x < 0)
            {
                enemy.gameObject.SendMessage("TakeDamage", shadowBurstDamage * -1f);
            }
            else
            {
                enemy.gameObject.SendMessage("TakeDamage", shadowBurstDamage);
            }
            action.cam.GetComponent<FollowCamera>().ShakeCamera();

        }
    }

    public override void SpellEffect(PlayerCharacter pc)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(pc.transform.position, attackRange, enemyMask);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.transform.position.x - pc.transform.position.x < 0)
            {
                enemy.gameObject.SendMessage("TakeDamage", shadowBurstDamage * -1f);
            }
            else
            {
                enemy.gameObject.SendMessage("TakeDamage", shadowBurstDamage);
            }
            pc.playerAction.cam.GetComponent<FollowCamera>().ShakeCamera();
        }
    }
}
