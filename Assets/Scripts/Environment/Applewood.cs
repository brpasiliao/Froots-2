using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Applewood : MonoBehaviour {
    [SerializeField] Animator anim;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger && other.CompareTag("Player")) {
            Inventory.applewoods++;
            EventBroker.CallApplewoodCount();
            anim.Play("Collect");
        }
    }

    void Deactivate() {
        gameObject.SetActive(false);
    }
}
