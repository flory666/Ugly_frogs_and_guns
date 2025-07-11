using UnityEngine;
using TMPro; // Ensure TextMesh Pro namespace is used for TMP_Dropdown
using System.Collections.Generic;

public static class SelectedCharacterManager
{
    // Stores mapping of player IDs to selected characters
    public static Dictionary<int, string> PlayerCharacterMap = new Dictionary<int, string>();

    // Assigns characters to players based on TMP_Dropdown selections
  public static void AssignCharacter(List<int> playerInputIDs, GameObject canvas)
{
    // Găsește toate obiectele părinte cu tag-ul "player_select"
    GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("player_select");

    // Dacă nu s-au găsit obiecte, afișează un avertisment și oprește metoda
    if (parentObjects.Length == 0)
    {
        Debug.LogWarning("No parent objects found with the specified tag!");
        return;
    }

    List<string> selectedCharacters = new List<string>();

    // Parcurge fiecare obiect părinte
    foreach (GameObject parentObject in parentObjects)
    {
        // Găsește toate componentele TMP_Dropdown din copii
        TMP_Dropdown[] dropdowns = parentObject.GetComponentsInChildren<TMP_Dropdown>();

        // Procesează doar dropdown-urile cu numele "dropdown1"
        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            if (dropdown.gameObject.name.Equals("dropdown1", System.StringComparison.OrdinalIgnoreCase)) // Verifică numele
            {
                if (dropdown.value != 0) // Verifică dacă selecția nu este cea implicită
                {
                    string selectedText = dropdown.options[dropdown.value].text;
                    selectedCharacters.Add(selectedText);
                }
            }
        }
    }

    // Asociază caracterele selectate cu ID-urile jucătorilor
    for (int i = 0; i < playerInputIDs.Count; i++)
    {
        int playerId = playerInputIDs[i];

        // Verifică dacă există suficiente selecții pentru fiecare jucător
        if (i >= selectedCharacters.Count)
        {
            Debug.LogWarning($"Not enough dropdown selections for player {playerId}. Skipping.");
            continue;
        }

        // Obține caracterul selectat pentru acest jucător
        string selectedCharacter = selectedCharacters[i];

        // Asociază caracterul cu ID-ul jucătorului
        if (PlayerCharacterMap.ContainsKey(playerId))
        {
            PlayerCharacterMap[playerId] = selectedCharacter;
        }
        else
        {
            PlayerCharacterMap.Add(playerId, selectedCharacter);
        }

        Debug.Log($"Assigned character '{selectedCharacter}' to player {playerId}.");
    }
}


    // Removes a player's character mapping
    public static void RemovePlayer(int playerId)
    {
        if (PlayerCharacterMap.ContainsKey(playerId))
        {
            PlayerCharacterMap.Remove(playerId);
            Debug.Log($"Removed player {playerId} from character map.");
        }
        else
        {
            Debug.LogWarning($"Player {playerId} not found in character map.");
        }
    }
}
