using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // Singleton for easy access from other scripts

    public TextMeshProUGUI endReasonText;
    public TextMeshProUGUI nForNextLevelText;
    public TextMeshProUGUI instructionsText;

    private void Awake()
    {
        // Configure the singleton
        if (instance == null)
        {
            instance = this;
            // Uncomment if you want the UIManager to persist between scenes
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ensure that the end reason text is disabled at the start
        if (endReasonText != null)
        {
            endReasonText.enabled = false;
        }

        // Ensure that the N for Next Level text is enabled at all times
        if (nForNextLevelText != null)
        {
            nForNextLevelText.enabled = true;
        }

        // Enable instructions at the start, make them zoom in and disappear 5 seconds later
        if (instructionsText != null)
        {
            instructionsText.enabled = true;
            Invoke("HideInstructions", 5f);
        }


        /*if (instructionsText != null)
        {
            instructionsText.enabled = true;
        }*/
    }

    // Method to show the end reason
    public void ShowEndReason(string reason)
    {
        if (endReasonText != null)
        {
            endReasonText.enabled = true;
            endReasonText.text = reason;
            endReasonText.verticalAlignment = VerticalAlignmentOptions.Top;
        }
    }

    // Method to hide the end reason
    public void HideEndReason()
    {
        if (endReasonText != null)
        {
            endReasonText.enabled = false;
        }
    }

    // Method to show the N for Next Level text
    public void ShowNForNextLevel()
    {
        if (nForNextLevelText != null)
        {
            nForNextLevelText.enabled = true;
        }
    }

    // Method to hide the N for Next Level text
    public void HideNForNextLevel()
    {
        if (nForNextLevelText != null)
        {
            nForNextLevelText.enabled = false;
        }
    }

    // Method to show the instructions
    public void ShowInstructions()
    {
        if (instructionsText != null)
        {
            instructionsText.enabled = true;
        }
    }

    // Method to hide the instructions
    public void HideInstructions()
    {
        if (instructionsText != null)
        {
            instructionsText.enabled = false;
        }
    }
}
