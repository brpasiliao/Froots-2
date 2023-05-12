using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertAnimation : MonoBehaviour {
    Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        input.Normalize();

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }
}
