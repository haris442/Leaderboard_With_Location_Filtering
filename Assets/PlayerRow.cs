using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Storage;
using UnityEngine.Networking;

public class PlayerRow : MonoBehaviour
{
    public List<PlayerRow> playerData;
    [SerializeField] public Text nameText;
    [SerializeField] public Text scoreText;
    [SerializeField] public Text rankText;
    [SerializeField] public Image playerProfilePic;

    FirebaseStorage storage;
    string downloadUrl;
    bool asyncCompleted = false;
    private string playerId;
    StorageReference imgRef;
    private void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        //  imgRef = storage.GetReferenceFromUrl("https://firebasestorage.googleapis.com/v0/b/leaderboardunity-182fb.appspot.com/o/images%2F2.jpeg?alt=media&token=110e0a50-1ada-48af-b2b9-db1dc75a4c03");
        imgRef = storage.GetReference("images/5.jpeg");
        Debug.Log("Image reference get");
        imgRef.GetDownloadUrlAsync().ContinueWith(task =>
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
                playerProfilePic.sprite = sprite;
            }
        }


    }
    public void SetData(string id, string name, int score, string profilePic, string rank)
    {
        Debug.Log("SetData Called!!!");
        playerId = id;
        nameText.text = name;
        scoreText.text = score.ToString();
        rankText.text = rank;
        Debug.Log("Profile pic name " + profilePic);
       // playerData.Add(this);
       // UpdateImage(profilePic);
    }

    void UpdateImage(string profileImageName)
    {
        imgRef = storage.GetReference(profileImageName);
        Debug.Log("Image reference get");
        imgRef.GetDownloadUrlAsync().ContinueWith(task =>
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




        StartCoroutine(WaitForTaskUpdate());

    }

    IEnumerator WaitForTaskUpdate()
    {
        while (!asyncCompleted)
        {
            yield return null;
        }

        Debug.Log("Coroutine started");
        StartCoroutine(LoadImageUpdate(downloadUrl));
    }

    IEnumerator LoadImageUpdate(string url)
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
                playerProfilePic.sprite = sprite;
            }
        }


    }
}


    
