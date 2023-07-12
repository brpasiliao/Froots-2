using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertBehavior : MonoBehaviour {
    public StrawbertMovement movement;
    public StrawbertGrasso grasso;
    public StrawbertAnimator animator;

    void Update() {
        if (grasso.canGrasso && Input.GetButtonDown("Fire2")) {
            Interact();
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
            ChangeColorOpacity(interacted, 0.7f);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.isTrigger && other.TryGetComponent<IInteractable>(out IInteractable interacted)) {
            ChangeColorOpacity(interacted, 1);
        }
    }

    void Interact() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        ContactFilter2D noFilter = new ContactFilter2D().NoFilter();
        List<Collider2D> overlappingColliders = new List<Collider2D>();
        rb.OverlapCollider(noFilter, overlappingColliders);
        
        foreach (Collider2D collider in overlappingColliders) {
            if (collider.isTrigger && collider.TryGetComponent<IInteractable>(out IInteractable interacted)) {
                interacted.GetInteracted();
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

    void ChangeColorOpacity(IInteractable interacted, float opacity) {
        interacted.sr.color = new Color(1, 1, 1, opacity);
    }
}
