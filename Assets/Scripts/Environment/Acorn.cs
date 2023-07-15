using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Acorn : MonoBehaviour, IInteractable, ITaggable {
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
