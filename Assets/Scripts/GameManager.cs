using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct ComponentPaused
{
    public MonoBehaviour Component;
    public bool StartEnabled;

    public ComponentPaused(MonoBehaviour component, bool startEnabled) {
        Component = component;
        StartEnabled = startEnabled;
    }
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Text textNoLevel;
    [SerializeField] private GameObject panelinterface;
    [Space(10)]

    [SerializeField] private Transform uiEnemyconteiner;
    [SerializeField] private GameObject uiEnemyPrefab;
    private List<GameObject> uiEnemyList = new List<GameObject>();
    [Space(5)]

    [SerializeField] private Transform uiHealthconteiner;
    [SerializeField] private GameObject uiHealthPrefab;
    private List<GameObject> uiHealthList = new List<GameObject>();
    [Space(10)]

    [SerializeField] private Image healthBar;
    [Space(10)]

    [SerializeField] private AudioSource music;
    [SerializeField] private AudioClip nextMusic;

    [SerializeField] private PauseMenu pauseMenu;

    public List<Qwest> Levels = new List<Qwest>();
    private int currentLevelId;
    private GameObject currentLevel; public GameObject CurrentLevel => currentLevel;

    public List<MonoBehaviour> DisadisableWhenPaused = new List<MonoBehaviour>();
    private List<ComponentPaused> pausedComponents = new List<ComponentPaused>();

    [SerializeField] private GameObject winSreen;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private GameObject endScreen;

    [SerializeField] private int numberOfEnemiesKilledToWin = 10;
    private int scoreOfEnemiesKilled;
    [SerializeField] private int numberOfLives = 3;
    private int remainingNumberOfLives;

    public bool EnemyPause = false;

    public PlayerController player;


    private void Awake() {
        if (Instance) {
            Destroy(this);
            Debug.LogError("ѕопытка повторной инициализации GameManager");
        } else {
            Instance = this;
        }
    }

    private void Start() {
        LoadLevel(0);
        StartCoroutine(ChangeMusic());
        Cursor.visible = false;
    }

    public void AddSlainEnemy() {
        scoreOfEnemiesKilled += 1;
        Destroy(uiEnemyList[0]);
        uiEnemyList.RemoveAt(0);

        if (scoreOfEnemiesKilled > numberOfEnemiesKilledToWin - 1) {
            if (currentLevelId + 1 == Levels.Count) {
                End();
            } else {
                Win();
            }
        }
    }

    public bool PlayerDeath() {
        remainingNumberOfLives -= 1;
        Destroy(uiHealthList[0]);
        uiHealthList.RemoveAt(0);

        if (remainingNumberOfLives > 0) {
            return true;
        } else {
            Defeat();
            return false;
        }
    }

    public void Win() {
        Pause();
        winSreen.SetActive(true);
    }

    public void Defeat() {
        Pause();
        defeatScreen.SetActive(true);
        pauseMenu.isEnd = true;
        Cursor.visible = true;
    }

    public void End() {
        Pause();
        endScreen.SetActive(true);
        pauseMenu.isEnd = true;
    }

    public void SetHealth(float i) {
        healthBar.fillAmount = i;
    }

    public void RestartLevel() {
        LoadLevel(currentLevelId);
    }
    public void LoadNextLevel() {
        LoadLevel(currentLevelId + 1);
    }
    public void LoadLevel(int levelId) {
        Pause();
        
        panelinterface.SetActive(true);

        if (Levels.Count - 1 < levelId) {
            Debug.LogError("ѕопытка загрузить несуществующий уровень: " + levelId);
            return;
        }
        DestroyCurrentLevel();
        Cursor.visible = false;

        textNoLevel.text = (levelId + 1).ToString();
        pauseMenu.isEnd = false;

        remainingNumberOfLives = numberOfLives;
        for (int i = 0; i < remainingNumberOfLives; i++) {
            uiHealthList.Add(Instantiate(uiHealthPrefab, uiHealthconteiner));
        }

        numberOfEnemiesKilledToWin = Levels[levelId].EnemyCount;
        for (int i = 0; i < numberOfEnemiesKilledToWin; i++) {
            uiEnemyList.Add(Instantiate(uiEnemyPrefab, uiEnemyconteiner));
        }

        currentLevelId = levelId;

        currentLevel = Instantiate(Levels[levelId].Level, Vector3.zero, Quaternion.identity);
        UnPause();
    }


    public void AddHP(int count) {
        remainingNumberOfLives += 1;
        uiHealthList.Add(Instantiate(uiHealthPrefab, uiHealthconteiner));
    }


    public void Pause() {
        for (int i = 0; i < DisadisableWhenPaused.Count; i++) {
            pausedComponents.Add(new ComponentPaused(DisadisableWhenPaused[i], DisadisableWhenPaused[i].enabled));
            DisadisableWhenPaused[i].enabled = false;
        }
        //DisadisableWhenPaused.Clear();
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    public void UnPause() {
        for (int i = 0; i < pausedComponents.Count; i++) {
            pausedComponents[i].Component.enabled = pausedComponents[i].StartEnabled;
        }
        pausedComponents.Clear();
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    private void DestroyCurrentLevel() {
        winSreen.SetActive(false);
        defeatScreen.SetActive(false);
        endScreen.SetActive(false);
        DisadisableWhenPaused.Clear();
        pausedComponents.Clear();
        Destroy(currentLevel);
        currentLevel = null;
        remainingNumberOfLives = 0;
        scoreOfEnemiesKilled = 0;
        Bonuses.Instance.ClearCoins();


        for (int i = 0; i < uiHealthList.Count; i++) {
            Destroy(uiHealthList[i]);
        }
        uiHealthList.Clear();

        for (int i = 0; i < uiEnemyList.Count; i++) {
            Destroy(uiEnemyList[i]);
        }
        uiEnemyList.Clear();
        healthBar.fillAmount = 1;

        textNoLevel.text = "0";
    }


    private IEnumerator ChangeMusic() {
        yield return new WaitForSecondsRealtime(music.clip.length);
        music.clip = nextMusic;
        music.Play();
    }
}
