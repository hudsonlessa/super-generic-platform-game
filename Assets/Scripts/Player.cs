using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
  // Configuration parameters
  [SerializeField] private float walkSpeed = 5f;
  [SerializeField] private float acceleration = 10f;
  [Tooltip("The higher the value, the faster the stop.")]
  [SerializeField] private float deceleration = 10f;
  [SerializeField] private float jumpSpeed = 5f;
  [SerializeField] private float fallGravityScale = .5f;
  [SerializeField] private float climbSpeed = 5f;
  [SerializeField] private float knockoutSpeed = 15f;
  [Space]
  [SerializeField] private float invunerabilityTime = 1f;
  [Space]
  [SerializeField] private AudioClip hitSound;
  [SerializeField] private GameSession gameSession;

  // States
  private bool isAlive;
  private bool isInvunerable;

  // Cached component references
  private Rigidbody2D playerRigidbody;
  private SpriteRenderer playerSpriteRenderer;
  private Animator playerAnimator;
  [Space]
  [SerializeField] private Collider2D playerBodyCollider;
  [SerializeField] private Collider2D playerFeetCollider;

  private float initialGravityScale;
  private bool hasMoveInput;
  private Vector2 moveInput;
  private PlayerControls playerControls;

  private void Awake()
  {
    playerControls = new PlayerControls();

    playerControls.Gameplay.Move.performed += ctx => hasMoveInput = true;
    playerControls.Gameplay.Move.canceled += ctx => hasMoveInput = false;

    playerControls.Gameplay.Jump.performed += ctx => Jump();
  }

  private void Start()
  {
    isAlive = true;
    isInvunerable = false;
    playerRigidbody = GetComponent<Rigidbody2D>();
    playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    playerAnimator = GetComponent<Animator>();
    initialGravityScale = playerRigidbody.gravityScale;
  }

  // Update is called once per frame
  private void Update()
  {
    if (!isAlive) return;

    TreatWalkSprite();
    ProcessDamage();
  }

  private void FixedUpdate()
  {
    if (!isAlive) return;

    Walk();
    Climb();
    FallSlowly();
  }

  private void Walk()
  {
    Vector2 newMoveInput = playerControls.Gameplay.Move.ReadValue<Vector2>();

    if (hasMoveInput) Accelerate(newMoveInput);
    else Decelerate(newMoveInput);

    Vector2 playerVelocity = new Vector2(moveInput.x * walkSpeed, playerRigidbody.velocity.y);
    playerRigidbody.velocity = playerVelocity;
  }

  private void Accelerate(Vector2 newMoveInput)
  {
    float xLerp = Mathf.Lerp(moveInput.x, newMoveInput.x, .5f * acceleration * Time.deltaTime);

    if (newMoveInput.x > 0)
      if (xLerp > .8f) xLerp = 1;

    if (newMoveInput.x < 0)
      if (xLerp < -.8f) xLerp = -1;

    moveInput = new Vector2(xLerp, newMoveInput.y);
  }

  private void Decelerate(Vector2 newMoveInput)
  {
    float xLerp = Mathf.Lerp(moveInput.x, newMoveInput.x, .5f * deceleration * Time.deltaTime);

    if (moveInput.x > 0)
      if (xLerp < .2f) xLerp = 0;

    if (moveInput.x < 0)
      if (xLerp > -.2f) xLerp = 0;

    moveInput = new Vector2(xLerp, newMoveInput.y);
  }

  private void Jump()
  {
    bool isGrounded = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

    if (isGrounded)
    {
      Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
      playerRigidbody.velocity += jumpVelocity;
    }
  }

  private void FallSlowly()
  {
    bool isFalling = playerRigidbody.velocity.y < 0;

    if (!playerAnimator.GetBool("isClimbing"))
    {
      if (isFalling) playerRigidbody.gravityScale = fallGravityScale;
      else playerRigidbody.gravityScale = initialGravityScale;
    }
  }

  private void Climb()
  {
    bool isOnLadder = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladders"));

    if (isOnLadder)
    {
      if (moveInput.y > 0)
      {
        playerRigidbody.gravityScale = 0;
        if (!playerAnimator.GetBool("isClimbing")) playerAnimator.SetBool("isClimbing", isOnLadder);
      }

      if (playerAnimator.GetBool("isClimbing"))
      {
        Vector2 playerVelocity = new Vector2(playerRigidbody.velocity.x, moveInput.y * climbSpeed);
        playerRigidbody.velocity = playerVelocity;
      }
    }
    else
    {
      playerRigidbody.gravityScale = initialGravityScale;
      if (playerAnimator.GetBool("isClimbing")) playerAnimator.SetBool("isClimbing", isOnLadder);
    }
  }

  private void TreatWalkSprite()
  {
    if (moveInput.x != 0)
    {
      playerSpriteRenderer.flipX = moveInput.x < 0;
      playerAnimator.SetBool("isWalking", true);
    }
    else playerAnimator.SetBool("isWalking", false);
  }

  private void ProcessDamage()
  {
    if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) && isAlive && !isInvunerable)
    {
      Vector2 playerVelocity = new Vector2(0f, knockoutSpeed);
      playerRigidbody.velocity = playerVelocity;

      AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);

      if (gameSession.GetHealth() > 1) StartCoroutine(MakeInvunerable());
      else Die();

      gameSession.RemoveHealth();
    }
  }

  private IEnumerator MakeInvunerable()
  {
    isInvunerable = true;
    yield return new WaitForSeconds(invunerabilityTime);
    isInvunerable = false;
  }

  private void Die()
  {
    isAlive = false;
    playerAnimator.SetBool("isDying", true);
    StartCoroutine(gameSession.ResetGameSession());
  }

  private void OnEnable()
  {
    playerControls.Gameplay.Enable();
  }

  private void OnDisable()
  {
    playerControls.Gameplay.Disable();
  }
}
