using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.XR.ARSubsystems;

public class ARTrack : MonoBehaviour
{
    [Header("AR Foundation Components")]
    public ARTrackedImageManager imageManager;
    public GameObject catPrefab;

    [Header("UI Elements")]
    public GameObject errorPanel;
    public TextMeshProUGUI errorMessage;

    private Camera arCamera;
    private GameObject spawnedCat;

    private float lastValidDetectionTime;
    private float scanTimeout = 5f;   // wait 5s before showing error
    private bool isErrorVisible = false;

    void Awake()
    {
        arCamera = Camera.main ?? FindObjectOfType<Camera>();
    }

    void Start()
    {
        lastValidDetectionTime = Time.time;

        if (errorPanel != null)
            errorPanel.SetActive(false);

        if (imageManager != null)
            imageManager.trackedImagesChanged += OnChanged;
    }

    void OnDestroy()
    {
        if (imageManager != null)
            imageManager.trackedImagesChanged -= OnChanged;
    }

    void Update()
    {
        // Check if we have a spawned cat that's active
        bool hasCatSpawned = spawnedCat != null && spawnedCat.activeInHierarchy;
        
        bool isAnyMarkerTracked = false;

        if (imageManager != null)
        {
            foreach (var trackedImage in imageManager.trackables)
            {
                if (trackedImage.trackingState == TrackingState.Tracking)
                {
                    isAnyMarkerTracked = true;
                    break;
                }
            }
        }

        // Hide error if we have tracking OR if we have a spawned cat
        if (isAnyMarkerTracked || hasCatSpawned)
        {
            lastValidDetectionTime = Time.time;
            HideError();
        }
        else
        {
            // Only show error if we don't have tracking AND no spawned cat
            if (Time.time - lastValidDetectionTime > scanTimeout && !isErrorVisible)
            {
                ShowError("NO Marker Detected!!");
            }
        }
    }

    void OnChanged(ARTrackedImagesChangedEventArgs e)
    {
        foreach (var img in e.added)
        {
            if (img.trackingState == TrackingState.Tracking)
            {
                SpawnCatPrefab(img);
                lastValidDetectionTime = Time.time;
                HideError();
            }
        }

        foreach (var img in e.updated)
        {
            if (img.trackingState == TrackingState.Tracking)
            {
                if (spawnedCat == null)
                {
                    SpawnCatPrefab(img);
                }
                lastValidDetectionTime = Time.time;
                HideError();
            }
            else if (img.trackingState == TrackingState.None || img.trackingState == TrackingState.Limited)
            {
                // Optional: Handle when tracking is lost
                // You might want to hide the cat or show a "tracking lost" message
                Debug.Log("Tracking lost for image: " + img.referenceImage.name);
            }
        }
    }

    void SpawnCatPrefab(ARTrackedImage img)
    {
        if (spawnedCat != null) return;

        Vector3 offset = img.transform.forward * 0.07f;
        spawnedCat = Instantiate(catPrefab, img.transform.position + offset, img.transform.rotation);

        if (arCamera != null)
        {
            spawnedCat.transform.LookAt(arCamera.transform);
            spawnedCat.transform.rotation = Quaternion.Euler(0, spawnedCat.transform.rotation.eulerAngles.y, 0);
        }

        spawnedCat.transform.parent = img.transform;
        
        // Ensure error is hidden when cat is spawned
        HideError();
        Debug.Log("✅ Prefab spawned, error disabled.");
    }

    void ShowError(string message)
    {
        if (errorPanel != null) errorPanel.SetActive(true);
        if (errorMessage != null) errorMessage.text = message;
        isErrorVisible = true;

        Debug.Log("⚠️ Error shown: " + message);
    }

    void HideError()
    {
        if (errorPanel != null && errorPanel.activeInHierarchy)
        {
            errorPanel.SetActive(false);
            isErrorVisible = false;
            Debug.Log("✅ Error hidden.");
        }
    }
}