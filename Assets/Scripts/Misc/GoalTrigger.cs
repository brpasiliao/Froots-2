using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour {
    [SerializeField] GameObject hidden;

    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<Acorn>() != null) {
            hidden.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
