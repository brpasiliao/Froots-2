using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Acorn : MonoBehaviour, IInteractable, ITaggable {
    public bool isTagged { get; set; } = false;
    [SerializeField] SpriteRenderer srTemp;

    SpriteRenderer sr;
    
    public GameObject sprite;
    public Springleaf springleaf;

    void Awake() {
        sr = srTemp;
    }

    public void DoPrimary() {
        if (!isTagged) GetTagged();
        else Reload();
    }

    public void DoSecondary() {}

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
            springleaf.loader.ReloadAcorn();
        } else {
            EventBroker.CallSendFeedback("Already tagged!");
        }
    }

    public void AssignToSpringleaf(Springleaf springleaf) {
        transform.SetParent(springleaf.transform);
        this.springleaf = springleaf;
    }

    public void ChangeToObject(GameObject acornAnimation) {
        acornAnimation.SetActive(false);
        transform.position = acornAnimation.transform.position;
        gameObject.SetActive(true);
    }

    public void SetObjectActive(bool setting) {
        gameObject.SetActive(setting);
    }
}
