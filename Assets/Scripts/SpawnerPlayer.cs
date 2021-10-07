using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlayer : MonoBehaviour
{
    public static SpawnerPlayer Instance { get; private set; }

    [SerializeField] private GameObject playerPrefab;

    private int level;

     public PlayerController playerController { get; private set; }

    private void Start() {
        Instance = this;
        Spawn();
    }

    public void PlayerDeath() {
        if (GameManager.Instance.PlayerDeath()) {
            level = playerController.level;
            Spawn();
        }
    }

    private void Spawn() {
        playerController = Instantiate(playerPrefab, transform.position, transform.rotation, transform.parent).GetComponent<PlayerController>();
        CameraFollowTarget.Instance.Target = playerController.CameraTarget;
        playerController.spawnerPlayer = this;

    }
}
