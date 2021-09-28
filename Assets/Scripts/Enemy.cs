using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  [SerializeField] private float walkSpeed = -1f;
  private Rigidbody2D enemyRigidbody;
  private SpriteRenderer enemySpriteRenderer;

  // Start is called before the first frame update
  private void Start()
  {
    enemyRigidbody = GetComponent<Rigidbody2D>();
    enemySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    Walk();
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    Player player = collision.gameObject.GetComponent<Player>();

    if (player) return;

    walkSpeed = -walkSpeed;
    transform.localScale = new Vector2(-Mathf.Sign(walkSpeed), 1f);
    Walk();
  }

  private void Walk()
  {
    Vector2 enemyVelocity = new Vector2(walkSpeed, 0f);
    enemyRigidbody.velocity = enemyVelocity;
  }

  private void TreatSprite()
  {
    enemySpriteRenderer.flipX = enemyRigidbody.velocity.x < 0;
  }
}
