using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controls : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private int hp=5;
    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    public void hit(int dmg)
    {Debug.Log("hit registred!\n");
        hp-=dmg;
    if(hp<=0)
    death();
    }
    private void death()
    {   playerControls.Disable();
        Destroy(gameObject);}
    private void FixedUpdate()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        myAnimator.SetFloat("moveHorizontal", movement.x);
        myAnimator.SetFloat("moveVertical", movement.y);
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
}
