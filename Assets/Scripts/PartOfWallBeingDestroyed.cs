using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfWallBeingDestroyed : Entity
{
    [SerializeField] private WallBeingDestroyed parentWall;

    //protected override void Death() {
    //    Debug.LogWarning("Стена разрушена");
    //    Destroy(gameObject);

    //    Sounds.Instance.WallExplosion.Play();
    //}

    protected override void Death() {

        bool isCreated = false;

        for (int i = 0; i < attacker.DoingDamageToThem.Count; i++) {
            if (attacker.DoingDamageToThem[i] is PartOfWallBeingDestroyed) {
                isCreated = true;
            }
        }

        if (isCreated) {
            Destroy(audioDeath.gameObject);
        } else {
            attacker.DoingDamageToThem.Add(this);
            audioDeath.Play();
            audioDeath.transform.parent = GameManager.Instance.CurrentLevel.transform;
            Destroy(audioDeath.gameObject, audioDeath.clip.length);
        }

        effectDeath.transform.parent = GameManager.Instance.CurrentLevel.transform;
        effectDeath.SetActive(true);

        parentWall.DeleteChildWall(this);
        Destroy(gameObject);
    }
}
