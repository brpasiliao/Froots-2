using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {
    [SerializeField] StrawbertBehavior strawbert; 
    [SerializeField] Collider2D col;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float shootForce;
    bool isShooting = false;

    void FixedUpdate() {
        if (isShooting) {
            Vector3 force = new Vector3(shootForce, 0, 0);
            rb.AddForce(force);

            // if (rb.velocity.x + rb.velocity.y == 0) {
            //     isShooting = false;
            //     // strawbert.grasso.EndGrasso();
            //     strawbert.animator.SetAnimatorBool("Swinging", false);
            // }
        }
    }
   
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

    public void Shoot() {
        isShooting = true;
    }

    public void SetObjectActive(bool setting) {
        gameObject.SetActive(setting);
    }
}
