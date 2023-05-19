using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unclog : MonoBehaviour, IInteractable {
    [SerializeField] Transform riverLeaves;

    public float removalTime;
    bool isUnclogging = false;
    int index;

    private void OnEnable() {
        EventBroker.onRiverClog += AddLeaves;
    }

    private void OnDisable() {
        EventBroker.onRiverClog -= AddLeaves;
    }

    public void PerformInteraction() {
        if (!isUnclogging) StartCoroutine("UnclogRiver");
        else EventBroker.CallSendFeedback("Already unclogging the river!");
    }

    IEnumerator UnclogRiver() {
        EventBroker.CallSendFeedback("Started unclogging the river!");
        isUnclogging = true;

        index = riverLeaves.childCount - 1;
        while (index >= 0) {
            yield return new WaitForSeconds(removalTime);
            riverLeaves.GetChild(index).gameObject.SetActive(false);
            index--;
        }

        isUnclogging = false;
        EventBroker.CallSendFeedback("Unclogged the river!");
    }

    void AddLeaves() {
        if (isUnclogging && index < riverLeaves.childCount - 1) {
            index++;
            riverLeaves.GetChild(index).gameObject.SetActive(true);
        }
    }
}
