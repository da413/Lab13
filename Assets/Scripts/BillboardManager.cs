using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BillboardManager : MonoBehaviour
{
    public GameObject[] Billboards;
    public Texture2D[] webImages = new Texture2D[3];
    private static readonly string[] webImageLinks =
    {
        "https://new-s3.shelterluv.com/profile-pictures/32de1a7d9bf060fd8de287d189c06563/0c25a05d44fd1785205b5e0122d1898f.jpg",
        "https://upload.wikimedia.org/wikipedia/commons/thumb/1/16/Curious_cat_starring_at_a_lizard.jpg/640px-Curious_cat_starring_at_a_lizard.jpg",
        "https://www.ndow.org/wp-content/uploads/2021/10/Family-of-Raccoons.jpg"
    };

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject billboard in Billboards)
        {
            billboard.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GetWebImage(0, UpdateBillboardTexture);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GetWebImage(1, UpdateBillboardTexture);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GetWebImage(2, UpdateBillboardTexture);
        }
    }

    private void UpdateBillboardTexture(Texture2D newTexture)
    {
        foreach (GameObject billboard in Billboards)
        {
            billboard.GetComponent<Renderer>().material.mainTexture = newTexture;
        }
    }

    private void SaveImage(int index, Texture2D image)
    {
        webImages[index] = image;
    }

    private void GetWebImage(int index, Action<Texture2D> callback)
    {
        if (webImages[index] != null)
        {
            Debug.Log($"Using cached image for index {index}");
            
            callback(webImages[index]);
        }
        else
        {
            Debug.Log($"Downloading image from web: {webImageLinks[index]}");

            StartCoroutine(DownloadImage(index, (texture) =>
            {
                SaveImage(index, texture);
                callback(texture);
            }));
        }
    }

    public IEnumerator DownloadImage(int index, Action<Texture2D> callback)
    { 
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImageLinks[index]);
        yield return request.SendWebRequest();
        callback(DownloadHandlerTexture.GetContent(request));
    }   
}
