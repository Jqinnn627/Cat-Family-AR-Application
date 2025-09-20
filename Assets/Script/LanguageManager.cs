using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public Sprite enViewCatImgBtn;
    public Sprite cnViewCatImgBtn;
    public Sprite malayViewCatImgBtn;
    public Sprite enQuizImgBtn;
    public Sprite cnQuizImgBtn;
    public Sprite malayQuizImgBtn;
    public Sprite enQuitImgBtn;
    public Sprite cnQuitImgBtn;
    public Sprite malayQuitImgBtn;

    [Header("UI References")]
    public GameObject languagePanel;
    public Button viewCatImgUI;
    public Button quizImgUI;
    public Button quitImgUI;
     
   public void EnLang()
    {
        LanguageScript.SetLanguage("en");
        languagePanel.SetActive(false);
        viewCatImgUI.image.sprite = enViewCatImgBtn;
        quizImgUI.image.sprite = enQuizImgBtn;
        quitImgUI.image.sprite = enQuitImgBtn;
    }
    public void CnLang()
    {
        LanguageScript.SetLanguage("cn");
        languagePanel.SetActive(false);
        viewCatImgUI.image.sprite = cnViewCatImgBtn;
        quizImgUI.image.sprite = cnQuizImgBtn;
        quitImgUI.image.sprite = cnQuitImgBtn;
    }
    public void MelayuLang()
    {
        LanguageScript.SetLanguage("malay");
        languagePanel.SetActive(false);
        viewCatImgUI.image.sprite = malayViewCatImgBtn;
        quizImgUI.image.sprite = malayQuizImgBtn;
        quitImgUI.image.sprite = malayQuitImgBtn;
    }
    public void chooseLang()
    {
        languagePanel.SetActive(true);
    }
}
