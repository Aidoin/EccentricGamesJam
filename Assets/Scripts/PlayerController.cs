using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Tank tank;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform cameraDirection;
    [SerializeField] private Transform cameraTarget; public Transform CameraTarget => cameraTarget;

    public int level { get { return tank.CurrentLevel; } set { tank.SetLevel(value, -1, false, true); } }

    private ButtonControls buttons;

    public SpawnerPlayer spawnerPlayer;

    [SerializeField] private float shotInterval = 0.5f;
    private float timerShot;


    //private Vector3 direction;


    private void Start() {
        buttons = ButtonControls.Instance;
        GameManager.Instance.DisadisableWhenPaused.Add(this);
        UpdateHealth();
    }

    private void Update() {
        timerShot += Time.deltaTime;
        Move();
        GetViewDirection();
        if (Input.GetKeyDown(buttons.Shot)) {
            if (timerShot > shotInterval) {
                tank.Shot();
                timerShot = 0;
            }
        }
    }

    Vector3 lastButton = Vector3.zero;
    Vector3 currentButton = Vector3.zero;
    bool directionHasChanged = false;

    public void UpdateHealth() {
        GameManager.Instance.SetHealth(tank.Health / (tank.CurrentLevel + 1));
    }



    private void Move() {
        //if (Input.GetKey(buttons.MoveForward)) {
        //    currentButton = cameraDirection.forward;
        //}
        //if (Input.GetKey(buttons.MoveBackward)) {
        //    currentButton = -cameraDirection.forward;
        //}
        //if (Input.GetKey(buttons.MoveLeft)) {
        //    currentButton = -cameraDirection.right;
        //}
        //if (Input.GetKey(buttons.MoveRight)) {
        //    currentButton = cameraDirection.right;
        //}

        if (Input.GetKeyDown(buttons.MoveForward)) {
            if (directionHasChanged) {
                directionHasChanged = false;
                currentButton = cameraDirection.forward;
            } else {
                lastButton = currentButton;
                currentButton = cameraDirection.forward;
            }
        }
        if (Input.GetKeyDown(buttons.MoveBackward)) {
            if (directionHasChanged) {
                directionHasChanged = false;
                currentButton = -cameraDirection.forward;
            } else {
                lastButton = currentButton;
                currentButton = -cameraDirection.forward;
            }
        }
        if (Input.GetKeyDown(buttons.MoveLeft)) {
            if (directionHasChanged) {
                directionHasChanged = false;
                currentButton = -cameraDirection.right;
            } else {
                lastButton = currentButton;
                currentButton = -cameraDirection.right;
            }
        }
        if (Input.GetKeyDown(buttons.MoveRight)) {
            if (directionHasChanged) {
                directionHasChanged = false;
                currentButton = cameraDirection.right;
            } else {
                lastButton = currentButton;
                currentButton = cameraDirection.right;
            }
        }




        if (Input.GetKeyUp(buttons.MoveForward)) {
            if (lastButton == cameraDirection.forward) {
                lastButton = Vector3.zero;
            }
            if (currentButton == cameraDirection.forward) {
                if (directionHasChanged) {
                    directionHasChanged = false;
                    lastButton = Vector3.zero;
                }
                currentButton = lastButton;
            }
            if (directionHasChanged) {
                directionHasChanged = false;
                currentButton = Vector3.zero;
            }
        }
        if (Input.GetKeyUp(buttons.MoveBackward)) {
            if (lastButton == -cameraDirection.forward) {
                lastButton = Vector3.zero;
            }
            if (currentButton == -cameraDirection.forward) {
                if (directionHasChanged) {
                    directionHasChanged = false;
                    lastButton = Vector3.zero;
                }
                currentButton = lastButton;
            }
            if (directionHasChanged) {
                directionHasChanged = false;
                currentButton = Vector3.zero;
            }
        }
        if (Input.GetKeyUp(buttons.MoveLeft)) {
            if (lastButton == -cameraDirection.right) {
                lastButton = Vector3.zero;
            }
            if (currentButton == -cameraDirection.right) {
                if (directionHasChanged) {
                    directionHasChanged = false;
                    lastButton = Vector3.zero;
                }
                currentButton = lastButton;
            }
            if (directionHasChanged) {
                directionHasChanged = false;
                currentButton = Vector3.zero;
            }
        }
        if (Input.GetKeyUp(buttons.MoveRight)) {
            if (lastButton == cameraDirection.right) {
                lastButton = Vector3.zero;
            }
            if (currentButton == cameraDirection.right) {
                if (directionHasChanged) {
                    directionHasChanged = false;
                    lastButton = Vector3.zero;
                }
                currentButton = lastButton;
            }
            if (directionHasChanged) {
                directionHasChanged = false;
                currentButton = Vector3.zero;
            }
        }

        if (!Input.GetKey(buttons.MoveRight) && !Input.GetKey(buttons.MoveLeft) && !Input.GetKey(buttons.MoveForward) && !Input.GetKey(buttons.MoveBackward)) {
            lastButton = Vector3.zero;
        }



        //if (Input.GetKeyDown(buttons.MoveForward)) {
        //    direction = cameraDirection.forward;
        //}
        //if (Input.GetKeyDown(buttons.MoveBackward)) {
        //    direction = -cameraDirection.forward;
        //}
        //if (Input.GetKeyDown(buttons.MoveLeft)) {
        //    direction = -cameraDirection.right;
        //}
        //if (Input.GetKeyDown(buttons.MoveRight)) {
        //    direction = cameraDirection.right;
        //}


        //if (Input.GetKeyUp(buttons.MoveForward) && direction == cameraDirection.forward) {
        //    direction = Vector3.zero;
        //    IsButtonBeingHeldDown();
        //}
        //if (Input.GetKeyUp(buttons.MoveBackward) && direction == -cameraDirection.forward) {
        //    direction = Vector3.zero;
        //    IsButtonBeingHeldDown();
        //}
        //if (Input.GetKeyUp(buttons.MoveLeft) && direction == -cameraDirection.right) {
        //    direction = Vector3.zero;
        //    IsButtonBeingHeldDown();
        //}
        //if (Input.GetKeyUp(buttons.MoveRight) && direction == cameraDirection.right) {
        //    direction = Vector3.zero;
        //    IsButtonBeingHeldDown();
        //}
    }

    //private void IsButtonBeingHeldDown() {
    //    if (Input.GetKey(buttons.MoveForward)) {
    //        direction = Vector3.forward;
    //    }
    //    if (Input.GetKey(buttons.MoveBackward)) {
    //        direction = Vector3.back;
    //    }
    //    if (Input.GetKey(buttons.MoveLeft)) {
    //        direction = Vector3.left;
    //    }
    //    if (Input.GetKey(buttons.MoveRight)) {
    //        direction = Vector3.right;
    //    }
    //}

    Vector3 dr;

    private void GetViewDirection() {
        if (dr != cameraDirection.forward) {
            directionHasChanged = true;
            dr = cameraDirection.forward;
        }

        if (Vector3.Angle(playerCamera.forward, Vector3.forward) < 45) {
            cameraDirection.eulerAngles = new Vector3(0, 0, 0);

        } else
        if (Vector3.Angle(playerCamera.forward, Vector3.back) < 45) {
            cameraDirection.eulerAngles = new Vector3(0, 180, 0);

        } else
        if (Vector3.Angle(playerCamera.forward, Vector3.left) < 45) {
            cameraDirection.eulerAngles = new Vector3(0, 270, 0);

        } else
        if (Vector3.Angle(playerCamera.forward, Vector3.right) < 45) {
            cameraDirection.eulerAngles = new Vector3(0, 90, 0);
        }
    }


    void FixedUpdate() {
        tank.Move(currentButton);
    }

    public void Destroy() {
        GameManager.Instance.DisadisableWhenPaused.Remove(this);
        spawnerPlayer.PlayerDeath(); 
        Destroy(gameObject);
    }
}
