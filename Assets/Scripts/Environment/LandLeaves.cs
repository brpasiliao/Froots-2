using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandLeaves : MonoBehaviour {
    [SerializeField] GameObject hidden;
    [SerializeField] Collider solidCollider;

    public void StartBreak() {
        solidCollider.enabled = false;
    }

    public void EndBreak() {
        IHideable hideable = hidden.GetComponent<IHideable>();
        hideable.Reveal();
        gameObject.SetActive(false);
    }

    public void Restore() {
        solidCollider.enabled = true;
    }
}
