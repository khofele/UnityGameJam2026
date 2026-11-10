using TMPro;
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
    [Space(15)]
    [Header("------ Enemy ------")]
    [SerializeField] private GameObject m_enemyMenu;
    [SerializeField] private TextMeshProUGUI m_enemyNameText;
    [SerializeField] private TextMeshProUGUI m_enemyHealthText;
    [SerializeField] private TextMeshProUGUI m_enemyOORWarningText;
    [SerializeField] private Button m_attackButton;

    private CamMovement camMovement;

    private void Start()
    {
        camMovement = new CamMovement();
        CloseAllMenues();
    }
    private void CloseAllMenues()
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
    //public void OnResetCamButtonPressed()
    //{
    //    camMovement.CenterCamera();
    //}
    public void OnEndTurnButtonPressed()
    {
        ToggleEndTurnButton(false);
        CloseAllMenues();
        //LevelManager.Instance.NextEntityTurn();
    }
    public void HandleEnemyClicked(GameObject _enemy)
    {
        CloseAllMenues();
        OpenEnemyMenu();
    }
    public void HandleGroundClicked(Path _path)
    {
        if (_path != null)
        {
            m_pathLenghtText.text = _path.GetSize().ToString();
            if (m_player.TryGetComponent(out EntityPlayer ep))
            {
                if (ep.GetCurrentActionPoints() > _path.GetSize())
                {
                    m_pathLenghtWarningText.text = "You don't have enough action points to walk this far!";
                }
                else
                {
                    m_pathLenghtWarningText.text = "";
                }
            }
            //m_unitTypeText.text = unit.unitType.ToString();
            //m_unitCountText.text = $"Members: {unit.CurrentMemberCount} / {unit.MaxMemberCount}";

            //m_addMemberButton.interactable = !unit.m_isFull;
            //m_addMemberButton.onClick.RemoveAllListeners();
            //m_addMemberButton.onClick.AddListener(() =>
            //{
            //    unit.AddMember();
            //    OpenUnitsMenu();
            //});

            //m_moveUnitButton.interactable = !unit.IsEmpty() && !unit.m_unitIsMoving;
        }
        CloseAllMenues();
        OpenPathMenu();
    }
}
