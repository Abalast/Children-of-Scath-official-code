using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpPowerUp : PowerUp
{
    private void Start()
    {
        if (GameInstance.gameInstance.data.canWallJump)
            gameObject.SetActive(false);
    }

    public override void ActivateNewPower(GameObject player)
    {
        player.GetComponent<PlayerCharacter>().canWallJump = true;
        GameInstance.gameInstance.data.canWallJump = true;
    }
}
