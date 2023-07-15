using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringleafAnimator : MonoBehaviour {
    [SerializeField] Animator animator;

    public void SetAnimatorBool(string animation, bool setting) {
        animator.SetBool(animation, setting);
    }

    public void SetAnimatorInt(string variable, int value) {
        animator.SetInteger(variable, value);
    }
}
