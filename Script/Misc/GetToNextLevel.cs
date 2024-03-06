using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetToNextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelLoader.instance.LoadLevel("DemoLevel");
    }
}
