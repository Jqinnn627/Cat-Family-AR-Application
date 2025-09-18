using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageScript
{
    public static string CurrentLang = "en";//Default English

    public static void SetLanguage(string lang)
    {
        CurrentLang = lang;
        Debug.Log("Lang is" + lang);
    }
}
