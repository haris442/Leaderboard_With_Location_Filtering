using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Storage;
using UnityEngine.UI;
public class byteimageGet : MonoBehaviour
{
    [SerializeField] private Image avatarImage;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://leaderboardunity-182fb.appspot.com");
        StorageReference imgFold = storageRef.Child("images");
        StorageReference imgRef = imgFold.Child("2.jpeg");

        imgRef.GetBytesAsync(225 * 225).ContinueWith(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to download image.");
                return;
            }

            byte[] imageData = task.Result;
            Debug.Log("Image data loaded successfully");
            Debug.Log(System.BitConverter.ToString(imageData));            // Use the imageData to create a Texture2D and set it to the sprite of the Image component
            Texture2D texture = new Texture2D(225, 225);
            Debug.Log("Texture created successfully");

            //   Debug.Log("Texture created successfully");
            texture.LoadImage(imageData);
          //  Debug.Log("Texture created successfully");
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            Debug.Log("Sprite created successfully");

            avatarImage.sprite = sprite;

            Debug.Log("Image component updated successfully");

        });

    }

    // Update is called once per frame
    void Update()
    {

    }
}
