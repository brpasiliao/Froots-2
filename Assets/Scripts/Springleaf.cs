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
    public static StrawbertBehavior strawbert;

    bool hasAcorn = false;
    bool acornSunk = false;
    Acorn acornObj;

    void Awake() {
        strawbert = GameObject.FindWithTag("Player").GetComponent<StrawbertBehavior>();
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
                acornAnim.SetActive(true);
                
                acornObj = Inventory.acorns[0];
                acornObj.gameObject.SetActive(false);
                acornObj.transform.SetParent(transform);

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
        target.SetActive(true);

        while (!Input.GetButtonDown("Fire1")) {
            if (Input.GetKeyDown(KeyCode.O)) {
                target.transform.Rotate(new Vector3(0, 0, 90));
                direction--;
                if (direction < 1) direction = 4;
            } else if (Input.GetKeyDown(KeyCode.P)) {
                target.transform.Rotate(new Vector3(0, 0, -90));
                direction++;
                if (direction > 4) direction = 1;
            }

            if (Input.GetButtonDown("Fire3")) {
                target.SetActive(false);
                StopCoroutine("ChangeDirection");
            }

            yield return null;
        }

        target.SetActive(false);
        flower.SetActive(false);
        animator.SetInteger("Direction", direction);
        strawbert.EndGrasso();
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
        }
    }
}
