using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Acorn : MonoBehaviour, IInteractable, IApproachable, ITaggable {
    public bool isTagged { get; set; } = false;
    public SpriteRenderer sr { get; set; }
    [SerializeField] SpriteRenderer srTemp;
    
    public GameObject sprite;
    public Springleaf springleaf;

    void Awake() {
        sr = srTemp;
    }

    public void GetInteracted() {
        if (!isTagged) GetTagged();
        else Reload();
    }

    public void GetApproached() {
        sr.color = new Color(1, 1, 1, 0.7f);
    }

    public void GetDeparted() {
        sr.color = new Color(1, 1, 1, 1);
    }

    public void GetTagged() {
        EventBroker.CallSendFeedback("Tagged!");
        isTagged = true;
        Inventory.AddAcorn(this);
        EventBroker.CallAcornCount();
    }

    public void Reload() {
        if (springleaf != null) {
            EventBroker.CallSendFeedback("Reloaded on springleaf!");
            springleaf.Reload();
        } else {
            EventBroker.CallSendFeedback("Already tagged!");
        }
    }

    public void AssignToSpringleaf(Springleaf springleaf) {
        transform.SetParent(springleaf.transform);
        this.springleaf = springleaf;
        gameObject.SetActive(false);
    }
}
