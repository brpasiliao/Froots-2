using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour, IInteractable {
    [SerializeField] CircleCollider2D col;

    public void PerformInteraction() {
        if (Inventory.springleaves.Count > 0)
            PlaceSpringleaf();
        else Debug.Log("not enough springleaves!");
    }

    void PlaceSpringleaf() {
        Springleaf springleaf = Inventory.springleaves[0];
        springleaf.pit.RemoveSpringleaf();
        springleaf.transform.position = transform.position;
        springleaf.transform.SetParent(gameObject.transform);
        springleaf.pit = this;

        // puts springleaf to back of list
        Inventory.springleaves.RemoveAt(0);
        Inventory.springleaves.Add(springleaf);

        col.enabled = false;
    }

    public void RemoveSpringleaf() {
        col.enabled = true;
    }
}
