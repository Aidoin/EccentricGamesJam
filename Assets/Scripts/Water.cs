using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private GameObject mesh1;
    [SerializeField] private GameObject mesh2;
    [SerializeField] private float switchingTime;
    private float timer = 1f;
    private bool hzKakNazvat = false;


    private void FixedUpdate() {
        timer += Time.fixedDeltaTime;
        if (timer > switchingTime) {
            timer = 0;
            switching();
        }
    }

    private void switching() {
        if (hzKakNazvat) {
            mesh1.SetActive(true);
            mesh2.SetActive(false);
            hzKakNazvat = false;
        } else {
            mesh1.SetActive(false);
            mesh2.SetActive(true);
            hzKakNazvat = true;
        }
    }
}
