using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject pausedCanvas;

    private enum GameState
    {
        Playing,
        Paused,
    }

    private const float PAUSED_SPEED = 0.0f;
    private const float NORMAL_SPEED = 1.0f;

    private GameState _state;
    private GameState State
    {
        get => _state;
        set
        {
            _state = value;
            switch (State)
            {
                default:
                case GameState.Playing:
                    Time.timeScale = NORMAL_SPEED;
                    PlayerInput.Instance.EnablePlayerInput();
                    pausedCanvas.SetActive(false);
                    break;
                case GameState.Paused:
                    Time.timeScale = PAUSED_SPEED;
                    PlayerInput.Instance.DisablePlayerInput();
                    pausedCanvas.SetActive(true);
                    break;
            }
        }
    }

    private void Start()
    {
        State = GameState.Playing;
        PlayerInput.Instance.OnPauseRequested += PlayerInput_OnPauseRequested;
    }

    private void PlayerInput_OnPauseRequested(object sender, System.EventArgs e)
    {
        if (State == GameState.Paused) State = GameState.Playing;
        else State = GameState.Paused;
    }

    public void Resume()
    {
        State = GameState.Playing;
    }

    private void OnDestroy()
    {
        Time.timeScale = NORMAL_SPEED; //exit to menu - time must return to normal
    }
}
