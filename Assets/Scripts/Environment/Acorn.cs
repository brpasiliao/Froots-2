using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Acorn : MonoBehaviour, IInteractable, ITaggable, IHideable {
    public bool isTagged { get; set; } = false;
    public bool isRevealed { get; set; } = false;

    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider col;
    [SerializeField] SpriteRenderer srTemp;
    [SerializeField] float velocityThreshold;

    SpriteRenderer sr;

    public Springleaf springleaf;
    public bool isLaunching = false;

    private bool alreadyHit = false;
    private bool isMoving = false;
    private bool wasMoving = false;
    private bool stoppedMoving = false;

    void FixedUpdate() {
        CheckIsMoving();

        stoppedMoving = false;
        if (isMoving) {
            wasMoving = true;
        } else if (wasMoving) {
            wasMoving = false;
            stoppedMoving = true;
        }

        if (isLaunching && stoppedMoving) {
            EndLaunch();
        }
    }

    void CheckIsMoving() {
        if (Math.Abs(rb.velocity.x) < velocityThreshold && 
            Math.Abs(rb.velocity.y) < velocityThreshold && 
            Math.Abs(rb.velocity.z) < velocityThreshold
        ) {
            rb.velocity = Vector3.zero;
            isMoving = false;
        } else {
            isMoving = true;
        }
    }

    void Awake() {
        sr = srTemp;
    }

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<Hole>(out Hole hole) && 
            rb.velocity.x + rb.velocity.z != 0
        ) {
            hole.Plug();
            GetPlugged();
        }

        if (isLaunching && !alreadyHit) {
            if (other.TryGetComponent<LandLeaves>(out LandLeaves landLeaves)) {
                landLeaves.StartBreak();
            }
        }

        if (other.CompareTag("Ground") && isLaunching && !alreadyHit) {
            alreadyHit = true;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, col.radius);
            foreach (Collider hitCollider in hitColliders) {
                // if (collider.CompareTag("River")) {
                //     acornSunk = true;
                // }
                // if (collider.TryGetComponent<OakLeaves>(out OakLeaves oakLeaves)) {
                //     acornSunk = true;
                //     oakLeaves.Disperse();
                // }
                if (hitCollider.TryGetComponent<LandLeaves>(out LandLeaves landLeaves)) {
                    landLeaves.EndBreak();
                }
                if (hitCollider.TryGetComponent<Hole>(out Hole hitHole)) {
                    // acornSunk = true;
                    hitHole.Plug();
                    GetPlugged();
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (isRevealed) {
            col.enabled = true;
            rb.constraints = RigidbodyConstraints.None;
        }

        if (other.TryGetComponent<LandLeaves>(out LandLeaves landLeaves)) {
            landLeaves.Restore();
        }
    }

    public void DoPrimary() {
        if (!isTagged) GetTagged();
        else TryReload();
    }

    public void DoSecondary() {}

    public void GetApproached() {
        sr.color = new Color(1, 1, 1, 0.7f);
    }

    public void GetDeparted() {
        sr.color = new Color(1, 1, 1, 1);
    }

    public void GetTagged() {
        EventBroker.CallSendFeedback("Tagged!");
        isTagged = true;
        Inventory.AddAcorn(this);
        EventBroker.CallAcornCount();
    }

    public void TryReload() {
        if (springleaf != null) {
            springleaf.loader.ReloadAcorn();
        } else {
            EventBroker.CallSendFeedback("Already tagged!");
        }
    }

    public void Reload() {
        transform.localPosition = new Vector3(-0.35f, 1f, 0);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        col.enabled = false;
    }

    public void Launch() {
        col.enabled = true;
        rb.constraints = RigidbodyConstraints.None;
        
        Vector3 force = new Vector3(springleaf.rotation.input.x * springleaf.launchDistanceMultiplier, springleaf.launchHeight, springleaf.rotation.input.y * springleaf.launchDistanceMultiplier);
        // Vector3 force = springleaf.launchAngle * springleaf.launchMultiplier;
        rb.AddForce(force, ForceMode.Impulse);

        isLaunching = true;
        alreadyHit = false;
    }

    public void EndLaunch() {
        isLaunching = false;

        springleaf.launcher.EndLaunch();
    }

    public void AssignToSpringleaf(Springleaf springleaf) {
        transform.SetParent(springleaf.transform);
        this.springleaf = springleaf;
    }

    public void Reveal() {
        isRevealed = true;
        gameObject.SetActive(true);
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ChangeToObject(GameObject acornAnimation) {
        acornAnimation.SetActive(false);
        transform.position = acornAnimation.transform.position;
        gameObject.SetActive(true);
    }

    private void GetPlugged() {
        // set parent to be acorns object
        if (springleaf != null) {
            springleaf.loader.Reset();
        }
        
        SetObjectActive(false);
    }

    public void GetHit(Vector3 angle, float speed) {
        Vector3 velocity = angle * speed;
        rb.AddForce(velocity, ForceMode.Impulse);
    }

    public void SetObjectActive(bool setting) {
        gameObject.SetActive(setting);
    }
}
