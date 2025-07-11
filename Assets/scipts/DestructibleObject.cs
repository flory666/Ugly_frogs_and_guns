
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public GameObject healthBarPrefab; // Prefab-ul pentru bara de sănătate
    private GameObject healthBarInstance; // Instanța barei de sănătate
    private Canvas mainCanvas; // Referință la Canvas-ul principal
    public int maxHealth = 100; // Sănătatea maximă a obiectului
    private int currentHealth;

    void Start()
    {
        // Setăm sănătatea inițială
        currentHealth = maxHealth;

        // Găsește Canvas-ul o singură dată
        mainCanvas = FindObjectOfType<Canvas>();

        if (mainCanvas == null)
        {
            Debug.LogError("Canvas-ul nu a fost găsit în scenă! Asigură-te că ai un obiect Canvas.");
            return;
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Distrugem obiectul dacă sănătatea ajunge la 0
        if (currentHealth <= 0)
        {
            Destroy(healthBarInstance); // Distrugem bara de sănătate
            Destroy(gameObject); // Distrugem obiectul
        }
    }

    void Update()
    {
        // Actualizăm poziția barei de sănătate deasupra obiectului
        if (healthBarInstance != null && Camera.main != null)
        {
            Vector3 worldPosition = transform.position + Vector3.up * 1.5f; // Poziția deasupra obiectului
            healthBarInstance.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
        }
    }
}
