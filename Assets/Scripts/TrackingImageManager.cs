using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public class KeyValue
{
    public string key;
    public GameObject value;
}


public class TrackingImageManager : MonoBehaviour
{
    public List<KeyValue> dataList = new List<KeyValue>();

    Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> refObjs = new Dictionary<string, GameObject>();
    ARTrackedImageManager imgManager;
    // Start is called before the first frame update
    void Start()
    {
        imgManager = GetComponent<ARTrackedImageManager>();
        imgManager.trackedImagesChanged += OnImageTracked;

        foreach (KeyValue kv in dataList)
        {
            if (!objects.ContainsKey(kv.key))
            {
                objects.Add(kv.key, kv.value);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnImageTracked(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage image in eventArgs.added)
        {
            string imgName = image.referenceImage.name;

            if (!objects.ContainsKey(imgName))
                continue;
            refObjs[imgName] = Instantiate(objects[imgName], image.transform.position,image.transform.rotation);
        }
        foreach (ARTrackedImage image in eventArgs.updated)
        {
            string imgName = image.referenceImage.name;

            if (!refObjs.ContainsKey(imgName))
                continue;
            refObjs[imgName].transform.SetLocalPositionAndRotation(image.transform.position, image.transform.rotation);
        }
        foreach (ARTrackedImage image in eventArgs.removed)
        {
            string imgName = image.referenceImage.name;

            if (!refObjs.ContainsKey(imgName))
                continue;
            Destroy(refObjs[imgName]);
            refObjs.Remove(imgName);
        }
    }
}
