using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
  [SerializeField] private float enableDelay = 5f;
  [SerializeField] private float disableDelay = 5f;
  private Canvas canvas;
  private bool isEnabable;

  private void Start()
  {
    canvas = GetComponent<Canvas>();
    canvas.enabled = false;
    isEnabable = false;
  }

  public void SetIsEnabable(bool value)
  {
    isEnabable = value;
  }

  public void DelayedEnable()
  {
    if (!isEnabable) return;

    StopAllCoroutines();
    StartCoroutine(HandleDelayedEnable());
  }

  private IEnumerator HandleDelayedEnable()
  {
    yield return new WaitForSecondsRealtime(enableDelay);
    canvas.enabled = true;
  }

  public void DelayedDisable()
  {
    if (!isEnabable) return;

    StopAllCoroutines();
    StartCoroutine(HandleDelayedDisable());
  }

  private IEnumerator HandleDelayedDisable()
  {
    yield return new WaitForSecondsRealtime(disableDelay);
    canvas.enabled = false;
  }

  public void EnableThenDisable()
  {
    if (!isEnabable) return;

    StopAllCoroutines();
    canvas.enabled = true;
    DelayedDisable();
  }
}
