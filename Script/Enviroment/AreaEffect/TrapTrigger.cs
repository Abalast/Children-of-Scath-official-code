using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public Trap[] traps;

    bool isActivated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Dash")) && traps != null && !isActivated)
        {
            foreach (Trap t in traps)
            {
                t.ActivateTrap();
            }

            isActivated = true;
            //Destroy(gameObject); 
        }

    }
}
