using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCoin : MonoBehaviour
{
    [SerializeField] private AudioSource pickUp;
    [SerializeField] private int coins = 500;

    private void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody) {
            if (other.attachedRigidbody.GetComponent<Tank>()) {
                if (other.attachedRigidbody.GetComponent<Tank>().Team == Team.Player) {
                    PickUp();
                }
            }
        }
    }

    private void PickUp() {
        Bonuses.Instance.AddCoins(coins);
        pickUp.Play();
        pickUp.transform.parent = GameManager.Instance.CurrentLevel.transform;
        Destroy(pickUp.gameObject, pickUp.clip.length);
        Destroy(gameObject);
    }
}