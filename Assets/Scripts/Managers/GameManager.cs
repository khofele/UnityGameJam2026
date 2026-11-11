using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>
{
    //[SerializeField] private LevelManager m_levelManager = null;

    private GameStates m_currentGameState = GameStates.PAUSED;

    public GameStates CurrentGameState
    {
        get { return m_currentGameState; }
    }

    public void SetState(GameStates _newState)
    {
        GameStates prevGameState = m_currentGameState;

        m_currentGameState = _newState;

        Debug.Log("Game State changed to: " + _newState);

        HandleStateChange(prevGameState, m_currentGameState);
    }

    private void HandleStateChange(GameStates _prevGameState, GameStates _newGameState)
    {
        if (_newGameState == GameStates.PAUSED)
        {
            Time.timeScale = 0.0f;
            return;
        }

        if (_prevGameState == GameStates.PAUSED)
        {
            Time.timeScale = 1.0f;
            return;
        }

        if (_newGameState == GameStates.GAMEWON)
        {
            // TODO implement
            return;
        }

        if (_newGameState == GameStates.GAMEOVER)
        {
            // TODO implement
            return;
        }
    }



    //SceneManagement & Application
    public void LoadScene(int sceneindex)
    {
        SceneManager.LoadScene(sceneindex);
        DestroyOtherManagers();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void DestroyOtherManagers()
    {
        Destroy(UIManager.Instance.gameObject);
        Destroy(GraphManager.Instance.gameObject);
        Destroy(LevelManager.Instance.gameObject);
    }
}
public enum Scenes { MainMenu, Tutorial ,Level1}