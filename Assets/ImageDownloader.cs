using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;

public class ImageDownloader : MonoBehaviour
{
    public Image imagePrefab;
    public ToastMessage toastMessage;
    //public Button downloadButtonPrefab; // Reference to your download button prefab
    public Transform content;
    private RectTransform targetRectTransform;
    // Replace this with your server URL
    private string serverUrl = "https://e404developers.com/Motivational/";

    // List to store image names
    private List<string> imageNames = new List<string>();

    private void Start()
    {
        targetRectTransform = this.GetComponent<RectTransform>();

        // Populate the list of image names (you should implement this)
        StartCoroutine(PopulateImageNames());

        // Download images and create UI elements
        
    }

    public IEnumerator PopulateImageNames()
    {
        Debug.Log("hi");
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://e404developers.com/Motivational/Size.php"))
        {

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);

                for(int i=1;i<= Int32.Parse(webRequest.downloadHandler.text); i++)
                {
                    imageNames.Add(i + ".jpg");
                    Debug.Log(i + ".jpg");
                }
                Vector2 newSize = targetRectTransform.sizeDelta;
                newSize.y = 900* Int32.Parse(webRequest.downloadHandler.text);
                targetRectTransform.sizeDelta = newSize;
                imageNames.Reverse();
                StartCoroutine(DownloadAndPopulateImages());
            }
        }
    }
    IEnumerator DownloadAndPopulateImages()
    {
        foreach (string imageName in imageNames)
        {
            // Construct the full URL of the image
            string imageUrl = serverUrl + imageName;

            // Request the image
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // Create an image element and set its texture
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Image image = Instantiate(imagePrefab, content);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                // Create a download button for this image
                //Button downloadButton = Instantiate(downloadButtonPrefab, buttonContent);
                string imageNameCopy = imageName; // Create a copy for the lambda function
                image.GetComponent<Button>().onClick.AddListener(() => SaveImageToLocal(imageNameCopy, texture));
            }
        }
    }

    private void SaveImageToLocal(string imageName, Texture2D texture)
    {
        // Save the image to local storage (e.g., persistentDataPath)
        byte[] bytes = texture.EncodeToPNG();
        string downloadPath = "/storage/emulated/0/Download/";
        string filePath = Path.Combine(downloadPath, imageName);
        System.IO.File.WriteAllBytes(filePath, bytes);
        Debug.Log("Image saved to: " + filePath);
        toastMessage.ShowMessage();
    }
}
