using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Acorn : MonoBehaviour, IInteractable, ITaggable {
    public bool IsTagged { get; set; }
    public SpriteRenderer sr { get; set; }
    [SerializeField] SpriteRenderer srTemp;
    
    public GameObject sprite;
    public Springleaf springleaf;

    void Awake() {
        IsTagged = false;
        sr = srTemp;
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
        if (springleaf != null) {
            EventBroker.CallSendFeedback("Reloaded on springleaf!");
            springleaf.Load();
        } else {
            EventBroker.CallSendFeedback("Already tagged!");
        }
    }
}
