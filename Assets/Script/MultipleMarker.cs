using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleMarker : MonoBehaviour
{   
    public ARTrackedImageManager imageManager;

    [System.Serializable]
    public class CatData
    {
        public string markerName;
        public GameObject prefab;
        public string nameEn;
        public string nameCn;
        public string nameMalay;
        public string descriptionEn;
        public string descriptionCn;
        public string descriptionMalay;
        public Sprite infoImage;
        public AudioClip sound;
    }

    public List<CatData> meow = new List<CatData>();

    [Header("UI References")]
    public GameObject MainPanel;
    public Button displayInfoButton;
    public Button soundButton;
    public GameObject infoPanel;
    public TextMeshProUGUI catName;
    public TextMeshProUGUI infoDescription;
    public Image infoImageUI;
    public AudioSource source;
    public Image scanner;

    private GameObject currCat = null;
    private string currMarker = "";
    private bool canSpawn = true;
    private AudioClip currSound;

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImageChanged;
        if (displayInfoButton != null ) displayInfoButton.onClick.AddListener(ShowInfo);

        if (soundButton != null) soundButton.onClick.AddListener(PlaySound);
    }
    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImageChanged;
        if (displayInfoButton != null) displayInfoButton.onClick.RemoveListener(ShowInfo);

        if (soundButton != null) soundButton.onClick.RemoveListener(PlaySound);
    }

    void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (!canSpawn) return;

        foreach (var trackedImage in args.added)
        {
            SpawnCat(trackedImage);
        }

        foreach (var trackedImage in args.updated)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                SpawnCat(trackedImage);
            }
        }
    }
    void SpawnCat(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;

        if( currCat != null )
        {
            return;
        }

        foreach (var cat in meow)
        {
            if (cat.markerName == name && cat.prefab != null)
            {
                GameObject newCat = Instantiate(cat.prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                newCat.transform.position = trackedImage.transform.position + trackedImage.transform.forward * 0.02f;
                currCat = newCat;
                currMarker = name;
                canSpawn = false;

                if (MainPanel != null) 
                {
                    MainPanel.SetActive(true);
                    scanner.enabled = false;
                    switch (LanguageScript.CurrentLang)
                    {
                        case "en":
                            catName.text = cat.nameEn;
                            infoDescription.text = cat.descriptionEn;
                            break;
                        case "cn":
                            catName.text = cat.nameCn;
                            infoDescription.text = cat.descriptionCn;
                            break;
                        case "malay":
                            catName.text = cat.nameMalay;
                            infoDescription.text = cat.descriptionMalay;
                            break;
                    }
                    infoImageUI.sprite = cat.infoImage;
                    currSound = cat.sound;
                }

                break;
            }
        }
    }

    public void ShowInfo()
    {
        infoPanel.SetActive(true);
    }
    public void CloseInfo()
    {
        infoPanel.SetActive(false);
    }
    public void PlaySound()
    {
        if (source != null && currSound != null) 
        {
            source.clip = currSound;
            source.Play();
        }
        if (currCat != null)
        {
            Animator animator = currCat.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("RoarTrigger");
            }
        }
    }

    public void ClearObject()
    {
        if (currCat != null)
        {
            Destroy(currCat);
            currCat = null;
        }
        currMarker = "";
        canSpawn = true;
        MainPanel.SetActive(false);
        infoPanel.SetActive(false);
        source.Stop();
        currSound = null;
        scanner.enabled = true;
        Debug.Log("ClearObject called");
    }
}
