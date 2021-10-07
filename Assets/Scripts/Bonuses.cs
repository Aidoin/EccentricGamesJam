using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonuses : MonoBehaviour
{
    public static Bonuses Instance { get; private set; }

    public List<GameObject> BonusPrefabs = new List<GameObject>();

    public float PauseTime = 10;
    private float PauseTimer;
    public int AllCoins = 0;
    private int localCoins = 0;

    [SerializeField] private Text coinCounter;


    private void Awake() {
        Instance = this;
    }


    private void FixedUpdate() {
        if (GameManager.Instance.EnemyPause) {
            PauseTimer += Time.deltaTime;
            if(PauseTimer> PauseTime) {
                PauseTimer = 0;
                EnemyUnPause();
            }
        }
    }

    public void EnemyPause() {

        for (int i = 0; i < SpawnerEnemy.ActiveEnemy.Count; i++) {
            SpawnerEnemy.ActiveEnemy[i].Disable();
            GameManager.Instance.EnemyPause = true;
        }
    }

    public void EnemyUnPause() {
        for (int i = 0; i < SpawnerEnemy.ActiveEnemy.Count; i++) {
            SpawnerEnemy.ActiveEnemy[i].Enable();
            GameManager.Instance.EnemyPause = false;
        }
    }

    public void AddCoins(int coins) {
        AllCoins += coins;
        coinCounter.text = AllCoins.ToString();
        localCoins += coins;
        if(localCoins> 999) {
            localCoins -= 1000;
            LevelUp();
        }
    }



    public void ClearCoins() {
        AllCoins = 0;
        localCoins = 0;
        coinCounter.text = "0";
    }

    private void LevelUp() {
        if (SpawnerPlayer.Instance.playerController.level < 3) {
            SpawnerPlayer.Instance.playerController.level = SpawnerPlayer.Instance.playerController.level + 1;
        }
    }

    public void CreateBonus() {
        int intin = Random.Range(0, BonusPrefabs.Count - 1);

        Instantiate(BonusPrefabs[intin], new Vector3(Random.Range(-11.5f, 11.5f), 50, Random.Range(-11.5f, 11.5f)), Quaternion.identity, GameManager.Instance.CurrentLevel.transform);
    }
}
