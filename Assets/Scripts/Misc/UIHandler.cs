using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIHandler : MonoBehaviour {
    [SerializeField] TMP_Text acornsText;
    [SerializeField] TMP_Text applewoodText;
    [SerializeField] RectTransform feedbackTextbox;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] RectTransform dialogueTextbox;
    [SerializeField] TMP_Text dialogueText;

    public float popupTime;

    public PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = InputManager.inputActions;
    }

    private void OnEnable() {
        EventBroker.onAcornCount += UpdateAcornCount;
        EventBroker.onApplewoodCount += UpdateApplewoodCount;
        EventBroker.onFeedbackSend += SendFeedback;
        EventBroker.onDialoguePlay += StartPlayDialogue;
    }

    private void OnDisable() {
        EventBroker.onAcornCount -= UpdateAcornCount;
        EventBroker.onApplewoodCount -= UpdateApplewoodCount;
        EventBroker.onFeedbackSend -= SendFeedback;
        EventBroker.onDialoguePlay -= StartPlayDialogue;
    }
    void UpdateAcornCount() {
        acornsText.text = Inventory.GetAcornCountString();
    }

    void UpdateApplewoodCount() {
        applewoodText.text = Inventory.GetApplewoodCountString() + "/20";
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

    void StartPlayDialogue(List<string> dialogue) {
        StartCoroutine("PlayDialogue", dialogue);
    }

    IEnumerator PlayDialogue(List<string> dialogue) {
        dialogueTextbox.gameObject.SetActive(true);

        foreach (string line in dialogue) {
            dialogueText.text = line;
            LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueTextbox);

            yield return 0;
            while (!playerInputActions.Player.PrimaryAction.triggered) 
                yield return null;
        }

        dialogueTextbox.gameObject.SetActive(false);
        EventBroker.CallEndDialogue();
    }
}
