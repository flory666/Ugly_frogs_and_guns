using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Numerics;
public class select_menu : MonoBehaviour
{
   private HashSet<int> joinedPlayers = new HashSet<int>();
   private int map1 = 0;
   private int map2 = 0;
   public GameObject canvas;
   public GameObject warning;
   public Transform warspawn;
   private bool warn = false;

   public void StartGame()
   {
      SelectedCharacterManager.AssignCharacter(PrefabSpawner.playerInputIDs, canvas);
        GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("player_select");

        // Dacă nu s-au găsit obiecte, afișează un avertisment și oprește metoda
        if (parentObjects.Length == 0)
        {
            Debug.LogWarning("No parent objects found with the specified tag!");
            return;
        }

        // Variabile pentru numărul de apariții al valorilor din sloturile 1 și 2
        int slot1Count = 0;
        int slot2Count = 0;

        // Parcurge fiecare obiect părinte
        foreach (GameObject parentObject in parentObjects)
        {
            // Găsește toate componentele TMP_Dropdown din copii
            TMP_Dropdown[] dropdowns = parentObject.GetComponentsInChildren<TMP_Dropdown>();

            // Procesează doar dropdown-urile cu numele "Dropdown2"
            foreach (TMP_Dropdown dropdown in dropdowns)
            {
                if (dropdown.gameObject.name.Equals("Dropdown2", System.StringComparison.OrdinalIgnoreCase)) // Verifică numele
                {
                    if (dropdown.value != 0) // Verifică dacă selecția nu este cea implicită (0)
                    {
                        // Obține valoarea selectată
                        string selectedText = dropdown.options[dropdown.value].text;

                        // Numără valorile pentru slotul 1 și slotul 2
                        if (selectedText == "Slot1Value") // Schimbă "Slot1Value" cu valoarea dorită
                        {
                            slot1Count++;
                        }
                        else if (selectedText == "Slot2Value") // Schimbă "Slot2Value" cu valoarea dorită
                        {
                            slot2Count++;
                        }
                    }
                }
            }
        }

        // Afișează rezultatele pentru sloturile 1 și 2
        Debug.Log("Slot 1 Value Count: " + slot1Count);
        Debug.Log("Slot 2 Value Count: " + slot2Count);
      SceneManager.LoadScene("test ground");
      SceneManager.LoadScene("scena_1");
   }
   public void back()
   { SceneManager.LoadScene(0); }
}