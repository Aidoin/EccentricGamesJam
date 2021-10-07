using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Tank tank; public Tank Tank => tank;
    private Vector3 direction = Vector3.zero;

    [SerializeField] private Vector2 rangeChangeTimeRange = new Vector2(1, 4);
    private float timeMove;
    private float timerMove;
    [SerializeField] private Vector2 shotTimeRange = new Vector2(2, 6);
    private float timeShot;
    private float timerShot;

    private bool IsActive = true;
    private float timeDisabled;
    private float timerDisabled;

    public SpawnerEnemy spawnerEnemy;


    void FixedUpdate() {
        if (IsActive == false) {
            tank.Move(Vector3.zero);
            timerDisabled += Time.fixedDeltaTime;
            if (timerDisabled > timeDisabled) {
                IsActive = true;
                timerDisabled = 0;
            }
            return;
        }

        timerMove += Time.fixedDeltaTime;
        timerShot += Time.fixedDeltaTime;

        if (timerMove > timeMove) {
            timerMove = 0;
            timeMove = Random.Range(rangeChangeTimeRange.x, rangeChangeTimeRange.y);

            ChangeDirectionOfMovement();
        }

        if (timerShot > timeShot) {
            timerShot = 0;
            timeShot = Random.Range(shotTimeRange.x, shotTimeRange.y);

            Shot();
        }
        tank.Move(direction);
    }


    public void SetTamkLevel(int newLevel = -1, int newColor = -1, bool isBonus = false) {
        tank.SetLevel(newLevel, newColor, isBonus);
    }

    public void DisableForWhile(float time) {
        timeDisabled = time;
        IsActive = false;
    }
    public void Disable() {
        timeDisabled = 99999;
        IsActive = false;
    }
    public void Enable() {
        IsActive = true;
    }

    public void ChangeDirectionOfMovement() {
        int rand = Random.Range(1, 5);

        if (rand == 1) {
            if (direction == Vector3.forward) {
                direction = Vector3.left;
            } else {
                direction = Vector3.forward;
            }
        } else
        if (rand == 2) {
            if (direction == Vector3.back) {
                direction = Vector3.right;
            } else {
                direction = Vector3.back;
            }
        } else
        if (rand == 3) {
            if (direction == Vector3.left) {
                direction = Vector3.back;
            } else {
                direction = Vector3.left;
            }
        } else
        if (rand == 4) {
            if (direction == Vector3.right) {
                direction = Vector3.forward;
            } else {
                direction = Vector3.right;
            }
        } else {
            direction = Vector3.zero;
        }
    }

    public void Destroy() {
        GameManager.Instance.AddSlainEnemy();
        spawnerEnemy.EnemyDeath(tank.IBonus);
        Destroy(gameObject);

        if(tank.CurrentLevel == 0) {
            Bonuses.Instance.AddCoins(100);
        } else
        if (tank.CurrentLevel == 1) {
            Bonuses.Instance.AddCoins(200);
        } else
        if (tank.CurrentLevel == 2) {
            Bonuses.Instance.AddCoins(300);
        } else
        if (tank.CurrentLevel == 3) {
            Bonuses.Instance.AddCoins(400);
        }
    }

    private void Shot() {
        tank.Shot();
    }
}






/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Tank tank;
    private Vector3 direction = Vector3.forward;

    [SerializeField] private Vector2 rangeChangeTimeRange = new Vector2(1, 4);
    private float timeMove;
    private float timerMove;
    [SerializeField] private Vector2 shotTimeRange = new Vector2(1, 4);
    private float timeShot;
    private float timerShot;

    private bool isChangeDirectionOfMovement;


    private void Update() {
        if (isChangeDirectionOfMovement) {

            if(direction == Vector3.forward || direction == Vector3.back) {
                float x = tank.transform.position.x - (int)tank.transform.position.x;

                if (x < 0.1f && x > -0.1f) {
                    ChangeDirectionX();
                }
            } else
            if (direction == Vector3.right || direction == Vector3.left) {
                float y = tank.transform.position.y - (int)tank.transform.position.y;
                if (y < 0.1f && y > -0.1f) {
                    ChangeDirectionY();
                }
            }
        }
    }

    void FixedUpdate() {
        timerMove += Time.fixedDeltaTime;
        timerShot += Time.fixedDeltaTime;

        if (timerMove > timeMove) {
            timerMove = 0;
            timeMove = Random.Range(rangeChangeTimeRange.x, rangeChangeTimeRange.y);

            isChangeDirectionOfMovement = true;
        }

        if (timerShot > timeShot) {
            timerShot = 0;
            timeShot = Random.Range(shotTimeRange.x, shotTimeRange.y);

            tank.Shot();
        }
        tank.Move(direction);
    }


    public void ChangeDirectionY() {
        int rand = Random.Range(1, 3);
        if (rand == 1) {
            if (direction == Vector3.forward) {
                ChangeDirection(Vector3.back);
            } else {
                ChangeDirection(Vector3.forward);
            }
        } else
       if (rand == 2) {
            if (direction == Vector3.back) {
                ChangeDirection(Vector3.forward);
            } else {
                ChangeDirection(Vector3.back);
            }
        } else {
            ChangeDirection(Vector3.zero);
        }
    }
    public void ChangeDirectionX() {
        int rand = Random.Range(1, 3);
        if (rand == 1) {
            if (direction == Vector3.left) {
                ChangeDirection(Vector3.right);
            } else {
                ChangeDirection(Vector3.left);
            }
        } else
       if (rand == 2) {
            if (direction == Vector3.right) {
                ChangeDirection(Vector3.left);
            } else {
                ChangeDirection(Vector3.right);
            }
        } else {
            ChangeDirection(Vector3.zero);
        }
    }

    private void ChangeDirection(Vector3 newDirection) {
        direction = newDirection;
        isChangeDirectionOfMovement = false;
    }

    public void Destroy() {
        Destroy(gameObject);
    }
}
*/