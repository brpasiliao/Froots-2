using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Acorn : MonoBehaviour, IInteractable, ITaggable {
    public GameObject sprite;

    public bool IsTagged { get; set; }

    public Springleaf springleaf;

    void Awake() {
        IsTagged = false;
    }

    public void PerformInteraction() {
        if (!IsTagged) GetTagged();
        else Reload();
    }

    public void GetTagged() {
        EventBroker.CallSendFeedback("Tagged!");
        IsTagged = true;
        Inventory.acorns.Add(this);
        EventBroker.CallAcornCount();
    }

    public void Reload() {
        EventBroker.CallSendFeedback("Reloaded on springleaf!");
        springleaf.Load();
    }
}
