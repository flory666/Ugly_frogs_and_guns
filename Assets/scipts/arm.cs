using UnityEngine;
using UnityEngine.InputSystem;
public class arm : MonoBehaviour
{
	public GameObject player;
	public GameObject glont;
	public PlayerControls playerControls;
	private Vector3 difference;
	public Transform shootSpawn;
	private bool m_FacingRight = true;
	private void Awake()
	{
		playerControls = new PlayerControls();
	}
	private void Start()
	{
		// Enable the input actions
		playerControls.Enable();
		difference.x = 1;
		playerControls.Movement.shoot.performed += OnShoot;
	}
private void OnDisable()
{playerControls.Disable();
    playerControls.Movement.shoot.performed -= OnShoot;
}
	private void FixedUpdate()
	{
		//Arm Rotation
		difference = playerControls.Movement.aim_direction.ReadValue<Vector2>();
		float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		// also check if you are facing the other way
		// so you do not call Flip() when you dont need to
		if (difference.sqrMagnitude < 0.01f) // Stick is in deadzone
		{	difference.x = 0;
		difference.y =0;
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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

		// Since your player's local scale is fliped
		// you'll need to adjust the rotation too
		if (!m_FacingRight)
		{
			rotationZ += 180;
		}

		// lastly apply the rotation
		transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
	}
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = player.transform.localScale;
		theScale.x *= -1;
		player.transform.localScale = theScale;
	}
	private void OnShoot(InputAction.CallbackContext context)
	{
		// Instantiate the bullet at the calculated position
		GameObject bullet = Instantiate(glont, shootSpawn.position, transform.rotation);
		BulletScript bulletScript = bullet.GetComponent<BulletScript>();
		// Pass the aim direction to the bullet script
		if (difference.x == 0 && difference.y == 0)
			if (!m_FacingRight)
				difference.x = -1;
			else
				difference.x = 1;

		bulletScript.SetDirection(difference);
	}
}