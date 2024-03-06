using UnityEngine;

[CreateAssetMenu(menuName = "GameInstanceData")]
public class GameInstanceData : ScriptableObject
{
    [Header("SpawnPoints")]
    public int levelIndex;
    public Vector2 lastSavepoint;
    public Vector2 lastKnownCheckpoint;
    [Space]

    [Header("HealthData")]
    public float maxHealth;
    public float currentHealth;
    [Space]

    [Header("ManaData")]
    public float maxMana;
    public float currentMana;
    [Space]

    [Header("Money")]
    public float money;
    [Space]

    [Header("Player Skills")]
    public bool canWallJump;
    public bool canDashInAir;
    public bool canTeleport;
    [Space]

    [Header("Level_01")]
    public bool placeholder;
}
