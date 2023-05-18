using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour {
    [SerializeField] TMP_Text acornsText;
    // [SerializeField] TMP_Text springleavesText;

    private void OnEnable() {
        Acorn.onAcornTag += UpdateAcornCount;
        // Springleaf.onSpringleafTag += UpdateSpringleafCount;
    }

    private void OnDisable() {
        Acorn.onAcornTag -= UpdateAcornCount;
        // Springleaf.onSpringleafTag -= UpdateSpringleafCount;
    }

    void UpdateAcornCount() {
        acornsText.text = "Acorns: " + Inventory.acorns.Count;
    }

    // void UpdateSpringleafCount() {
    //     springleavesText.text = "Springleaves: " + Inventory.springleaves.Count;
    // }
}
