using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unclog : MonoBehaviour, IInteractable {
    [SerializeField] Transform riverLeaves;

    public float removalTime;
    bool isUnclogging = false;
    int index;

    private void OnEnable() {
        OakLeaves.onClog += AddLeaves;
    }

    private void OnDisable() {
        OakLeaves.onClog -= AddLeaves;
    }

    public void PerformInteraction() {
        if (!isUnclogging) StartCoroutine("UnclogRiver");
        else Debug.Log("already unclogging!");
    }

    IEnumerator UnclogRiver() {
        Debug.Log("unclogging!");
        isUnclogging = true;

        index = riverLeaves.childCount - 1;
        while (index >= 0) {
            yield return new WaitForSeconds(removalTime);
            riverLeaves.GetChild(index).gameObject.SetActive(false);
            index--;
        }

        isUnclogging = false;
        Debug.Log("unclogged!");
    }

    void AddLeaves() {
        if (isUnclogging && index < riverLeaves.childCount - 1) {
            Debug.Log("clog");
            index++;
            riverLeaves.GetChild(index).gameObject.SetActive(true);
        }
    }
}
