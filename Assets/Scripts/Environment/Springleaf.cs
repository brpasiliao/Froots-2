using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Springleaf : MonoBehaviour, IInteractable, IGrabbable {
    [SerializeField] Animator animator;
    [SerializeField] GameObject target;
    [SerializeField] GameObject flower;
    [SerializeField] GameObject acornAnim;
    [SerializeField] int direction;     // 1=up, 2=right, 3=down, 4=left
    [SerializeField] SpriteRenderer srTemp;
    public static StrawbertBehavior strawbert;

    public SpriteRenderer sr { get; set; }

    bool hasAcorn = false;
    bool acornSunk = false;
    Acorn acornObj;

    void Awake() {
        strawbert = GameObject.FindWithTag("Player").GetComponent<StrawbertBehavior>();
        sr = srTemp;
    }

    void Start() {
        animator.SetInteger("Direction", direction);

        if (direction == 1) 
            target.transform.eulerAngles = new Vector3(0, 0, 180);
        else if (direction == 2)
            target.transform.eulerAngles = new Vector3(0, 0, 90);
        else if (direction == 3)
            target.transform.eulerAngles = new Vector3(0, 0, 0);
        else
            target.transform.eulerAngles = new Vector3(0, 0, -90);
    }

    public void PerformInteraction() {
        if (!hasAcorn) {
            if (Inventory.acorns.Count > 0) {
                EventBroker.CallSendFeedback("Assigned acorn to springleaf!");
                hasAcorn = true;

                acornObj = Inventory.acorns[0];
                acornObj.transform.SetParent(transform);
                acornObj.springleaf = this;
                acornObj.gameObject.SetActive(false);
                acornAnim.SetActive(true);

                Inventory.acorns.RemoveAt(0);
                EventBroker.CallAcornCount();
            } else {
                EventBroker.CallSendFeedback("Not enough acorns!");
            }
        } else {
            Launch();
        }
    }

    void Launch() {
        if (!animator.GetBool("Launching")) {
            animator.SetBool("Launching", true);
            acornAnim.SetActive(true);
            acornObj.gameObject.SetActive(false);
            acornSunk = false;
        }
    }

    void EndLaunch() {
        animator.SetBool("Launching", false);

        if (acornSunk) {
            acornAnim.SetActive(true);
        } else {
            acornAnim.SetActive(false);
            acornObj.transform.position = acornAnim.transform.position;
            acornObj.sprite.transform.rotation = acornAnim.transform.GetChild(0).rotation;
            acornObj.gameObject.SetActive(true);
        }
    }

    public void Grab() {
        StartCoroutine("ChangeDirection");
    }

    IEnumerator ChangeDirection() {
        flower.SetActive(true);

        while (!Input.GetButtonDown("Fire1")) {
            if (Input.GetKeyDown(KeyCode.Q)) {
                target.transform.Rotate(new Vector3(0, 0, 90));
                direction--;
                if (direction < 1) direction = 4;
            } else if (Input.GetKeyDown(KeyCode.E)) {
                target.transform.Rotate(new Vector3(0, 0, -90));
                direction++;
                if (direction > 4) direction = 1;
            }

            if (Input.GetButtonDown("Fire3")) {
                StopCoroutine("ChangeDirection");
            }

            yield return null;
        }

        flower.SetActive(false);
        animator.SetInteger("Direction", direction);
        strawbert.grasso.EndGrasso();
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

    public void Load() {
        acornObj.gameObject.SetActive(false);
        acornAnim.SetActive(true);
    }
}
