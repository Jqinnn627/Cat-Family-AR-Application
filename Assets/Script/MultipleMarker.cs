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

        [TextArea(3, 10)]
        public string descriptionEn;

        [TextArea(3, 10)]
        public string descriptionCn;

        [TextArea(3, 10)]
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
    public GameObject errorPanel;
    public TextMeshProUGUI errorMessage;

    //private Camera arCamera;

    private float lastValidDetectionTime;
    private float scanTimeout = 5f;   // wait 5s before showing error
    private bool isMarkerDetect = false;

    private GameObject currCat = null;
    private string currMarker = "";
    private bool canSpawn = true;
    private AudioClip currSound;
    public string errMsg;

    void Start()
    {
        lastValidDetectionTime = Time.time;
    }
    //void Awake()
    //{
    //    arCamera = Camera.main ?? FindObjectOfType<Camera>();
    //}
    void Update()
    {        
        if (Time.time - lastValidDetectionTime > scanTimeout && !isMarkerDetect)
        {
            switch (LanguageScript.CurrentLang) 
            {
                case "en":
                    errMsg = "No Marker Detected.";
                    ShowError(errMsg);
                    break;
                case "cn":
                    errMsg = "未检测到标记。";
                    ShowError(errMsg);
                    break;
                case "malay":
                    errMsg = "Tiada Marker Dikesan.";
                    ShowError(errMsg);
                    break;
            }
            
        }
    }
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
                HideError();
                isMarkerDetect = true;

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
        isMarkerDetect = false;
        lastValidDetectionTime = Time.time;
        Debug.Log("ClearObject called");
    }
    void ShowError(string message)
    {
        if (errorPanel != null) errorPanel.SetActive(true);
        if (errorMessage != null) errorMessage.text = message;

        Debug.Log("⚠️ Error shown: " + message);
    }

    void HideError()
    {
        errorPanel.SetActive(false);
        Debug.Log("✅ Error hidden.");
    }
}
