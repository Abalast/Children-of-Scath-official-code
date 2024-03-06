using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMidAirJumpUp : PowerUp
{
    private void Start()
    {
        if (GameInstance.gameInstance.data.canDashInAir)
            gameObject.SetActive(false);
    }

    public override void ActivateNewPower(GameObject player)
    {
        player.GetComponent<PlayerCharacter>().canDashMidAir = true;
        GameInstance.gameInstance.data.canDashInAir = true;
    }
}
