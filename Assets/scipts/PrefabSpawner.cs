using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;

public class PrefabSpawner : MonoBehaviour
{
    public Button externalButton;
    public GameObject prefabToSpawn; // Prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public Canvas canvas; // Canvas to attach spawned objects to (optional)
    public int playerLimit = 4; // Maximum number of players (prefabs) allowed

    private int currentPlayerCount = 0; // Tracks the number of currently spawned players
    private PlayerControls controls; // Reference to the PlayerControls input system
    private Dictionary<int, GameObject> spawnedPlayers = new Dictionary<int, GameObject>(); // Tracks spawned players by device ID
    private Dictionary<int, bool> spawnPointStatus = new Dictionary<int, bool>(); // Tracks whether spawn points are occupied
    public static HashSet<int> joinedPlayers = new HashSet<int>(); // Tracks the player controllers that have joined
    public static List<int> playerInputIDs = new List<int>(); // Tracks the input IDs of players

    private void Awake()
    {
        controls = new PlayerControls();

        // Initialize spawn point statuses as empty
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPointStatus[i] = false; // All spawn points are initially empty
        }
        prefabToSpawn.SetActive(false);
    }

    private void OnEnable()
    {
        controls.Enable(); // Enable the controls
        controls.instance.addPlayer.performed += OnSpawnPrefab; // Bind to the input action for adding players
        controls.instance.removePlayer.performed += OnRemovePrefab; // Bind to the input action for removing players
    }

    private void OnDisable()
    {
        controls.instance.addPlayer.performed -= OnSpawnPrefab; // Unbind the add action
        controls.instance.removePlayer.performed -= OnRemovePrefab; // Unbind the remove action
        controls.Disable(); // Disable the controls
    }

    private void OnSpawnPrefab(InputAction.CallbackContext context)
    {prefabToSpawn.SetActive(true);
        int deviceId = context.control.device.deviceId;

        // Check if this device has already joined
        if (joinedPlayers.Contains(deviceId))
        {
            Debug.LogWarning($"Player with device ID {deviceId} has already joined!");
            return; // Exit to prevent duplicate spawning
        }

        // Check if the player limit has been reached
        if (currentPlayerCount >= playerLimit)
        {
            Debug.LogWarning("Player limit reached! Cannot spawn more players.");
            return;
        }

        if (spawnPoints.Length == 0 || prefabToSpawn == null)
        {
            Debug.LogError("No spawn points or prefab assigned!");
            return;
        }

        // Find the first available spawn point
        int spawnIndex = FindAvailableSpawnPoint();
        if (spawnIndex == -1)
        {
            Debug.LogWarning("No available spawn points!");
            return;
        }

        // Spawn the player prefab
        Transform chosenSpawnPoint = spawnPoints[spawnIndex];
        
        GameObject spawnedPlayer = Instantiate(prefabToSpawn, chosenSpawnPoint.position, chosenSpawnPoint.rotation, canvas.transform);
        
        // Update tracking collections
        joinedPlayers.Add(deviceId); // Track this device as joined
        playerInputIDs.Add(deviceId); // Add the raw device ID to the list
        spawnedPlayers[deviceId] = spawnedPlayer; // Store player prefab by device ID
        spawnPointStatus[spawnIndex] = true; // Mark the spawn point as occupied

        // Assign the raw device ID to the prefab's ReadyController or similar script
        ReadyController readyController = spawnedPlayer.GetComponentInChildren<ReadyController>();
        if (readyController != null)
        {
            readyController.index = deviceId; // Use the raw device ID
        }

        // Update player count
        currentPlayerCount++;

        Debug.Log($"Spawned player {deviceId} at spawn point {spawnIndex}");
        prefabToSpawn.SetActive(false);
    }

    private void OnRemovePrefab(InputAction.CallbackContext context)
    {
        int deviceId = context.control.device.deviceId;

        // Check if the player has not joined
        if (!joinedPlayers.Contains(deviceId))
        {
            Debug.LogWarning($"Player {deviceId} is not in the game!");
            return;
        }

        // Find and remove the player's prefab
        if (spawnedPlayers.TryGetValue(deviceId, out GameObject spawnedPlayer))
        {
            // Find the spawn point index associated with the player
            int spawnIndex = FindSpawnPointIndex(spawnedPlayer.transform.position);

            Destroy(spawnedPlayer);
            spawnedPlayers.Remove(deviceId);
            joinedPlayers.Remove(deviceId);
            playerInputIDs.Remove(deviceId); // Remove the raw device ID from the list

            if (spawnIndex != -1)
            {
                spawnPointStatus[spawnIndex] = false; // Mark the spawn point as empty
            }

            currentPlayerCount--;

            Debug.Log($"Removed player with device ID {deviceId} from spawn point {spawnIndex}");
        }
        else
        {
            Debug.LogError($"Player prefab not found for device ID {deviceId}");
        }
    }

    private int FindAvailableSpawnPoint()
    {
        foreach (var spawnPoint in spawnPointStatus)
        {
            if (!spawnPoint.Value)
            {
                return spawnPoint.Key; // Return the index of the first empty spawn point
            }
        }

        return -1; // No available spawn points
    }

    private int FindSpawnPointIndex(Vector3 position)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Vector3.Distance(spawnPoints[i].position, position) < 0.1f) // Allow for small position offsets
            {
                return i;
            }
        }

        return -1; // Spawn point not found
    }
}
