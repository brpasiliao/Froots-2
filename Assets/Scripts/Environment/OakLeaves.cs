using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OakLeaves : MonoBehaviour {
    [SerializeField] Collider2D col;
    [SerializeField] SpriteRenderer sr;

    bool isDispersed = false;

    public void Spawn() {
        col.enabled = true;
        sr.enabled = true;
        isDispersed = false;
    }

    public void Clog() {
        if (!Unclog.riverUnclogged && !isDispersed) {
            col.enabled = false;
            EventBroker.CallRiverClog();
        }
    }

    public void Disperse() {
        col.enabled = false;
        sr.enabled = false;
        isDispersed = true;
    }

}
