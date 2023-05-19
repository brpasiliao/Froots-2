using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawbertBehavior : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] GameObject pointer;
    [SerializeField] GameObject flower;
    List<Collider2D> overlappingColliders = new List<Collider2D>();

    List<string> acornDialogue = new List<string> {
        "I saw some springleaves around here... ",
        "Maybe I can launch these acorns with them.",
        "Press right click on mouse to tag an acorn to use for later",
    };
    List<string> springleafDialogue = new List<string> {
        "There's a springleaf!",
        "Press right click on mouse to load an acorn, and again to launch it.",
        "To change its direction, press left click on mouse to use the grasso and QE to aim.",
        "Then, press QE again to rotate the springleaf.",
    };
    List<string> applewoodDialogue = new List<string> {
        "This is great wood for building houses!",
        "I knew helping the Apples and Pears was a good idea!",
    };

    bool firstAcorn = true;
    bool firstSpringleaf = true;
    bool firstApplewood = true;

    bool canMove = true;
    bool canGrasso = true;

    Vector2 destination;
    public float movementSpeed;
    public float rotationSpeed;

    private void OnEnable() {
        EventBroker.onDialoguePlay += Stall;
        EventBroker.onDialogueEnd += Unstall;
    }

    private void OnDisable() {
        EventBroker.onDialoguePlay -= Stall;
        EventBroker.onDialogueEnd -= Unstall;
    }

    void FixedUpdate() {
        if (canMove) Move();
    }

    void Update() {
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
            if (Input.GetKey(KeyCode.Q)) {
                pointer.transform.Rotate(new Vector3(0, 0, rotationSpeed));
            } else if (Input.GetKey(KeyCode.E)) {
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
            if (collider.isTrigger && collider.TryGetComponent<IInteractable>(out IInteractable interactedObj)) {
                interactedObj.PerformInteraction();
            }
        }
    }

    public void Stall(List<string> dialogue) {
        canMove = false;
        canGrasso = false;
        animator.enabled = false;
        rb.velocity = Vector2.zero;
    }

    public void Unstall() {
        canMove = true;
        canGrasso = true;
        animator.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (firstAcorn && other.GetComponent<Acorn>() != null) {
            EventBroker.CallPlayDialogue(acornDialogue);
            firstAcorn = false;
        }
        if (firstSpringleaf && other.GetComponent<Springleaf>() != null) {
            EventBroker.CallPlayDialogue(springleafDialogue);
            firstSpringleaf = false;
        }
        if (firstApplewood && other.GetComponent<Applewood>() != null) {
            EventBroker.CallPlayDialogue(applewoodDialogue);
            firstApplewood = false;
        } 
    }
}
