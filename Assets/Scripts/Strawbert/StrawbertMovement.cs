using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertMovement : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float movementSpeed;

    public bool canMove { get; set; } = true;

    void FixedUpdate() {
        if (canMove) {
            Move();
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
}
