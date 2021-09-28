using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
  [SerializeField] private float sceneLoadDelay = 5f;
  [SerializeField] private float slowMotionFactor = .2f;

  private void Awake()
  {
    int gameSessionsQuantity = FindObjectsOfType<GameSession>().Length;

    if (gameSessionsQuantity > 1) Destroy(gameObject);
    else DontDestroyOnLoad(gameObject);
  }

  public IEnumerator ResetGameSession()
  {
    Time.timeScale = slowMotionFactor;
    yield return new WaitForSecondsRealtime(sceneLoadDelay);
    Time.timeScale = 1f;

    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(currentSceneIndex);

    Destroy(gameObject);
  }
}
