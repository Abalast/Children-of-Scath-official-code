using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : DestroyableObjects
{
    [Header("Money Prefab")]
    public GameObject money;
    public int howMany;
    List<GameObject> moneyList; 
    
    private void Start()
    {
        moneyList = new List<GameObject>();

        for(int i = 0; i <howMany; i++)
        {
            moneyList.Add(Instantiate(money,transform.position,transform.rotation));
        }

        /*
        foreach(Transform child in transform)
        {
            if(child.gameObject.layer == LayerMask.NameToLayer("Money"))
            {
                moneyList.Add(child.gameObject);
            }
        }
        */
    }

    public override void TakeDamage(float damage)
    {

        float direction = damage / Mathf.Abs(damage);

        damage = Mathf.Abs(damage);
        health -= damage;

        Instantiate(destroyEffect, transform.position, Quaternion.identity);

        if (health <= 0)
        {
            foreach(GameObject child in moneyList)
            {
                child.transform.parent = null;
                child.SetActive(true);
            }

            gameObject.SetActive(false);
        }

    }
}
