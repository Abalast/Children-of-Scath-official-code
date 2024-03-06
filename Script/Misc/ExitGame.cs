using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player") || col.gameObject.layer == LayerMask.NameToLayer("Dash"))
        {
            GameInstance.gameInstance.playerUI.enabled = false;
            LevelLoader.instance.LoadLevel("WinScreen");
        }

    }
}
