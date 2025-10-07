using UnityEngine;
using UnityEngine.UI; // Required for UI components like Button

[RequireComponent(typeof(Button))] // Ensures this script is always on an object with a Button
public class ReplayButtonHandler : MonoBehaviour
{
    void Start()
    {
        // Get the Button component attached to this GameObject
        Button replayButton = GetComponent<Button>();

        // Make sure we found the button
        if (replayButton != null)
        {
            // Clear any existing listeners to be safe
            replayButton.onClick.RemoveAllListeners();

            // Add a new listener that calls the RestartGame method on the GameManager singleton
            replayButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
        }
    }
}