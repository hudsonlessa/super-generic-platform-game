using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleConstrainer : MonoBehaviour
{
  [SerializeField] private Canvas titleCanvas;
  [SerializeField] private MainCanvas mainCanvas;

  private void OnTriggerExit2D()
  {
    mainCanvas.SetIsEnabable(true);
    Destroy(titleCanvas.gameObject);
    Destroy(gameObject);
  }
}
