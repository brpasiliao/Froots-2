using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringleafLauncher : MonoBehaviour {
    [SerializeField] Springleaf springleaf;

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

        springleaf.acorn.Launch();
    }

    public void EndLaunch() {
        canLaunch = true;

        if (acornSunk) {
            springleaf.loader.ReloadAcorn();
        }
    }

    public void Sink() {
        // List<Collider> overlappingColliders = new List<Collider>();
        // col.OverlapCollider(new ContactFilter().NoFilter(), overlappingColliders);

        // foreach (Collider2D collider in overlappingColliders) {
        //     if (collider.CompareTag("River")) {
        //         acornSunk = true;
        //     }
        //     if (collider.TryGetComponent<OakLeaves>(out OakLeaves oakLeaves)) {
        //         acornSunk = true;
        //         oakLeaves.Disperse();
        //     }
        //     if (collider.TryGetComponent<LandLeaves>(out LandLeaves landLeaves)) {
        //         landLeaves.Break();
        //     }
        //     if (collider.TryGetComponent<Hole>(out Hole hole)) {
        //         acornSunk = true;
        //         hole.Plug();
        //     }
        // }
    }
}
