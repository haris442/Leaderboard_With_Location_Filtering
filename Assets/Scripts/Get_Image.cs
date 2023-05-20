using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Storage;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading;
using System;
using System.Threading.Tasks;

public class Get_Image : MonoBehaviour
{

    
    string downloadUrl;
    bool asyncCompleted = false;
[SerializeField] private Image avatarImage;

    private void Awake()
    {
        
      

    }
    void Start()
    
    {

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference pathReference = storage.GetReference("images/2.jpeg");
        pathReference.GetDownloadUrlAsync().ContinueWith(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                downloadUrl = task.Result.AbsoluteUri;
             
                Debug.Log(downloadUrl);
                asyncCompleted = true;


            }
            else
            {
                Debug.LogError("Failed to get download URL.");
            }
        });




        StartCoroutine(WaitForTask());



    }
    IEnumerator WaitForTask()
    {
        while (!asyncCompleted)
        {
            yield return null;
        }

        Debug.Log("Coroutine started");
        StartCoroutine(LoadImage(downloadUrl));
    }

    IEnumerator LoadImage(string url)
    {
        Debug.Log("Corotuine start");
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to download image: {request.error}");
            }
            else
            {
                // Get the downloaded texture
                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                // Create a new sprite from the texture and set it to the image component
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                avatarImage.sprite = sprite;
            }
        }
    }
  /* IEnumerator LoadImage(string url)
    {
        Debug.Log("Corountine Started");
        // Load the image from the URL
        var www = new WWW(url);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            // Create a new texture from the downloaded image data
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(www.bytes);
            Debug.Log("Texture Created");

            // Create a new sprite from the texture and set it to the image component
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Debug.Log("Sprite Created");

            avatarImage.sprite = sprite;
            Debug.Log("Sprite Assigned");

        }
        else
        {
            Debug.LogError("Failed to download image.");
        }
    }
  */
    // Update is called once per frame
    void Update()
    {

    }
}
