using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventBroker : MonoBehaviour {
    public static event Action onAcornCount;
    public static event Action onApplewoodCount;
    public static event Action onRiverClog;
    public static event Action<string> onFeedbackSend;
    public static event Action<List<string>> onDialoguePlay;
    public static event Action onDialogueEnd;

    public static void CallAcornCount() {
        onAcornCount?.Invoke();
    }

    public static void CallApplewoodCount() {
        onApplewoodCount?.Invoke();
    }

    public static void CallRiverClog() {
        onRiverClog?.Invoke();
    }

    public static void CallSendFeedback(string text) {
        onFeedbackSend?.Invoke(text);
    }

    public static void CallPlayDialogue(List<string> dialogue) {
        onDialoguePlay?.Invoke(dialogue);
    }

    public static void CallEndDialogue() {
        onDialogueEnd?.Invoke();
    }

}
