using TMPro;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private GameObject m_player;
    [Header("------ Game Running ------")]
    [SerializeField] private GameObject m_gameRunningUI;
    [SerializeField] private Button m_endTurnButton;
    [Space(15)]
    [Header("------ Game Paused ------")]
    [SerializeField] private GameObject m_pauseMenu;
    [Space(15)]
    [Header("------ Path ------")]
    [SerializeField] private GameObject m_pathMenu;
    [SerializeField] private TextMeshProUGUI m_pathLenghtText;
    [SerializeField] private TextMeshProUGUI m_pathLenghtWarningText;
    [SerializeField] private Button m_moveButton;
    [SerializeField] private TextMeshProUGUI m_playerPathCurrentActionPointsText;
    [Space(15)]
    [Header("------ Enemy ------")]
    [SerializeField] private GameObject m_enemyMenu;
    //[SerializeField] private TextMeshProUGUI m_enemyNameText;
    [SerializeField] private TextMeshProUGUI m_enemyHealthText;
    [SerializeField] private TextMeshProUGUI m_enemyOORWarningText;
    [SerializeField] private Button m_attackButton;
    //[SerializeField] private TextMeshProUGUI m_playerEnemyCurrentActionPointsText;


    private void Start()
    {
        CloseAllMenues();
    }
    public void CloseAllMenues()
    {
        m_pauseMenu.SetActive(false);
        m_pathMenu.SetActive(false);
        m_enemyMenu.SetActive(false);

        m_gameRunningUI.SetActive(true);
    }
    private void OpenEnemyMenu()
    {
        m_pauseMenu.SetActive(false);
        m_pathMenu.SetActive(false);
        m_gameRunningUI.SetActive(false);

        m_enemyMenu.SetActive(true);
    }
    private void OpenPathMenu()
    {
        m_pauseMenu.SetActive(false);
        m_enemyMenu.SetActive(false);
        m_gameRunningUI.SetActive(false);

        m_pathMenu.SetActive(true);
    }
    public void ToggleEndTurnButton(bool _enabled)
    {
        m_endTurnButton.interactable = _enabled;
    }
    public void OnPauseButtonPressed()
    {
        GameManager.Instance.SetState(GameStates.PAUSED);
        CloseAllMenues();
        m_gameRunningUI.SetActive(false);
        m_pauseMenu.SetActive(true);
    }
    public void OnResumeButtonPressed()
    {
        GameManager.Instance.SetState(GameStates.RUNNING);//prüfen ob game over/won
        CloseAllMenues();
    }
    public void OnMainMenuButtonPressed()//int vom button bekommen?
    {
        GameManager.Instance.LoadScene((int)Scenes.MainMenu);
    }
    public void OnQuitGameButtonPressed()
    {
        GameManager.Instance.QuitGame();
    }
    //public void OnResetCamButtonPressed()
    //{
    //    camMovement.CenterCamera();
    //}
    public void OnEndTurnButtonPressed()
    {
        ToggleEndTurnButton(false);
        CloseAllMenues();
        LevelManager.Instance.NextEntityTurn();//mit Karo abgleichen
    }
    public void HandleEnemyClicked(GameObject _enemy)
    {
        //TODO Daten aus _enemy auslesen und in text schreiben
        CloseAllMenues();
        OpenEnemyMenu();
    }
    public void HandleGroundClicked(Path _path)
    {
        if (_path != null)
        {
            m_pathLenghtText.text = "Distance: " + _path.GetSize().ToString();
            if (m_player.TryGetComponent(out EntityPlayer ep))
            {
                m_playerPathCurrentActionPointsText.text = "Anzahl AP: " + ep.GetCurrentActionPoints().ToString();
                if (ep.GetCurrentActionPoints() +1 < _path.GetSize())
                {
                    m_moveButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellowGreen;
                    m_pathLenghtWarningText.color = Color.red;
                    m_pathLenghtText.color = Color.red;
                    m_pathLenghtWarningText.text = "You don't have enough action points to walk this far!";
                }
                else
                {
                    m_moveButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                    m_pathLenghtText.color = Color.green;
                    m_pathLenghtWarningText.text = "";
                }
                m_moveButton.onClick.RemoveAllListeners();
                m_moveButton.onClick.AddListener(() =>
                {
                    LevelManager.Instance.ConfirmEntityMove(ep);
                });
                m_moveButton.interactable = !ep.IsMoving && (ep.GetCurrentActionPoints() > 0);
            }
        }
        CloseAllMenues();
        OpenPathMenu();
    }
}
