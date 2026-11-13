using UnityEngine;

public class GameOverUi : MonoBehaviour
{
    public void OnReturnToMenu()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.LoadScene((int)Scenes.MainMenu);

    }

    public void OnQuitGameButtonPressed()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.QuitGame();
    }
}
