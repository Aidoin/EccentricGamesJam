using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private Transform camera;

    private float mouseY;

    private void Start() {
        GameManager.Instance.DisadisableWhenPaused.Add(this);
    }

    void Update() {
        transform.RotateAround(center.position, Vector3.up, Input.GetAxis("Mouse X") * 5f);

        mouseY = Mathf.Clamp(mouseY += -Input.GetAxis("Mouse Y"), 45, 80);
        camera.localEulerAngles = new Vector3(mouseY, 0f, 0f);
    }

    private void OnDestroy() {
        GameManager.Instance.DisadisableWhenPaused.Remove(this);
    }
}
