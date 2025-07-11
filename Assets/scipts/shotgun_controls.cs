using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections;
public class shotgun : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;
    [SerializeField]
    private PlayerControls playerControls;
    private Vector2 Movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    public GameObject player;
    public GameObject glont;
    private Vector3 difference;
    public Transform shootSpawn;
    public GameObject arm;
    public healthbar healthbar;
    private bool m_FacingRight = true;
    private int hp = 5;
    private bool controls = false;
    private int shieldDuration = 5;
    private bool shield = false;
    private bool isShooting = false; // Track if the shoot action is ongoing
    private float shootCooldown = 1.5f; // Cooldown time in seconds
    private float lastShootTime = 0f; // Time when the last shot was fired


    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Movement.Enable();
        playerControls.Movement.Move.performed += OnMove;
        playerControls.Movement.aim_direction.performed += OnAim;
        playerControls.Movement.shoot.performed += OnShoot;
        playerControls.Movement.shoot.canceled += OnShootReleased;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    public void heal()
    {
        hp = 5;
        hit(0);
    }
    public void hit(int dmg)
    {
        if (shield == false)
        {
            Debug.Log("hit registered!\n");
            hp -= dmg;
            healthbar.sethealth(hp);
            if (hp <= 0)
                death();
        }
    }
public IEnumerator ActivateShield()
    {
        // Activează scutul
        shield = true;
        Debug.Log("Shield activat!");
       yield return new WaitForSeconds(5f);
        // După 5 secunde, dezactivează scutul
        shield = false;
        Debug.Log("Shield dezactivat!");
    }
    private void death()
    {
        playerControls.Disable();
        Destroy(gameObject);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        difference = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        myAnimator.SetFloat("moveHorizontal", Movement.x);
        myAnimator.SetFloat("moveVertical", Movement.y);
        rb.MovePosition(rb.position + Movement * (moveSpeed * Time.fixedDeltaTime));

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (difference.sqrMagnitude < 0.01f) // Stick is in deadzone
        {
            difference.x = 0;
            difference.y = 0;
            arm.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            return; // Do not process rotation if in deadzone
        }

        if (Mathf.Abs(rotationZ) > 90f && m_FacingRight)
        {
            Flip();
        }
        else if (Mathf.Abs(rotationZ) < 90f && !m_FacingRight)
        {
            Flip();
        }

        if (!m_FacingRight)
        {
            rotationZ += 180;
        }

        arm.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = player.transform.localScale;
        theScale.x *= -1;
        player.transform.localScale = theScale;
        healthbar.spin();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
       if (isShooting || Time.time - lastShootTime < shootCooldown)
            return;

        isShooting = true;
        lastShootTime = Time.time;

        for (int i = 2; i >= 0; i--)
        {
            GameObject bullet = Instantiate(glont, shootSpawn.position, transform.rotation);
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            difference = playerControls.Movement.aim_direction.ReadValue<Vector2>();

            if (difference.sqrMagnitude < 0.02f)
            {
                if (!m_FacingRight)
                    difference.x = -1;
                else
                    difference.x = 1;
            }

            if (i == 2)
            {
                bulletScript.SetDirection(difference);
            }
            else if (i == 1)
            {
                difference.x += 0.01f;
                difference.y += 0.01f;
                bulletScript.SetDirection(difference);
            }
            else if (i == 0)
            {
                difference.x -= 0.01f;
                difference.y -= 0.01f;
                bulletScript.SetDirection(difference);
            }
        }
    }

    private void OnShootReleased(InputAction.CallbackContext context)
    {
        isShooting = false;
    }
}
