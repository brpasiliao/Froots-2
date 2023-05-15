using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springleaf : Interactable {
    [SerializeField] Animator animator;
    [SerializeField] int direction;     // 1=up, 2=right, 3=down, 4=left

    void Start() {
        animator.SetInteger("Direction", direction);
    }

    public override void PerformInteraction() {
        Launch();
    }

    void Launch() {
        if (!animator.GetBool("Launching"))
            animator.SetBool("Launching", true);
    }

    void EndLaunch() {
        animator.SetBool("Launching", false);
    }
}
