using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControls : MonoBehaviour
{
    public static ButtonControls Instance;

    [SerializeField] private KeyCode escape; public KeyCode Escape => escape;
    [SerializeField] private KeyCode moveForward; public KeyCode MoveForward => moveForward;
    [SerializeField] private KeyCode moveBackward; public KeyCode MoveBackward => moveBackward;
    [SerializeField] private KeyCode moveLeft; public KeyCode MoveLeft => moveLeft;
    [SerializeField] private KeyCode moveRight; public KeyCode MoveRight => moveRight;
    [SerializeField] private KeyCode shot; public KeyCode Shot => shot;


    private void Awake() {
        if (Instance) {
            Destroy(this);
            Debug.LogError("Попытка повторной инициализации ButtonControls");
        } else {
            Instance = this;
        }
    }
}
