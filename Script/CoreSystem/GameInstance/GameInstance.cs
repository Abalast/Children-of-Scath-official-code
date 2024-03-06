using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public static GameInstance gameInstance;
    public GameInstanceData data; 

    public GameObject playerObject;
    public PlayerCharacter player;
    public PlayerUI playerUI;
    public GameObject UIGameObject;

    private void Awake()
    {
        if(gameInstance == null)
        {
            gameInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (data == null)
            data = new GameInstanceData();

    }

    private void Start()
    {
        if(FindObjectOfType<PlayerCharacter>())
        {
            player = FindObjectOfType<PlayerCharacter>();
            playerObject = player.gameObject;
            data.lastKnownCheckpoint = playerObject.transform.position;
        }

        playerUI = FindObjectOfType<PlayerUI>();
        UIGameObject = playerUI.gameObject;

        if (!playerObject)
        {
            UIGameObject.SetActive(false);
        }
    }

    public Vector2 RespawnPlayer()
    {
        return data.lastKnownCheckpoint;
    }

    public void FindPlayer()
    {
        if(FindObjectOfType<PlayerCharacter>())
        {

            UIGameObject.SetActive(true);

            player = FindObjectOfType<PlayerCharacter>();
            playerObject = player.gameObject;
            playerUI = FindObjectOfType<PlayerUI>();

            player.playerStats.SetPlayerUI(playerUI);
            playerUI.UpdateMoney();
        }
        else
        {
            UIGameObject.SetActive(false);
        }
    }

    public void EnablePlayerController()
    {
        if(player)
            player.EnablePlayerController();
    }

    public void CreateNewSlate()
    {
        data = ScriptableObject.CreateInstance<GameInstanceData>();
        data.lastKnownCheckpoint = new Vector2(-29.659f,9.98f);
        data.lastSavepoint = new Vector2(-34.629f, 9.773f);
    }

}
