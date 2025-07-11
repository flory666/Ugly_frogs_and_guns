using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CharacterSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // Spawn points for multiple players
    public InputActionAsset inputActions; // Reference to the Input Action Asset
    public int maxPlayers = 4; // Maximum number of players
    public GameObject shield;
    public Vector3 spawnpoint;
    public GameObject endMenu;
    public Button myButton;
    private float interval = 1f; // Intervalul de o secundă
    private float timer = 0f;
    private int currentInputMapIndex = 0; // Keeps track of the next input map index

    void Start()
    {
        int spawnIndex = 0;
        int playerCount = 0;

        // Loop through the PlayerCharacterMap in SelectedCharacterManager
        foreach (var entry in SelectedCharacterManager.PlayerCharacterMap)
        {
            int playerId = entry.Key; // Using playerId from the map
            string character = entry.Value;
            Debug.Log($"Spawning character '{character}' for player ID {playerId}");

            // Normalize character name for consistency
            character = character.ToLower(); // Ensure lowercase for folder consistency

            // Load character prefab from Resources
            GameObject spawnedCharacter = Resources.Load<GameObject>($"Characters/{character}");
            if (spawnedCharacter != null)
            {
                // Get the next spawn point (loop if needed)
                Transform spawnPoint = spawnPoints[spawnIndex];
                GameObject playerCharacter = Instantiate(spawnedCharacter, spawnPoint.position, spawnPoint.rotation);

                // Add the PlayerInput component to handle input actions
                PlayerInput playerInput = playerCharacter.AddComponent<PlayerInput>();
                playerInput.actions = inputActions; // Assign the InputActionAsset
                playerInput.defaultActionMap = "Player"; // Default action map for your characters
                // Ensure the spawn index loops
                spawnIndex = (spawnIndex + 1) % spawnPoints.Length;

                // Move to the next input map
                currentInputMapIndex = (currentInputMapIndex + 1) % maxPlayers;
                playerCount++;
            }
            else
            {
                Debug.LogError($"Character prefab '{character}' not found in Resources/Characters folder.");
            }
        }
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int playerCount = players.Length;

        if (playerCount <= 1)
        {
            Debug.Log("endgame");
            endMenu.SetActive(true);
EventSystem.current.SetSelectedGameObject(myButton.gameObject);
        }
        timer += Time.deltaTime; // Incrementăm timpul scurs

        if (timer >= interval)
        {
            timer = 0f; // Resetăm timer-ul

            // Generăm un număr random între 0 și 100
            int chance = Random.Range(1, 101); // Include și 100

            // Verificăm dacă este în intervalul de 5%
            if (chance <= 5)
            {
                Instantiate(shield, spawnpoint, Quaternion.identity);
            }
        }
    }
    public void endgame()
    {


        // Numărul de obiecte găsite

    }
}
