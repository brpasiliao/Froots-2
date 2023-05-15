using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertBehavior : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    public float speed;
    Vector2 destination;
    List<Collider2D> overlappingColliders = new List<Collider2D>();

    void Update() {
        Move();

        if (Input.GetButtonDown("Fire1")) Interact();
    }

    void Move() {
        destination = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = destination.normalized*speed;
        // transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y/2);
    }

    void Interact() {
        rb.OverlapCollider(new ContactFilter2D().NoFilter(), overlappingColliders);
        foreach (Collider2D collider in overlappingColliders) {
            if (collider.TryGetComponent<Interactable>(out Interactable interactedObj)) {
                interactedObj.PerformInteraction();
            }
        }
    }
}
