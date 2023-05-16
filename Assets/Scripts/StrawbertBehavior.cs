using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertBehavior : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] GameObject pointer;
    [SerializeField] GameObject flower;
    List<Collider2D> overlappingColliders = new List<Collider2D>();

    bool canMove = true;
    bool canGrasso = true;

    Vector2 destination;
    public float movementSpeed;
    public float rotationSpeed;

    void Update() {
        if (canMove) Move();

        if (canGrasso) {
            if (Input.GetButtonDown("Fire1")) 
                StartCoroutine("UseGrasso");
            if (Input.GetButtonDown("Fire2")) 
                Interact();
        }
    }

    void Move() {
        destination = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = destination.normalized*movementSpeed;
    }

    IEnumerator UseGrasso() {
        canGrasso = false;
        pointer.SetActive(true);
        flower.SetActive(true);
        animator.SetBool("Swinging", true);
        
        yield return 0;
        while (!Input.GetButtonDown("Fire1")) {
            if (Input.GetKey(KeyCode.O)) {
                pointer.transform.Rotate(new Vector3(0, 0, rotationSpeed));
            } else if (Input.GetKey(KeyCode.P)) {
                pointer.transform.Rotate(new Vector3(0, 0, -rotationSpeed));
            }

            // float angle = Vector3.Angle(pointer.transform.position, Input.mousePosition);
            // pointer.transform.eulerAngles = new Vector3(0, 0, angle);

            if (Input.GetButtonDown("Fire3")) {
                EndGrasso();
            }

            yield return null;
        }

        canMove = false;
        animator.SetBool("Shooting", true);
    }

    public void EndGrasso() {
        StopCoroutine("UseGrasso");
        animator.SetBool("Swinging", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Wrapping", false);
        pointer.SetActive(false);
        canMove = true;
        canGrasso = true;
    }

    void Interact() {
        rb.OverlapCollider(new ContactFilter2D().NoFilter(), overlappingColliders);
        foreach (Collider2D collider in overlappingColliders) {
            if (collider.TryGetComponent<IInteractable>(out IInteractable interactedObj)) {
                interactedObj.PerformInteraction();
            }
        }
    }
}
