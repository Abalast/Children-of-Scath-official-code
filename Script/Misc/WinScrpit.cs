using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScrpit : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(BackToTitle());
    }

    IEnumerator BackToTitle()
    {
        Debug.Log("Wait");
        yield return new WaitForSeconds(5f);
        LevelLoader.instance.LoadLevel("TitleScreen");
    }
}
