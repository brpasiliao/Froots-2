using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
    public List<string> dialogue = new List<string>();

    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger && other.CompareTag("Player")) {
            EventBroker.CallPlayDialogue(dialogue);
            gameObject.SetActive(false);
        }
    }
}
