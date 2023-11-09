using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertMovement : MonoBehaviour {
    [SerializeField] Rigidbody rb;
    [SerializeField] float movementSpeed;

    private PlayerInputActions playerInputActions;

    List<IInteractable> interactableObjects = new List<IInteractable>();
    IInteractable closestObject;
    bool isFindingClosestObject;

    public bool canMove { get; set; } = true;

    private void Awake() {
        playerInputActions = InputManager.inputActions;

    }


    void FixedUpdate() {
        if (canMove) {
            Move();
        }

        if (isFindingClosestObject) {
            FindClosestObject();
        }
    }

    void Move() {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        Vector3 destination = new Vector3(inputVector.x, 0, inputVector.y);
        rb.velocity = destination.normalized * movementSpeed;
    }

    public void StopMovement() {
        rb.velocity = Vector3.zero;
    }

    public void FindClosestObject() {
        IInteractable newClosestObject = null;
        float minDistance = 0;

        foreach (IInteractable interactable in interactableObjects) {
            float distance = CalculateDistanceFrom(interactable);

            if (newClosestObject == null || distance < minDistance) {
                newClosestObject = interactable;
                minDistance = distance;
            } 
        }

        if (closestObject != newClosestObject) {
            closestObject.GetDeparted();
            newClosestObject.GetApproached();

            closestObject = newClosestObject;
        }        
    }

    float CalculateDistanceFrom(IInteractable other) {
        MonoBehaviour otherObject = other as MonoBehaviour;
        Transform otherTransform = otherObject.transform;
        return Vector3.Distance(transform.position, otherTransform.position);
    }

    public void AddInteractable(IInteractable toAdd) {
        interactableObjects.Add(toAdd);

        if (interactableObjects.Count == 1) {
            closestObject = interactableObjects[0];
            closestObject.GetApproached();
        } else if (interactableObjects.Count > 1) {
            isFindingClosestObject = true;
        }
    }

    public void RemoveInteractable(IInteractable toRemove) {
        interactableObjects.Remove(toRemove);

        if (interactableObjects.Count == 1) {
            closestObject = interactableObjects[0];
            isFindingClosestObject = false;
        } else if (interactableObjects.Count == 0) {
            closestObject = null;
            toRemove.GetDeparted();
        }
    }

    public IInteractable GetClosestObject() {
        return closestObject;
    }
}
