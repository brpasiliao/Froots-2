using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringleafLauncher : MonoBehaviour {
    [SerializeField] Springleaf springleaf;
    [SerializeField] GameObject acornAnimation;

    bool canLaunch = true;
    bool acornSunk = false;

    public void TryLaunchAcorn() {
        if (canLaunch) {
            LaunchAcorn();
        }
    }

    void LaunchAcorn() {
        canLaunch = false;
        acornSunk = false;
        springleaf.loader.acornLoaded = false;

        springleaf.animator.PlayLaunchAnimation();
        springleaf.animator.SetAnimatorBool("Launching", true);
        acornAnimation.SetActive(true);

        springleaf.acorn.SetObjectActive(false);
    }

    void EndLaunch() {
        canLaunch = true;
        springleaf.animator.SetAnimatorBool("Launching", false);

        if (acornSunk) {
            springleaf.loader.ReloadAcorn();
        } else {
            springleaf.acorn.ChangeToObject(acornAnimation);
        }
    }

    public void Sink() {
        Collider2D col = acornAnimation.GetComponent<Collider2D>();
        List<Collider2D> overlappingColliders = new List<Collider2D>();
        col.OverlapCollider(new ContactFilter2D().NoFilter(), overlappingColliders);

        foreach (Collider2D collider in overlappingColliders) {
            if (collider.CompareTag("River")) {
                acornAnimation.SetActive(false);
                acornSunk = true;
            }
            if (collider.TryGetComponent<OakLeaves>(out OakLeaves oakLeaves)) {
                acornAnimation.SetActive(false);
                acornSunk = true;
                oakLeaves.Disperse();
            }
            if (collider.TryGetComponent<LandLeaves>(out LandLeaves landLeaves)) {
                landLeaves.Break();
            }
            if (collider.TryGetComponent<Hole>(out Hole hole)) {
                acornAnimation.SetActive(false);
                acornSunk = true;
                hole.Plug();
            }
        }
    }

    public void SetAcornAnimationActive(bool setting) {
        acornAnimation.SetActive(setting);
    }
}
