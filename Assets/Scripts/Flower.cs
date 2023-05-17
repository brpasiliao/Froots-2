using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {   
    [SerializeField] Animator animator;
    [SerializeField] Collider2D col;
   
    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger && other.TryGetComponent<IGrabbable>(out IGrabbable grabbedObj)) {
            Debug.Log("grab");

            col.enabled = false;
            animator.SetBool("Wrapping", true);
            grabbedObj.Grab();
            gameObject.SetActive(false);
        }
    }
}
