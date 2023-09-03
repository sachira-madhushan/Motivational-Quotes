using UnityEngine;
using UnityEngine.UI;

public class ToastMessage : MonoBehaviour
{
    public Text messageText;
    public float displayDuration = 2.0f; // Adjust the display duration as needed
    private float displayTimer = 0.0f;
    private bool displaying = false;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowMessage()
    {
        displaying = true;
        displayTimer = 0.0f;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (displaying)
        {
            displayTimer += Time.deltaTime;
            if (displayTimer >= displayDuration)
            {
                displaying = false;
                gameObject.SetActive(false);
            }
        }
    }
}
