using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Direction {
    up, right, down, left
}

public class Springleaf : MonoBehaviour, IInteractable, IGrabbable {
    public SpringleafAnimator animator;
    [SerializeField] GameObject target;
    [SerializeField] GameObject flower;
    [SerializeField] GameObject acornAnim;
    [SerializeField] Direction direction = Direction.right;
    [SerializeField] SpriteRenderer srTemp;
    public static StrawbertBehavior strawbert;

    bool canLaunch = true;
    bool acornAssigned = false;
    bool acornLoaded = false;
    bool acornSunk = false;
    Acorn acorn;
    SpriteRenderer sr;

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

    public void DoPrimary() {
        if (!acornAssigned) {
            TryLoadAcorn();
        } else if (!acornLoaded) {
            Reload();
        }else {
            LaunchAcorn();
        }
    }

    public void DoSecondary() {
        StartCoroutine("ChangeDirection");
    }

    public void GetApproached() {
        target.SetActive(true);
        sr.color = new Color(1, 1, 1, 0.7f);
    }

    public void GetDeparted() {
        target.SetActive(false);
        sr.color = new Color(1, 1, 1, 1);
    }

    public void GetGrabbed() {
        strawbert.transform.position = transform.position;
        strawbert.grasso.EndGrasso();
    }

    void TryLoadAcorn() {
        if (Inventory.HasAcorns()) {
            LoadAcorn();
            acornLoaded = true;

            EventBroker.CallSendFeedback(assignedAcorn);
            EventBroker.CallAcornCount();

        } else {
            EventBroker.CallSendFeedback(noAcorns);
        }
    }

    void LaunchAcorn() {
        if (canLaunch) {
            acornLoaded = false;

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

    IEnumerator ChangeDirection() {
        strawbert.Stall(null);
        flower.SetActive(true);
        target.SetActive(true);

        yield return 0;
        while (!Input.GetButtonDown("Jump")) {
            RotateTarget();
            yield return null;
        }

        flower.SetActive(false);
        target.SetActive(true);
        animator.SetAnimatorInt("Direction", (int)direction);
        strawbert.Unstall();
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
        acornLoaded = true;
        acorn.gameObject.SetActive(false);
        acornAnim.SetActive(true);
    }

    void LoadAcorn() {
        acorn = Inventory.TakeAcorn();
        acorn.AssignToSpringleaf(this);

        acornAssigned = true;
        Reload();
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
