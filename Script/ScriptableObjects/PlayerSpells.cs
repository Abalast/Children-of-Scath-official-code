using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerSpell/Test")]
public class PlayerSpells : ScriptableObject
{
    public string spellName;
    public float requiredMana;
    public string triggerAnimationName;

    public virtual void SpellEffect()
    {
        Debug.Log("Default");
    }

    public virtual void SpellEffect(PlayerAction action)
    {
        Debug.Log("Default");
    }

    public virtual void SpellEffect(PlayerCharacter pc)
    {
        Debug.Log("Default");
    }

    public virtual void SpellEffect(PlayerLocomotion loco)
    {
        loco.rigidBody.velocity = new Vector2(loco.rigidBody.velocity.x, 6f);
    }

    public virtual void SpellEffect(PlayerAction action, PlayerLocomotion loco)
    {
        loco.rigidBody.velocity = new Vector2(loco.rigidBody.velocity.x, 6f);
    }
}
