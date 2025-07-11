using System.Collections;
using UnityEngine;

public class heal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifică dacă obiectul are componenta `shotgun`
        shotgun shotgun = other.GetComponent<shotgun>();
        if (shotgun != null)
        {
           shotgun.heal(); // Apelează metoda de activare a scutului
            Destroy(gameObject); // Distruge buff-ul
            return;
        }

        // Verifică dacă obiectul are componenta `awp`
        awp awp = other.GetComponent<awp>();
        if (awp != null)
        {
            awp.heal(); // Apelează metoda de activare a scutului
            Destroy(gameObject); // Distruge buff-ul
            return;
        }

        // Verifică dacă obiectul are componenta `ak47`
        ak47 ak47 = other.GetComponent<ak47>();
        if (ak47 != null)
        {
            ak47.heal(); // Apelează metoda de activare a scutului
            Destroy(gameObject); // Distruge buff-ul
            return;
        }
    }
}
