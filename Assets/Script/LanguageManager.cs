using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public GameObject languagePanel;
   public void EnLang()
    {
        LanguageScript.SetLanguage("en");
        languagePanel.SetActive(false);
    }
    public void CnLang()
    {
        LanguageScript.SetLanguage("cn");
        languagePanel.SetActive(false);
    }
    public void MelayuLang()
    {
        LanguageScript.SetLanguage("malay");
        languagePanel.SetActive(false);
    }
    public void chooseLang()
    {
        languagePanel.SetActive(true);
    }
}
