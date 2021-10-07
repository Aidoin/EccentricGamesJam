using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct TankModelsOfLevel
{
    public List<GameObject> models1;
    public List<GameObject> models2;
    public List<GameObject> models3;
    public List<GameObject> models4;

    public void ShowLevel(int level) {
        if (level < 0 || level > models1.Count-1) {
            Debug.LogError("Устанавливаемый левел выходит за диапазон существующих: "+ level);
            return;
        }
        for (int i = 0; i < models1.Count; i++) {
            models1[i].SetActive(false);
            models2[i].SetActive(false);
            models3[i].SetActive(false);
            models4[i].SetActive(false);
        }

        if (level == 0) {
            for (int i = 0; i < models1.Count; i++) {
                models1[i].SetActive(true);
            }
        } else
        if (level == 1) {
            for (int i = 0; i < models2.Count; i++) {
                models2[i].SetActive(true);
            }
        } else
        if (level == 2) {
            for (int i = 0; i < models3.Count; i++) {
                models3[i].SetActive(true);
            }
        } else
        if (level == 3) {
            for (int i = 0; i < models4.Count; i++) {
                models4[i].SetActive(true);
            }
        }
    }
}


public class Tank : Entity
{
    [SerializeField] private float speedMove;
    [SerializeField] private float speedRotate;

    [SerializeField] private AudioSource audioIdle;
    [SerializeField] private AudioSource audioMove;
    [SerializeField] float maxVolume = 0.5f;

    [SerializeField] private GameObject model;
    [SerializeField] private GameObject buletPrefab;
    [SerializeField] private Transform gun;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private float damage = 1;
    [SerializeField] private TankModelsOfLevel modelsOnLevel;    
    [SerializeField] private List<GameObject> colorTank = new List<GameObject>();

    public float invulnerabilityTime = 0; 
    private float invulnerabilityTimer = 0;

    public UnityEvent OnDeath;
    public UnityEvent OnTakeDamage;
    public UnityEvent OnSetLevel;
    public UnityEvent OnCollided;

    private Rigidbody rigidbody;
    private Quaternion targetRotation;

    private bool isMoving = false;

    private int currentLevel = 0; public int CurrentLevel => currentLevel;
    private int currentColor = 0;
    private bool iBonus = false; public bool IBonus => iBonus;
    private float timer;


    //[SerializeField] private List<GameObject> modelsOnLevel = new List<GameObject>()

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        targetRotation = model.transform.rotation;
        canTakeDamage = false;
    }

    
    public void Sethealth() {
        if(currentLevel > 2) {
            health = 2;
        }
    }
    public void SetLevel(int newLevel = -1, int newColor = -1, bool isBonus = false, bool ChangeHealth = false) {
        if (newColor == -1) newColor = currentColor;
        if (newLevel == -1) newLevel = currentLevel;
        if (isBonus == false) isBonus = iBonus;
        if (newLevel < 0 || newLevel > modelsOnLevel.models1.Count-1) {
            Debug.LogError("Устанавливаемый левел выходит за диапазон существующих: "+ newLevel);
            ChangeHealth = false;
            return;
        }
        if(newColor < 0 || newColor > colorTank.Count-1) {
            Debug.LogError("Устанавливаемый цвет выходит за диапазон существующих: " + newColor);
            return;
        }

        iBonus = isBonus;

        currentLevel = newLevel;
        currentColor = newColor;
        modelsOnLevel.ShowLevel(newLevel);

        if (ChangeHealth) {
            health = newLevel + 1;
        }

        if(newLevel> 2) {
            damage = 1.5f;
        }

        for (int i = 0; i < colorTank.Count; i++) {
            colorTank[i].SetActive(false);
        }
        colorTank[newColor].SetActive(true);
        OnSetLevel.Invoke();
    }

    private void Update() {
        if(canTakeDamage == false) {
            invulnerabilityTimer += Time.deltaTime;
            if (invulnerabilityTimer > invulnerabilityTime) {
                canTakeDamage = true;
            }
        }

        if (iBonus) {
            timer += Time.deltaTime;
            if (timer > 1) {
                timer = 0;
                if (currentColor + 1 > 3) {
                    SetLevel(currentLevel, 0);
                } else {
                    SetLevel(currentLevel, currentColor + 1);
                }
            }
        }

        Rotate();

        if (isMoving) {
            audioMove.volume = Mathf.MoveTowards(audioMove.volume, maxVolume, 0.1f);
        } else {
            audioMove.volume = Mathf.MoveTowards(audioMove.volume, 0, 0.1f); ;
        }
        audioIdle.volume = maxVolume - audioMove.volume;
    }


    public void Move(Vector3 side) {
        if (side != Vector3.zero) {
            isMoving = true;
        } else {
            isMoving = false;
        }

        Vector3 speed = side * speedMove;
        rigidbody.velocity = new Vector3(speed.x, rigidbody.velocity.y, speed.z);
        if (side != Vector3.zero) {
            targetRotation = Quaternion.LookRotation(side);
        }
    }

    private void Rotate() {
        model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, targetRotation, speedRotate);
    }

    public void Shot() {
        if (currentLevel > 1) {
            StartCoroutine(CreateBulet(2));
        } else {
            StartCoroutine(CreateBulet(1));
        }
    }

    private IEnumerator CreateBulet(int count) {
        for (int i = 0; i < count; i++) {
            GameObject newBulet = Instantiate(buletPrefab, gun.position, gun.transform.rotation, GameManager.Instance.CurrentLevel.transform);
            Bulet bulet = newBulet.GetComponent<Bulet>();
            bulet.Damage = damage;
            bulet.SetTeam(team);
            if (currentLevel > 0) {
                bulet.speed = bulet.speed * 2;
            }

            for (int l = 0; l < colliders.Length; l++) {
                for (int j = 0; j < bulet.Colliders.Length; j++) {
                    Physics.IgnoreCollision(colliders[l], bulet.Colliders[j]);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void Death() {
        base.Death();
        Destroy(gameObject);
        OnDeath.Invoke();
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision) {
        OnCollided.Invoke();
    }

    public override void TakeDamage(float value, Bulet attacker) {
        base.TakeDamage(value, attacker);
        OnTakeDamage.Invoke();
    }
}
