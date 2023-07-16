using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertBehavior : MonoBehaviour {
    public StrawbertMovement movement;
    public StrawbertGrasso grasso;
    public StrawbertAnimator animator;

    void Update() {
        if (Input.GetButtonDown("Fire2")) {
            PrimaryAction();
        } else if (Input.GetButtonDown("Jump")) {
            SecondaryAction();
        }
    }

    private void OnEnable() {
        EventBroker.onDialoguePlay += Stall;
        EventBroker.onDialogueEnd += Unstall;
    }

    private void OnDisable() {
        EventBroker.onDialoguePlay -= Stall;
        EventBroker.onDialogueEnd -= Unstall;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger && other.TryGetComponent<IInteractable>(out IInteractable interacted)) {
            movement.AddInteractable(interacted);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.isTrigger && other.TryGetComponent<IInteractable>(out IInteractable interacted)) {
            movement.RemoveInteractable(interacted);
        }
    }

    void PrimaryAction() {
        if (grasso.canGrasso) {
            IInteractable interacted = movement.GetClosestObject();
            if (interacted != null) {
                interacted.DoPrimary();
            }
        }
    }

    void SecondaryAction() {
        if (grasso.canGrasso) {
            IInteractable interacted = movement.GetClosestObject();
            if (interacted != null) {
                interacted.DoSecondary();
            }
        }
    }

    public void Stall(List<string> dialogue) {
        movement.StopMovement();
        grasso.EndGrasso();
        movement.canMove = false;
        grasso.canGrasso = false;
        animator.SetAnimation(false);
    }

    public void Unstall() {
        movement.canMove = true;
        grasso.canGrasso = true;
        animator.SetAnimation(true);
    }
}
