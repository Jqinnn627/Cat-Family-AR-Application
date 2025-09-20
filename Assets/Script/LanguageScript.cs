using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageScript
{
    public static string CurrentLang = "en";//Default English

    public static void SetLanguage(string lang)
    {
        CurrentLang = lang;
        PlayerPrefs.SetString("selectedLang", CurrentLang);
        PlayerPrefs.Save();
        Debug.Log("Lang is" + lang);
    }

    public static string GetLanguage()
    {
        CurrentLang = PlayerPrefs.GetString("selectedLang", CurrentLang);
        return CurrentLang; 
    }
}
