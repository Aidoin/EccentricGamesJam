using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public static CameraFollowTarget Instance { get; private set; }

    public Transform Target;
    public float LerpRate = 1;
    public Vector3 Offset;
    public bool Smoothly;
    public bool Rotate;


    private void Awake() {
        if (Instance) {
            Destroy(this);
            Debug.LogError("Попытка повторной инициализации CameraFollowTarget");
        } else {
            Instance = this;
        }
    }

    void Update() {
        ///////// ЭТОТ КОСТЫЛЬ ЖИЗНЕНО НЕОБХОДИМ!! пока не будет решена проблемма что null ссылки после удаления объекта и обычный null не будут равны...
        ///////// Если объект по ссылке HowerInteractBody удалён, то: HowerInteractBody != null. Понимайте как хотите...
        if (Target == null) {
            if (Target.ToString() == "null") {
                Debug.LogWarning("Target = null");
                return;
            }
        }

        if (Smoothly) {
            transform.position = Vector3.Lerp(transform.position, Target.position + Offset, Time.deltaTime * LerpRate);
        } else {
            transform.position = Target.position + Offset;
        }
        if (Rotate) {
            transform.rotation = Target.rotation;
        }
    }


    public void SetOffset(Vector3 newOffset) {
        Offset = newOffset;
    }
    public void SetOffsetX(float newX) {
        Offset.x = newX;
    }
    public void SetOffsetY(float newY) {
        Offset.y = newY;
    }
    public void SetOffsetZ(float newZ) {    
        Offset.z = newZ;
    }
}
