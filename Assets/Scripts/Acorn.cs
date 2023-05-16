using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : MonoBehaviour, IInteractable, ITaggable {
    public bool IsTagged { get; set; }

    void Awake() {
        IsTagged = false;
    }

    public void PerformInteraction() {
        if (!IsTagged) GetTagged();
        else Debug.Log("already tagged!");
    }

    public void GetTagged() {
        IsTagged = true;
        Debug.Log("tagged!");
    }
}
