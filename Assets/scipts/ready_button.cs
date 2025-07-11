using UnityEngine;
using UnityEngine.UI; // For Button and Dropdown
using TMPro;
using UnityEngine.EventSystems;

public class ReadyController : MonoBehaviour
{
    public Button readyButton; // Reference to the Ready button
    public TMP_Dropdown dropdown1; // Dropdown for selecting character
    public TMP_Dropdown dropdown2;  // Another Dropdown (if required)
    public Button externalButton;
    public int index;  // This could represent the player ID or a related index
    private bool isReady = false; // Tracks if the user is ready

    private void Awake()
    {
        if (readyButton != null)
        {
            // Add a listener to the button's onClick event
            readyButton.onClick.AddListener(OnReadyButtonPressed);
        }
        else
        {
            Debug.LogWarning("Ready Button is not assigned in the Inspector!");
        }

        if (dropdown1 == null)
        {
            Debug.LogWarning("Dropdown1 is not assigned in the Inspector!");
        }
    }

    private void OnReadyButtonPressed()
    {
        if (!isReady)
        {
            // Ensure that a valid selection has been made in the dropdown
            if (dropdown1.value != 0) // Assuming index 0 is "None" or "Default"
            {
                // Lock the dropdown (make them un-interactable) to prevent further changes
                EventSystem.current.SetSelectedGameObject(externalButton.gameObject); // Focus on the external button (or another UI element)
                dropdown1.interactable = false;
                dropdown2.interactable = false;
                readyButton.interactable = false; // Disable the ready button

                // Mark the player as ready
                isReady = true;

                // Optionally, update the button's text to indicate readiness (can be set to "Ready" or similar)
                readyButton.GetComponentInChildren<TMP_Text>().text = "Ready!";
            }
            else
            {
                Debug.LogWarning("No character selected! Please select a character.");
            }
        }
        else
        {
            Debug.Log("Already marked as ready.");
        }
    }

    private void OnDestroy()
    {
        // Clean up the listener to prevent memory leaks
        if (readyButton != null)
        {
            readyButton.onClick.RemoveListener(OnReadyButtonPressed);
        }
    }
}
