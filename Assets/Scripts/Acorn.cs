using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Acorn : MonoBehaviour, IInteractable, ITaggable {
    public GameObject sprite;

    public bool IsTagged { get; set; }

    public static event Action onAcornTag;

    void Awake() {
        IsTagged = false;
    }

    public void PerformInteraction() {
        if (!IsTagged) GetTagged();
        else Debug.Log("acorn already tagged!");
        // roll?
    }

    public void GetTagged() {
        Debug.Log("acorn tagged!");
        IsTagged = true;
        Inventory.acorns.Add(this);
        onAcornTag?.Invoke();
    }
}
