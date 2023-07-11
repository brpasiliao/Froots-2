using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertAnimator : MonoBehaviour {
    [SerializeField] Animator animator;

    void Update() {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) {
            SetAnimatorBool("Moving", false);
        } else {
            SetAnimatorBool("Moving", true);
            SetAnimatorFloat("InputX", Input.GetAxisRaw("Horizontal"));
            SetAnimatorFloat("InputY", Input.GetAxisRaw("Vertical"));
        }
    }

    public void EndGrassoAnimation() {
        SetAnimatorBool("Swinging", false);
        SetAnimatorBool("Shooting", false);
        SetAnimatorBool("Wrapping", false);
    }

    public void SetAnimation(bool setting) {
        animator.enabled = setting;
    }

    public void SetAnimatorBool(string animation, bool setting) {
        animator.SetBool(animation, setting);
    }

    public void SetAnimatorFloat(string variable, float value) {
        animator.SetFloat(variable, value);
    }
}
