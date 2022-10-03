using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GizmoManager : MonoBehaviour {
    [SerializeField] private Transform elevationTracker;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text elevationText;
    [SerializeField] private TMP_Text bestTextEver;

    private float startElevation;

    private void Start() {
        MenuManager.OnExitMenu += Reset;
    }

    private void Reset () {
        startElevation = elevationTracker.position.y;
    }

    private void Update() {
        int relativeElevation = Mathf.RoundToInt(elevationTracker.position.y - startElevation);
        PlayerPrefs.SetInt(PlayerPrefsKey, Mathf.Max(relativeElevation, PlayerPrefs.GetInt(PlayerPrefsKey)));

        keyText.text = PlayerPrefsKey;
        elevationText.text = $"{relativeElevation} m";
        bestTextEver.text = $"best {PlayerPrefs.GetInt(PlayerPrefsKey)} m";
    }

    private string PlayerPrefsKey => $"{MenuManager.Rulestring}-{(int)(MenuManager.FillDensity * 10)}-{MenuManager.ClockPeriod:D2}";
}
