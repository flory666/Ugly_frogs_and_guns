using System.Collections;
using UnityEngine;

public class ShieldBuff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifică dacă obiectul are componenta `shotgun`
        shotgun shotgun = other.GetComponent<shotgun>();
        if (shotgun != null)
        {
            StartCoroutine(shotgun.ActivateShield()); // Apelează metoda de activare a scutului
            Destroy(gameObject); // Distruge buff-ul
            return;
        }

        // Verifică dacă obiectul are componenta `awp`
        awp awp = other.GetComponent<awp>();
        if (awp != null)
        {
            StartCoroutine(awp.ActivateShield()); // Apelează metoda de activare a scutului
            Destroy(gameObject); // Distruge buff-ul
            return;
        }

        // Verifică dacă obiectul are componenta `ak47`
        ak47 ak47 = other.GetComponent<ak47>();
        if (ak47 != null)
        {
            StartCoroutine(ak47.ActivateShield()); // Apelează metoda de activare a scutului
            Destroy(gameObject); // Distruge buff-ul
            return;
        }
    }
}
