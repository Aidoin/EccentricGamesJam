using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static Sounds Instance { get; private set; }

    private void Awake() {
        if (Instance) {
            Destroy(this);
            Debug.LogError("Повторная инициализация Sounds");
        } else {
            Instance = this;
        }
    }

    [SerializeField] private AudioSource wallExplosion; public AudioSource WallExplosion => wallExplosion;
}
