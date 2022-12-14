using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class MenuManager : MonoBehaviour {
    public static event System.Action OnMenuExit, OnMenuEnter;

    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private TMP_InputField rulestringInputField;
    [SerializeField] private TMP_Dropdown presetDropdown;
    [SerializeField] private TMP_Text fillDensityText;
    [SerializeField] private TMP_Text clockPeriodText;

    private static string rulestring = "3/23";
    private static float fillDensity = 0.3f;
    private static int clockPeriod = 10;

    public static string Rulestring => rulestring;
    public static float FillDensity => fillDensity;
    public static int ClockPeriod => clockPeriod;

    private void Start () {
        ToggleMenu();
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) ToggleMenu();
    }

    public void ToggleMenu () {
        if (menuCanvas.activeSelf) {
            menuCanvas.SetActive(false);
            Time.timeScale = 1;
            
            OnMenuExit?.Invoke();
        }
        else {
            menuCanvas.SetActive(true);
            Time.timeScale = 0;

            OnMenuEnter?.Invoke();
        }
    }

    private void NormalizeRulestring (ref string rulestring) {
        rulestring = Regex.Replace(rulestring, "[^0-9\\/]", string.Empty);

        string[] rules = rulestring.Split('/');

        for (int i = 0; i < 2; i++) {
            List<char> normalizedRule = new();
            foreach (char c in rules[i]) {
                if (c == '9') continue;
                if (!normalizedRule.Contains(c)) normalizedRule.Add(c);
            }
            normalizedRule.Sort();
            rules[i] = string.Concat(normalizedRule);
        }

        rulestring = $"{rules[0]}/{rules[1]}";
        if (rules.Length == 3) rulestring += $"/{rules[2]}";
    }

    public void UpdateRulestring (string rulestring) {
        int slashCount = 0;
        foreach (char c in rulestring) if (c == '/') slashCount++;
        
        if (slashCount == 0 || slashCount > 2) {
            rulestringInputField.textComponent.color = Color.red;
            return;
        }
        else {
            rulestringInputField.textComponent.color = new Color(0.1960784f, 0.1960784f, 0.1960784f);
        }

        NormalizeRulestring(ref rulestring);
        if (rulestringInputField.text != rulestring) {
            rulestringInputField.text = rulestring;
            return;
        }
    }

    public void FinalizeRulestring () {
        if (rulestringInputField.textComponent.color == Color.red) {
            rulestringInputField.text = "3/23";
        }

        string[] rules = rulestringInputField.text.Split('/');
        
        if (rules.Length == 3) {
            if (!int.TryParse(rules[2], out int generations) || generations < 3) {
                rulestringInputField.text = $"{rules[0]}/{rules[1]}";
            }
        }
        
        rulestring = rulestringInputField.text;

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
        if (rulestring != "custom") {
            rulestringInputField.text = rulestring;
            MenuManager.rulestring = rulestring;
        }
    }

    private string PresetToRulestring (int id) => id switch {
        00 => "3/23",
        01 => "custom",
        02 => "36/125",
        03 => "34/34",
        04 => "2//3",
        05 => "2/23/8",
        06 => "3678/34678",
        07 => "35678/5678",
        08 => "34/12/3",
        09 => "36/23",
        10 => "2/0",
        11 => "3/345/6",
        12 => "38/13458/6",
        13 => "234/2/5",
        14 => "2/345/4",
        _ => "3/23",
    };

    public void UpdateFillDensity (float value) {
        float fillDensity = Mathf.Round(value * 10f) * 0.1f;
        MenuManager.fillDensity = fillDensity;
        fillDensityText.text = fillDensity.ToString();
    }

    public void UpdateClockPeriod (float value) {
        int clockPeriod = (int)value;
        MenuManager.clockPeriod = clockPeriod;
        clockPeriodText.text = clockPeriod.ToString();
    }
}
