using UnityEngine;
public class BulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force = 10f;
    private Vector2 direction;  // To store the direction of the bullet

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // This method is called from the PlayerController to pass the direction
    public void SetDirection(Vector2 aimDirection)
    {
        direction = aimDirection.normalized; // Normalize the direction to avoid inconsistent speeds
    }

    private void Start()
    {
        // Apply the direction to the bullet’s velocity after it’s been set
        if (direction.sqrMagnitude > 0.01f) // Only move if direction is valid
        {
            rb.velocity = direction * force;
        }
    }
    private void OnTriggerEnter2D(Collider2D hit)
    {
        shotgun targetShotgun = hit.GetComponent<shotgun>();
        if (targetShotgun != null)
        {
            targetShotgun.hit(1);
            Destroy(gameObject);
        }
        awp targetAWP = hit.GetComponent<awp>();
        if (targetAWP != null)
        {
            targetAWP.hit(1);
            Destroy(gameObject);
        }
        target enemy = hit.GetComponent<target>();
        if (enemy != null)
        {
            enemy.hit(1);
            Destroy(gameObject);
        }
        ak47 targerak = hit.GetComponent<ak47>();
        if (targerak != null)
        {
            targerak.hit(1);
            Destroy(gameObject);
        }
        DestructibleObject destructible = hit.GetComponent<DestructibleObject>();
        if (destructible != null)
        {
            destructible.TakeDamage(15); // Aplicăm 1 punct de daune
            Destroy(gameObject);
        }

        Destroy(gameObject);

    }
}
