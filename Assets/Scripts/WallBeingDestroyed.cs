using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBeingDestroyed : MonoBehaviour
{
    [SerializeField] private List<PartOfWallBeingDestroyed> parts = new List<PartOfWallBeingDestroyed>();

    public void DeleteChildWall(PartOfWallBeingDestroyed wall) {
        parts.Remove(wall);
        if (parts.Count == 0) {
            Delete();
        }
    }

    private void Delete() {
        Destroy(gameObject);
    }
}
