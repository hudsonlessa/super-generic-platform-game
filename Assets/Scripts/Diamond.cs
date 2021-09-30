using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
  [SerializeField] private AudioClip pickupSound;
  [SerializeField] private GameSession gameSession;

  private void OnTriggerEnter2D()
  {
    AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
    gameSession.AddDiamond();
    Destroy(gameObject);
  }
}
