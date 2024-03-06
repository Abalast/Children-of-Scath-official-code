using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyingTerrain : Trap
{
    public override void ActivateTrap()
    {
        gameObject.SetActive(false);
    }
}
