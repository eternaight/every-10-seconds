using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GizmoManager : MonoBehaviour {
    [SerializeField] private Transform elevationTracker;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text elevationText;
    [SerializeField] private TMP_Text bestTextEver;
    [SerializeField] private Image WorldClockProgressBar;

    private float startElevation;
    private float clockMidPeriodTime;

    private void Awake () {
        MenuManager.OnMenuEnter += Reset;
        WorldClock.OnPreTick += CaptureTime;
    }

    private void Reset () {
        startElevation = elevationTracker.position.y;
    }

    private void CaptureTime () {
        clockMidPeriodTime = Time.time;
    }

    private void Update() {
        int relativeElevation = Mathf.RoundToInt(elevationTracker.position.y - startElevation);
        PlayerPrefs.SetInt(PlayerPrefsKey, Mathf.Max(relativeElevation, PlayerPrefs.GetInt(PlayerPrefsKey)));

        keyText.text = PlayerPrefsKey;
        elevationText.text = $"{relativeElevation} m";
        bestTextEver.text = $"best {PlayerPrefs.GetInt(PlayerPrefsKey)} m";

        float normalizedClockProgress = (Time.time - clockMidPeriodTime) / (MenuManager.ClockPeriod * 0.5f);
        normalizedClockProgress = Mathf.Clamp01(normalizedClockProgress);
        normalizedClockProgress = Mathf.Pow(normalizedClockProgress, 2f);
    
        WorldClockProgressBar.color = Color.Lerp(Color.clear, Color.white, normalizedClockProgress);
        WorldClockProgressBar.transform.localScale = new Vector3(1f - normalizedClockProgress, 1, 1);
    }

    private string PlayerPrefsKey => $"{MenuManager.Rulestring}-{(int)(MenuManager.FillDensity * 10)}-{MenuManager.ClockPeriod:D2}";
}
