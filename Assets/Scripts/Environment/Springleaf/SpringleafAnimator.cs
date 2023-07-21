using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringleafAnimator : MonoBehaviour {
    [SerializeField] Animator animator;

    void Start() {
        SetLaunchAnimationSpeed();
    }

    public void SetAnimatorBool(string animation, bool setting) {
        animator.SetBool(animation, setting);
    }

    public void SetAnimatorInt(string variable, int value) {
        animator.SetInteger(variable, value);
    }

    public void PlayLaunchAnimation() {
        animator.Play("Launch");
    }

    void SetLaunchAnimationSpeed() {
        float speed = SpringleafSingleton.instance.animationSpeed;
        animator.SetFloat("Speed", speed);
    }
}
