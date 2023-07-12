using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {  
    [SerializeField] StrawbertBehavior strawbert; 
    [SerializeField] Collider2D col;
   
    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger && other.TryGetComponent<IGrabbable>(out IGrabbable grabbed)) {
            WrapGrasso();
            grabbed.GetGrabbed();
            gameObject.SetActive(false);
        }
    }

    void WrapGrasso() {
        col.enabled = false;
        strawbert.animator.SetAnimatorBool("Wrapping", true);
    }
}
