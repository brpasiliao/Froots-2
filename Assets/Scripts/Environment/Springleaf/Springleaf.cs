using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Springleaf : MonoBehaviour, IInteractable, IGrabbable {
    public SpringleafLoader loader;
    public SpringleafLauncher launcher;
    public SpringleafRotation rotation;
    public SpringleafAnimator animator;
    public Acorn acorn;
    
    [SerializeField] SpriteRenderer srTemp;
    public static StrawbertBehavior strawbert;

    public Vector3 launchAngle;
    public float launchMultiplier;

    SpriteRenderer sr;

    void Awake() {
        GameObject player = GameObject.FindWithTag("Player");
        strawbert = player.GetComponent<StrawbertBehavior>();
        sr = srTemp;
    }

    public void DoPrimary() {
        if (!loader.acornLoaded) {
            loader.TryLoadAcorn();
        } else {
            launcher.TryLaunchAcorn();
        }
    }

    public void DoSecondary() {
        rotation.RotateSpringleaf();
    }

    public void GetApproached() {
        rotation.SetTargetActive(true);
        sr.color = new Color(1, 1, 1, 0.7f);
    }

    public void GetDeparted() {
        rotation.SetTargetActive(false);
        sr.color = new Color(1, 1, 1, 1);
    }

    public void GetGrabbed() {
        strawbert.transform.position = transform.position;
    }

    public void StallStrawbert(List<string> dialogue) {
        strawbert.Stall(dialogue);
    }

    public void UnstallStrawbert() {
        strawbert.Unstall();
    }
}
