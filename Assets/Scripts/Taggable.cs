using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taggable : Interactable {
    bool isTagged = false;
    public override void PerformInteraction() {
        GetTagged();
    }

    void GetTagged() {
        if (!isTagged) Debug.Log("tagged!");
        else Debug.Log("already tagged!");
    }
}
