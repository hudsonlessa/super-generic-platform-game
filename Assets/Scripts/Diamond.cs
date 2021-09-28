using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
  [SerializeField] private AudioClip pickupSound;
  private DiamondsDisplay diamondsDisplay;

  private void Start()
  {
    diamondsDisplay = FindObjectOfType<DiamondsDisplay>();
  }

  private void OnTriggerEnter2D()
  {
    AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
    diamondsDisplay.AddDiamond();
    Destroy(gameObject);
  }
}
