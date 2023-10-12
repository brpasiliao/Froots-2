using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Acorn : MonoBehaviour, IInteractable, ITaggable {
    public bool isTagged { get; set; } = false;
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] SpriteRenderer srTemp;
    [SerializeField] float velocityThreshold;

    SpriteRenderer sr;
    Vector3 previousVelocity = new Vector3(0, 0, 0);
    Vector3 velocityDifference;

    public Springleaf springleaf;
    bool launching;

    void FixedUpdate() {
        if (launching && rb.velocity == Vector3.zero) {
            EndLaunch();
        }

        // velocityDifference = rb.velocity - previousVelocity;
        // if (Math.Abs(velocityDifference.x) < velocityThreshold && 
        //     Math.Abs(velocityDifference.y) < velocityThreshold && 
        //     Math.Abs(velocityDifference.z) < velocityThreshold
        if (Math.Abs(rb.velocity.x) < velocityThreshold && 
            Math.Abs(rb.velocity.y) < velocityThreshold && 
            Math.Abs(rb.velocity.z) < velocityThreshold
        ) {
            rb.velocity = Vector3.zero;
        }
        if (!previousVelocity.Equals(rb.velocity)) {
            previousVelocity = rb.velocity;
        }

        Debug.Log(rb.velocity);
    }

    void Awake() {
        sr = srTemp;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Hole>(out Hole hole) && 
            rb.velocity.x + rb.velocity.z != 0) {
            hole.Plug();
            SetObjectActive(false);
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
        // rb.AddForce(new Vector3(1f, 0, 0));
    }

    public void TryReload() {
        if (springleaf != null) {
            springleaf.loader.ReloadAcorn();
        } else {
            EventBroker.CallSendFeedback("Already tagged!");
        }
    }

    public void Reload() {
        transform.localPosition = new Vector3(-0.35f, 1.15f, 0);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        col.enabled = false;
    }

    public void Launch() {
        col.enabled = true;
        // rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        rb.constraints = RigidbodyConstraints.None;
        // rb.drag = 0f;
        
        // translate angle to x,z coordinates
        Vector3 force = springleaf.launchAngle * springleaf.launchMultiplier;
        rb.AddForce(force, ForceMode.Impulse);

        launching = true;
    }

    public void EndLaunch() {
        Debug.Log("end launch");
        launching = false;
        // rb.drag = 3f;

        springleaf.launcher.EndLaunch();
    }

    public void AssignToSpringleaf(Springleaf springleaf) {
        transform.SetParent(springleaf.transform);
        this.springleaf = springleaf;
    }

    public void ChangeToObject(GameObject acornAnimation) {
        acornAnimation.SetActive(false);
        transform.position = acornAnimation.transform.position;
        gameObject.SetActive(true);
    }

    public void SetObjectActive(bool setting) {
        gameObject.SetActive(setting);
    }
}
