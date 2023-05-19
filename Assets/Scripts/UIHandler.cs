using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    [SerializeField] TMP_Text acornsText;
    [SerializeField] RectTransform feedbackTextbox;
    [SerializeField] TMP_Text feedbackText;

    public float popupTime;

    private void OnEnable() {
        EventBroker.onAcornCount += UpdateAcornCount;
        EventBroker.onFeedbackSend += SendFeedback;
    }

    private void OnDisable() {
        EventBroker.onAcornCount -= UpdateAcornCount;
        EventBroker.onFeedbackSend -= SendFeedback;
    }

    void UpdateAcornCount() {
        acornsText.text = Inventory.acorns.Count.ToString();
    }

    void SendFeedback(string text) {
        StopCoroutine("ShowFeedback");
        feedbackTextbox.gameObject.SetActive(false);
        StartCoroutine("ShowFeedback", text);
    }

    IEnumerator ShowFeedback(string text) {
        feedbackTextbox.gameObject.SetActive(true);
        feedbackText.text = text;
        LayoutRebuilder.ForceRebuildLayoutImmediate(feedbackTextbox);
        yield return new WaitForSeconds(popupTime);
        feedbackTextbox.gameObject.SetActive(false);
    }

}
