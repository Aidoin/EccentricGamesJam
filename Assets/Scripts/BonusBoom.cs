using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBoom : MonoBehaviour
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
        List<Tank> tanks = new List<Tank>();
        for (int i = 0; i < SpawnerEnemy.ActiveEnemy.Count; i++) {
            tanks.Add(SpawnerEnemy.ActiveEnemy[i].Tank);
        }
        for (int i = 0; i < tanks.Count; i++) {
            tanks[i].TakeDamage(99, null);
        }
        pickUp.Play();
        pickUp.transform.parent = GameManager.Instance.CurrentLevel.transform;
        Destroy(pickUp.gameObject, pickUp.clip.length);
        Destroy(gameObject);
    }
}
