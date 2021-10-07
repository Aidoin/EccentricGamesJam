using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    public static List<EnemyController> ActiveEnemy { get; private set; } = new List<EnemyController>();

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject predspawnPrefab;

    private EnemyController enemyController;

    [SerializeField] private float spawnInterval = 10;
    private float timer = 999;
    private float timer2 = 999;
    private bool needToCreate = true;


    private void FixedUpdate() {
        if (needToCreate) {
            //if (GameManager.Instance.EnemyPause) {
            //    return;
            //} else {
            timer += Time.fixedDeltaTime;
            timer2 += Time.fixedDeltaTime;

            if (timer > spawnInterval-2) {
                timer2 = 0;
                Instantiate(predspawnPrefab, transform.position, transform.rotation, transform.parent);
            }
            if (timer > spawnInterval) {
                timer = 0;
                timer2 = 0;
                needToCreate = false;
                Spawn();
            }
            //}
        }
    }

    public void EnemyDeath(bool isBonus) {
        ActiveEnemy.Remove(enemyController);
        needToCreate = true;
        if (isBonus) {
            Bonuses.Instance.CreateBonus();
        }
    }

    private void Spawn() {
        Random.Range(1, 5);

        enemyController = Instantiate(enemyPrefab, transform.position, transform.rotation, transform.parent).GetComponent<EnemyController>();
        enemyController.spawnerEnemy = this;
        enemyController.SetTamkLevel();
        enemyController.SetTamkLevel(Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 100f) > 80);
        enemyController.Tank.Sethealth();
        ActiveEnemy.Add(enemyController);
        if (GameManager.Instance.EnemyPause) {
            enemyController.Disable();
        }
    }
    private void OnDestroy() {
        ActiveEnemy.Clear();
    }
}
