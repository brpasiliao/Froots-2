using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Direction {
    up, right, down, left
}

public class Springleaf : MonoBehaviour, IInteractable, IApproachable, IGrabbable {
    public SpringleafAnimator animator;
    [SerializeField] GameObject target;
    [SerializeField] GameObject flower;
    [SerializeField] GameObject acornAnim;
    [SerializeField] Direction direction = Direction.right;
    [SerializeField] SpriteRenderer srTemp;
    public static StrawbertBehavior strawbert;

    public SpriteRenderer sr { get; set; }

    bool hasAcorn = false;
    bool canLaunch = true;
    bool acornSunk = false;
    Acorn acorn;

    string assignedAcorn = "Assigned acorn to springleaf!";
    string noAcorns = "Not enough acorns!";

    void Awake() {
        GameObject player = GameObject.FindWithTag("Player");
        strawbert = player.GetComponent<StrawbertBehavior>();
        sr = srTemp;
    }

    void Start() {
        ChangeDirection(direction);
    }

    public void GetInteracted() {
        if (!hasAcorn) {
            TryLoadAcorn();
        } else {
            LaunchAcorn();
        }
    }

    public void GetApproached() {
        target.SetActive(true);
        sr.color = new Color(1, 1, 1, 0.7f);
    }

    public void GetDeparted() {
        target.SetActive(false);
        sr.color = new Color(1, 1, 1, 1);
    }

    void TryLoadAcorn() {
        if (Inventory.HasAcorns()) {
            LoadAcorn();

            EventBroker.CallSendFeedback(assignedAcorn);
            EventBroker.CallAcornCount();

        } else {
            EventBroker.CallSendFeedback(noAcorns);
        }
    }

    void LaunchAcorn() {
        if (canLaunch) {
            canLaunch = false;
            animator.SetAnimatorBool("Launching", true);
            acornAnim.SetActive(true);

            acorn.gameObject.SetActive(false);
            acornSunk = false;
        }
    }

    void EndLaunch() {
        canLaunch = true;
        animator.SetAnimatorBool("Launching", false);

        if (acornSunk) {
            acornAnim.SetActive(true);
        } else {
            acornAnim.SetActive(false);
            acorn.transform.position = acornAnim.transform.position;
            acorn.sprite.transform.rotation = acornAnim.transform.GetChild(0).rotation;
            acorn.gameObject.SetActive(true);
        }
    }

    public void GetGrabbed() {
        StartCoroutine("ChangeDirection");
    }

    IEnumerator ChangeDirection() {
        flower.SetActive(true);
        target.SetActive(true);

        while (!Input.GetButtonDown("Fire1")) {
            RotateTarget();

            if (Input.GetButtonDown("Fire3")) {
                StopCoroutine("ChangeDirection");
            }

            yield return null;
        }

        flower.SetActive(false);
        target.SetActive(true);
        animator.SetAnimatorInt("Direction", (int)direction);
        strawbert.grasso.EndGrasso();
    }

    public void RotateTarget() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        mousePos.x = mousePos.x - targetPos.x;
        mousePos.y = mousePos.y - targetPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
    }

    public void Sink() {
        Collider2D col = acornAnim.GetComponent<Collider2D>();
        List<Collider2D> overlappingColliders = new List<Collider2D>();
        col.OverlapCollider(new ContactFilter2D().NoFilter(), overlappingColliders);

        foreach (Collider2D collider in overlappingColliders) {
            if (collider.CompareTag("River")) {
                acornAnim.SetActive(false);
                acornSunk = true;
            }
            if (collider.TryGetComponent<OakLeaves>(out OakLeaves oakLeaves)) {
                acornAnim.SetActive(false);
                acornSunk = true;
                oakLeaves.Disperse();
            }
            if (collider.TryGetComponent<LandLeaves>(out LandLeaves landLeaves)) {
                landLeaves.Break();
            }
        }
    }

    public void Reload() {
        acorn.gameObject.SetActive(false);
        acornAnim.SetActive(true);
    }

    void LoadAcorn() {
        hasAcorn = true;
        acorn = Inventory.TakeAcorn();
        acorn.AssignToSpringleaf(this);
        acornAnim.SetActive(true);
    }

    void ChangeDirection(Direction newDirection) {
        if (newDirection == Direction.up) {
            target.transform.eulerAngles = new Vector3(0, 0, 90);
        } else if (newDirection == Direction.right) {
            target.transform.eulerAngles = new Vector3(0, 0, 0);
        } else if (newDirection == Direction.down) {
            target.transform.eulerAngles = new Vector3(0, 0, -90);
        } else {
            target.transform.eulerAngles = new Vector3(0, 0, 180);
        }

        animator.SetAnimatorInt("Direction", (int)newDirection);
    }
}
