using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertAnimation : MonoBehaviour {
    [SerializeField] Animator animator;

    void Update() {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) {
            animator.SetBool("Moving", false);
        } else {
            animator.SetBool("Moving", true);
            animator.SetFloat("InputX", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("InputY", Input.GetAxisRaw("Vertical"));
        }
    }
}
