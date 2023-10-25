using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Applewood : MonoBehaviour, IHideable {
    [SerializeField] Animator anim;

    public bool isRevealed { get; set; } = false;

    void OnTriggerEnter(Collider other) {
        if (other.isTrigger && other.CompareTag("Player")) {
            Inventory.applewoods++;
            EventBroker.CallApplewoodCount();
            anim.Play("Collect");
        }
    }

    void Deactivate() {
        gameObject.SetActive(false);
    }

    public void Reveal() {
        isRevealed = true;
        gameObject.SetActive(true);
    }
}
