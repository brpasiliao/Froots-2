using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertBehavior : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    public float speed;
    Vector2 destination;

    void Update() {
        destination = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = destination.normalized*speed;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y/2);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.name);
    }
}
