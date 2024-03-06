using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    public Animator transition;

    public float transitionTime = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        transition = GameObject.Find("Transitor").GetComponent<Animator>();
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadNameLevel(levelName));
    }

    public void RespawnToCheckpoint()
    {
        StartCoroutine(RespawnToLastCheckpoint());
    }

    IEnumerator RespawnToLastCheckpoint()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        GameInstance.gameInstance.playerObject.transform.position = GameInstance.gameInstance.data.lastKnownCheckpoint;

        yield return new WaitForSeconds(transitionTime);

        transition.SetTrigger("End");

        GameInstance.gameInstance.playerObject.GetComponent<PlayerController>().EnableControl();
    }

    public void Respawn()
    {
        StartCoroutine(RespawnLevel());
    }
    /*
    IEnumerator RespawnLevel()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return new WaitForSeconds(transitionTime);

        transition.SetTrigger("End");
    }
    */
    IEnumerator RespawnLevel()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(GameInstance.gameInstance.data.levelIndex);

        SceneManager.sceneLoaded += OnSceneLoad;


    }

    void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartCoroutine(Respawned());
    }

    IEnumerator Respawned()
    {
        yield return new WaitForSeconds(transitionTime);

        GameInstance.gameInstance.FindPlayer();

        if(GameInstance.gameInstance.playerObject)
            GameInstance.gameInstance.playerObject.transform.position = GameInstance.gameInstance.data.lastSavepoint;

        yield return new WaitForSeconds(transitionTime);

        transition.SetTrigger("End");
    }

    IEnumerator LoadNameLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public void StartNewGame()
    {
        StartCoroutine(StartNewGameIEnumerator());
    }

    IEnumerator StartNewGameIEnumerator()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(1);

        SceneManager.sceneLoaded += OnSceneLoad;
    }
}
