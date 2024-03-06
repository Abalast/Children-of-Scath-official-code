using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObjects : MonoBehaviour
{
    [Header("Health")]
    public float health;
    public ParticleSystem destroyEffect;
    // Start is called before the first frame update

    public virtual void TakeDamage(float damage)
    {

            float direction = damage / Mathf.Abs(damage);

            damage = Mathf.Abs(damage);
            health -= damage;

        destroyEffect.Play();

        if (health <= 0)
            gameObject.SetActive(false);



    }
}
