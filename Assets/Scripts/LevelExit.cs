using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
  [SerializeField] private float sceneLoadDelay = 3f;
  [SerializeField] private float slowMotionFactor = .2f;

  private void OnTriggerEnter2D()
  {
    StartCoroutine(LoadNextScene());
  }

  private IEnumerator LoadNextScene()
  {
    Time.timeScale = slowMotionFactor;
    yield return new WaitForSecondsRealtime(sceneLoadDelay);
    Time.timeScale = 1f;
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    // Temporary
    SceneManager.LoadScene(currentSceneIndex);
  }
}
