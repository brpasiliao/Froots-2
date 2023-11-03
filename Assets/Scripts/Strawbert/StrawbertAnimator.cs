using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertAnimator : MonoBehaviour {
    [SerializeField] Animator animator;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = InputManager.inputActions;
    }

    void Update() {
        if (playerInputActions.Player.Movement.ReadValue<Vector2>().x == 0 && playerInputActions.Player.Movement.ReadValue<Vector2>().y == 0) {
            SetAnimatorBool("Moving", false);
        } else {
            SetAnimatorBool("Moving", true);
            SetAnimatorFloat("InputX", playerInputActions.Player.Movement.ReadValue<Vector2>().x);
            SetAnimatorFloat("InputY", playerInputActions.Player.Movement.ReadValue<Vector2>().y);
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
