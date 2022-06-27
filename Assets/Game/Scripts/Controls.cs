using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    public static Controls Instance { get; private set; }
    
    public GameObject uiStart;
    public GameObject uiGameOver;
    public GameObject uiVictory;
    public GameObject uiRestart;
    
    private Player player;
    private GameState gameState;
    private Camera cam;
    private bool isRegisteringTouches = true;

    protected void Awake()
    {
        Instance = this;
        
        player = Player.Instance;
        gameState = GameState.Starting;
        cam = Camera.main;
    }

    protected void Start()
    {
        uiStart.SetActive(true);
        uiGameOver.SetActive(false);
        uiVictory.SetActive(false);
        uiRestart.SetActive(false);
    }

    protected void Update()
    {
        processTouches();        
    }

    private void processTouches()
    {
        if (!isRegisteringTouches)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            switch (gameState)
            {
                case GameState.Starting:
                    StartGame();
                    break;
                case GameState.Playing:
                    processAttack(Input.mousePosition);
                    break;
                default:
                    RestartGame();
                    break;
            }
        }
    }

    private void processAttack(Vector3 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        Vector3 target;
        if (Physics.Raycast(ray, out RaycastHit hit, 30f))
            target = hit.point;
        else
            target = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, cam.farClipPlane));
        player.Throw(target);
    }
    
    public void StartGame()
    {
        gameState = GameState.Playing;
        pauseRegisteringTouches(1f);
        uiStart.SetActive(false);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        gameState = GameState.Loss;
        pauseRegisteringTouches(1f);
        uiGameOver.SetActive(true);
        uiRestart.SetActive(true);
    }
    
    public void Victory()
    {
        gameState = GameState.Victory;
        pauseRegisteringTouches(1f);
        uiVictory.SetActive(true);
        uiRestart.SetActive(true);
    }

    private void pauseRegisteringTouches(float timeSeconds)
    {
        Invoke(nameof(startRegisteringTouches), timeSeconds);
    }
    
    private void startRegisteringTouches()
    {
        isRegisteringTouches = true;
    }
}

public enum GameState
{
    Starting,
    Playing,
    Loss,
    Victory
}
