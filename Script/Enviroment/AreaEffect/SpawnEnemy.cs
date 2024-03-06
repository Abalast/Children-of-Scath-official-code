using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : Trap
{
    public GameObject EnemySpawn;
    public bool flipEnemy;

    BasicEnemy enemy;

    public void Start()
    {
        enemy = EnemySpawn.GetComponent<BasicEnemy>();
        
    }

    public override void ActivateTrap()
    {
        //Instantiate(EnemySpawn, transform.position,transform.rotation);
        if (flipEnemy)
            enemy.Flip();
        EnemySpawn.SetActive(true);
    }
}
