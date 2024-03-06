using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public PlayerAction playerAction;

    private void Start()
    {
        playerAction = GetComponentInParent<PlayerAction>();
    }

    //This Function is called through Animations
    void DealDamage()
    {
        playerAction.DealDamage();
    }

    void CastSpell()
    {
        playerAction.selectedSpell.SpellEffect(playerAction);
    }
}
