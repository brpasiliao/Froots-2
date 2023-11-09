using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum FlowerState {
    None,
    Shooting,
    Retracting,
}

public class Flower : MonoBehaviour {
    [SerializeField] StrawbertBehavior strawbert; 
    [SerializeField] Collider col;
    [SerializeField] Rigidbody rb;
    [SerializeField] float grassoSpeed;
    [SerializeField] float hitSpeed;
    [SerializeField] float velocityThreshold;

    [SerializeField] float minReach;
    [SerializeField] float maxReach;

    private FlowerState currentState;

    private Vector3 grassoAngle;
    private Vector3 grassoVelocity;
    private bool isShooting = false;
    private bool isMoving = false;
    private bool wasMoving = false;
    private bool stoppedMoving = false;

    private void Awake() {
        transform.localPosition = new Vector3(minReach, 0, 0);
    }

    private void FixedUpdate() {
        switch (currentState) {
            case FlowerState.Shooting:
                if (transform.localPosition.x > maxReach) {
                    StartRetract();
                };
                Shoot();
                break;

            case FlowerState.Retracting:
                if (transform.localPosition.x < minReach) {
                    EndShoot();
                }
                Shoot();
                break;
        }
    }

    public void StartShoot() {
        col.enabled = true;
        rb.constraints = RigidbodyConstraints.None;
        grassoAngle = new Vector3(strawbert.grasso.input.x, 0, strawbert.grasso.input.y);
        grassoVelocity = grassoAngle * grassoSpeed * 10f;

        currentState = FlowerState.Shooting;
    }

    public void StartRetract() {
        grassoVelocity = -grassoVelocity;
        currentState = FlowerState.Retracting;
    }

    public void Shoot() {
        rb.AddForce(grassoVelocity, ForceMode.Acceleration);
    }

    public void EndShoot() {
        currentState = FlowerState.None;

        col.enabled = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.localPosition = new Vector3(minReach, 0, 0);
        strawbert.grasso.EndGrasso();
    }
   
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<Acorn>(out Acorn acorn)) {
            StartRetract();
            acorn.GetHit(grassoAngle, hitSpeed);
        } else if (other.isTrigger && other.TryGetComponent<IGrabbable>(out IGrabbable grabbed)) {
            EndShoot();
            grabbed.GetGrabbed();
        } else if (!other.isTrigger) {
            StartRetract();
        }
    }

    public void SetObjectActive(bool setting) {
        gameObject.SetActive(setting);
    }
}
