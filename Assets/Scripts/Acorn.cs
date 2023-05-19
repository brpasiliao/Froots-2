using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Acorn : MonoBehaviour, IInteractable, ITaggable {
    public GameObject sprite;

    public bool IsTagged { get; set; }

    void Awake() {
        IsTagged = false;
    }

    public void PerformInteraction() {
        if (!IsTagged) GetTagged();
        else EventBroker.CallSendFeedback("Already tagged!");
    }

    public void GetTagged() {
        EventBroker.CallSendFeedback("Tagged!");
        IsTagged = true;
        Inventory.acorns.Add(this);
        EventBroker.CallAcornCount();
    }
}
