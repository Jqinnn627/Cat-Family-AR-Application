using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARTrack : MonoBehaviour
{
    public ARTrackedImageManager imageManager;
    public GameObject catPrefab;

    private Camera arCamera;

    void Awake()
    {
        // Find AR Camera
        arCamera = Camera.main;
        if (arCamera == null)
        {
            arCamera = FindObjectOfType<Camera>();
        }
    }

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnChanged;
    }

    void OnChanged(ARTrackedImagesChangedEventArgs e)
    {
        foreach (var img in e.added)
        {
            // Always spawn in front of the marker
            Vector3 offset = img.transform.forward * 0.07f; // 7 cm in front
            GameObject cat = Instantiate(catPrefab, img.transform.position + offset, img.transform.rotation);

            // Optional: face the AR camera
            if (arCamera != null)
            {
                cat.transform.LookAt(arCamera.transform);
                cat.transform.rotation = Quaternion.Euler(0, cat.transform.rotation.eulerAngles.y, 0);
            }

            // Parent to marker so it follows marker movement
            cat.transform.parent = img.transform;
        }
    }
}
