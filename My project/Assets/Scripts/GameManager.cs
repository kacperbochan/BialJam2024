using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        Playing,
        Paused,
    }

    private GameState _state;
    private GameState state
    {
        get => _state;
        set
        {
            _state = value;
        }
    }

    private void Start()
    {
        state = GameState.Playing;
    }
}
