using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Temporary
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
  // Configuration parameters
  [SerializeField] private int playerHealth = 2;
  [SerializeField] private float walkSpeed = 5f;
  [SerializeField] private float jumpSpeed = 5f;
  [SerializeField] private float climbSpeed = 5f;
  [SerializeField] private float knockoutSpeed = 15f;
  [SerializeField] private float invunerabilityTime = 1f;
  [SerializeField] private AudioClip hitSound;

  // States
  private bool isAlive;
  private bool isInvunerable;
  private float horizontalInput = 0f;
  private float verticalInput = 0f;

  // Cached component references
  private float initialGravityScale;
  private Rigidbody2D playerRigidbody;
  private SpriteRenderer playerSpriteRenderer;
  private Animator playerAnimator;
  private HealthDisplay healthDisplay;
  [SerializeField] private Collider2D playerBodyCollider;
  [SerializeField] private Collider2D playerFeetCollider;

  private void Start()
  {
    isAlive = true;
    isInvunerable = false;
    playerRigidbody = GetComponent<Rigidbody2D>();
    playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    playerAnimator = GetComponent<Animator>();
    initialGravityScale = playerRigidbody.gravityScale;
    healthDisplay = GameObject.Find("Health Display").GetComponent<HealthDisplay>();
  }

  // Update is called once per frame
  private void Update()
  {
    if (!isAlive) return;

    SetInputValues();
    Walk();
    Jump();
    Climb();
    TreatSprite();
    ProcessDamage();
  }

  private void SetInputValues()
  {
    horizontalInput = Input.GetAxis("Horizontal");
    verticalInput = Input.GetAxis("Vertical");
  }

  private void Walk()
  {
    Vector2 playerVelocity = new Vector2(horizontalInput * walkSpeed, playerRigidbody.velocity.y);
    playerRigidbody.velocity = playerVelocity;
  }

  private void Jump()
  {
    bool isGrounded = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
      playerRigidbody.velocity += jumpVelocity;
    }
  }

  private void Climb()
  {
    bool isOnLadder = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladders"));

    if (isOnLadder)
    {
      if (verticalInput > 0)
      {
        playerRigidbody.gravityScale = 0;
        if (!playerAnimator.GetBool("isClimbing")) playerAnimator.SetBool("isClimbing", isOnLadder);
      }

      if (playerAnimator.GetBool("isClimbing"))
      {
        Vector2 playerVelocity = new Vector2(playerRigidbody.velocity.x, verticalInput * climbSpeed);
        playerRigidbody.velocity = playerVelocity;
      }
    }
    else
    {
      playerRigidbody.gravityScale = initialGravityScale;
      if (playerAnimator.GetBool("isClimbing")) playerAnimator.SetBool("isClimbing", isOnLadder);
    }
  }

  private void TreatSprite()
  {
    if (horizontalInput != 0)
    {
      playerSpriteRenderer.flipX = horizontalInput < 0;
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

      healthDisplay.RemoveHeart();

      if (playerHealth > 1)
      {
        RemoveHealth();
        StartCoroutine(MakeInvunerable());
      }
      else Die();
    }
  }

  private void RemoveHealth()
  {
    playerHealth--;
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
    Physics2D.IgnoreLayerCollision(10, 12, true);

    GameSession gameSession = FindObjectOfType<GameSession>();
    StartCoroutine(gameSession.ResetGameSession());
  }
}
