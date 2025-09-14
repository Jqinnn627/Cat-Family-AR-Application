using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    public List<CatData> meow = new List<CatData>();

    private Dictionary<string, GameObject> spawnedCats = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImageChanged;
    }
    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }

    void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            SpawnAnimal(trackedImage);
        }

        foreach (var trackedImage in args.updated)
        {
            UpdateAnimal(trackedImage);
        }

        foreach (var trackedImage in args.removed)
        {
            if (spawnedCats.ContainsKey(trackedImage.referenceImage.name))
            {
                Destroy(spawnedCats[trackedImage.referenceImage.name]);
                spawnedCats.Remove(trackedImage.referenceImage.name);
            }
        }
    }
    void SpawnAnimal(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;

        foreach (var animal in meow)
        {
            if (animal.markerName == name && animal.prefab != null)
            {
                GameObject newAnimal = Instantiate(animal.prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                newAnimal.transform.localPosition = new Vector3(0, 0, 0.05f);
                newAnimal.transform.SetParent(trackedImage.transform);
                spawnedCats[name] = newAnimal;
            }
        }
    }

    void UpdateAnimal(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;

        if (spawnedCats.ContainsKey(name))
        {
            GameObject animal = spawnedCats[name];
            animal.transform.position = trackedImage.transform.position;
            animal.transform.rotation = trackedImage.transform.rotation;
        }
    }
}
