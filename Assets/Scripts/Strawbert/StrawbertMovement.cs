using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertMovement : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float movementSpeed;

    List<IInteractable> interactableObjects = new List<IInteractable>();
    IInteractable closestObject;
    bool isFindingClosestObject;

    public bool canMove { get; set; } = true;

    void FixedUpdate() {
        if (canMove) {
            Move();
        }

        if (isFindingClosestObject) {
            FindClosestObject();
        }
    }

    void Move() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 destination = new Vector2(horizontalInput, verticalInput);
        rb.velocity = destination.normalized * movementSpeed;
    }

    public void StopMovement() {
        rb.velocity = Vector2.zero;
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
            toRemove.GetDeparted();
        }
    }

    public IInteractable GetClosestObject() {
        return closestObject;
    }
}
