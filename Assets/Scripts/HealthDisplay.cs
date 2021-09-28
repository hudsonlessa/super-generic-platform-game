using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
  private MainCanvas canvas;

  private void Start()
  {
    canvas = GameObject.Find("Main Canvas").GetComponent<MainCanvas>();
  }

  public void RemoveHeart()
  {
    canvas.EnableThenDisable();

    int heartCount = transform.childCount;
    GameObject lastHeart = transform.GetChild(heartCount - 1).gameObject;

    Destroy(lastHeart);
  }
}
