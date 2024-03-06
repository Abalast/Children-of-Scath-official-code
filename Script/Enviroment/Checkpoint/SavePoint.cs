using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class SavePoint : MonoBehaviour
{
    public Light2DBase lightSource;
    private void Start()
    {
        lightSource = GetComponent<Light2DBase>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameInstance.gameInstance.data.lastSavepoint = transform.position;
            GameInstance.gameInstance.data.levelIndex = SceneManager.GetActiveScene().buildIndex;
            lightSource.enabled = true;
        }
    }
}
