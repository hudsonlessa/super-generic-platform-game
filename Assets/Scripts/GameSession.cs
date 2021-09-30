using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
  [SerializeField] private float sceneLoadDelay = 5f;
  [SerializeField] private float slowMotionFactor = .2f;
  [SerializeField] private int health = 2;
  [SerializeField] private MainCanvas mainCanvas;
  [SerializeField] private GameObject healthDisplay;
  [SerializeField] private GameObject diamondsDisplay;
  private Transform dots;
  private Transform diamonds;

  private void Awake()
  {
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    if (currentSceneIndex != 0) Destroy(gameObject);
    else DontDestroyOnLoad(gameObject);
  }

  private void Start()
  {
    PrepareDiamondsDisplay();
  }

  public int GetHealth()
  {
    return health;
  }

  public void RemoveHealth()
  {
    mainCanvas.EnableThenDisable();

    health--;

    int heartCount = healthDisplay.transform.childCount;
    GameObject lastHeart = healthDisplay.transform.GetChild(heartCount - 1).gameObject;

    Destroy(lastHeart);
  }

  private void PrepareDiamondsDisplay()
  {
    dots = diamondsDisplay.transform.Find("Dots");
    diamonds = diamondsDisplay.transform.Find("Diamonds");

    foreach (Transform diamond in diamonds)
    {
      diamond.gameObject.SetActive(false);
    }
  }

  public void AddDiamond()
  {
    mainCanvas.EnableThenDisable();

    int diamondsInSceneQuantity = GameObject.FindGameObjectsWithTag("Diamond").Length;

    GameObject nextDot = dots.GetChild(diamondsInSceneQuantity - 1).gameObject;
    nextDot.SetActive(false);

    GameObject nextDiamond = diamonds.GetChild(3 - diamondsInSceneQuantity).gameObject;
    nextDiamond.SetActive(true);
  }

  public IEnumerator ResetGameSession()
  {
    Physics2D.IgnoreLayerCollision(10, 12, true);
    Time.timeScale = slowMotionFactor;
    yield return new WaitForSecondsRealtime(sceneLoadDelay);
    Time.timeScale = 1f;
    Physics2D.IgnoreLayerCollision(10, 12, false);

    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(currentSceneIndex);
  }
}
