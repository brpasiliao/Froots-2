using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Applewood : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger && other.CompareTag("Player")) {
            Inventory.applewoods++;
            EventBroker.CallApplewoodCount();
            gameObject.SetActive(false);
        }
    }
}
