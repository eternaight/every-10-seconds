using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class MenuManager : MonoBehaviour {
    public static event System.Action OnExitMenu;

    [SerializeField] private Behaviour thingToDisable;
    [SerializeField] private TMP_InputField rulestringInputField;
    [SerializeField] private TMP_Dropdown presetDropdown;

    private static string rulestring = "3/23";
    private static float clockPeriod = 10f;

    public static string Rulestring => rulestring;
    public static float ClockPeriod => clockPeriod;

    private void Start () {
        Time.timeScale = 0;
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) GoAway();
    }

    public void GoAway () {
        if (thingToDisable.enabled) {
            thingToDisable.enabled = false;
            Time.timeScale = 1;
            
            OnExitMenu?.Invoke();
        }
        else {
            thingToDisable.enabled = true;
            Time.timeScale = 0;
        }
    }

    public void UpdateRulestring (string rulestring) {
        string cleanRulestring  = Regex.Replace(rulestring, "[^0-9\\/]", string.Empty);
        if (rulestring != cleanRulestring) {
            rulestringInputField.text = cleanRulestring;
            return;
        }
        
        int slashCount = 0;
        foreach (char c in rulestring) if (c == '/') slashCount++;
        
        if (slashCount == 0 || slashCount > 2) {
            rulestringInputField.textComponent.color = Color.red;
            return;
        }
        else {
            rulestringInputField.textComponent.color = new Color(0.1960784f, 0.1960784f, 0.1960784f);
        }

        MenuManager.rulestring = rulestring;
    }

    public void RulestringIFOnEndEdit () {
        for (int i = 0; i < presetDropdown.options.Count; i++) {
            if (PresetToRulestring(i) == rulestring) {
                presetDropdown.value = i;
                return;
            }
        }

        presetDropdown.value = 1;
    }

    public void ChoosePreset (int id) {
        string rulestring = PresetToRulestring(id);
        if (rulestring != "custom") rulestringInputField.text = rulestring;
    }

    private string PresetToRulestring (int id) => id switch {
        0 => "3/23",
        1 => "custom",
        2 => "36/125",
        3 => "36/23",
        4 => "3678/34678",
        _ => "3/23",
    };

    public void UpdateClockPeriod (string clockPeriodString) {
        float clockPeriod = float.Parse(clockPeriodString);
        MenuManager.clockPeriod = clockPeriod;
    }
}
