using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public LayerMask whoCanGetPower;
    public virtual void ActivateNewPower(GameObject player)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.IsInLayerMasks(whoCanGetPower))
        {
            ActivateNewPower(collision.gameObject);
            gameObject.SetActive(false);
        }
    }
}
