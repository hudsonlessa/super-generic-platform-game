using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondsDisplay : MonoBehaviour
{
  private MainCanvas canvas;
  private Transform dots;
  private Transform diamonds;

  private void Start()
  {
    canvas = GameObject.Find("Main Canvas").GetComponent<MainCanvas>();

    dots = transform.Find("Dots");
    diamonds = transform.Find("Diamonds");

    foreach (Transform diamond in diamonds)
    {
      diamond.gameObject.SetActive(false);
    }
  }

  public void AddDiamond()
  {
    canvas.EnableThenDisable();

    int diamondsInSceneQuantity = GameObject.FindGameObjectsWithTag("Diamond").Length;

    GameObject nextDot = dots.GetChild(diamondsInSceneQuantity - 1).gameObject;
    nextDot.SetActive(false);

    GameObject nextDiamond = diamonds.GetChild(3 - diamondsInSceneQuantity).gameObject;
    nextDiamond.SetActive(true);
  }
}
