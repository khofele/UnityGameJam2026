using TMPro;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private GameObject m_player;
    [Header("------ Kevin ------")]
    [SerializeField] private GameObject m_playingField;
    private bool m_slideKevin = false;
    [Range(0f, 10f)]
    [SerializeField] private float m_speedKevin = 1f;
    private Material m_material;
    private int m_kevinID;
    [Range(0f, 1f)]
    [SerializeField] private float m_kevinMin = 0.2f;
    [Range(0f, 1f)]
    [SerializeField] private float m_kevinMax = 1f;
    private float m_time = 0f;
    [Space(15)]
    [Header("------ Game Running ------")]
    [SerializeField] private GameObject m_gameRunningUI;
    [SerializeField] private TextMeshProUGUI m_gameRunningPlayerCurrentActionPointsText;
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
    //[SerializeField] private Button m_cancelMoveButton;
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
        m_kevinID = Shader.PropertyToID("_kevin");
        if(m_playingField.TryGetComponent(out MeshRenderer rend))
        {
            m_material = rend.material;//.sharedMaterial wenn 'globales' Material geändert werden soll
        }

        CloseAllMenues();
        m_endTurnButton.onClick.AddListener(LevelManager.Instance.HandlePlayerTurnEnd);
        m_endTurnButton.interactable = false;
    }
    public void CloseAllMenues()
    {
        m_pauseMenu.SetActive(false);
        m_pathMenu.SetActive(false);
        m_enemyMenu.SetActive(false);

        m_slideKevin = false;
        m_material.SetFloat(m_kevinID, 0f);
        if (m_player.TryGetComponent(out EntityPlayer ep))
            m_gameRunningPlayerCurrentActionPointsText.text = ep.CurrentActionPoints.ToString();

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

        m_slideKevin = true;

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
            m_pathLenghtText.text = "Distance: " + (_path.GetSize() -1).ToString();
            if (m_player.TryGetComponent(out EntityPlayer ep))
            {
                m_playerPathCurrentActionPointsText.text = "Anzahl AP: " + ep.CurrentActionPoints.ToString();
                if (ep.CurrentActionPoints +1 < _path.GetSize())
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
                if (ep.CurrentActionPoints <= 0)
                {
                    SetPlayerMoveButtonInteractable(false);
                    m_moveButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                }
            }
        }
        CloseAllMenues();
        OpenPathMenu();
    }
    public void SetPlayerMoveButtonInteractable(bool _interactable)
    {
        m_moveButton.interactable = _interactable;
    }
    public void EnableEndRoundButton()
    {
        m_endTurnButton.interactable = true;
    }
    public void DisableEndRoundButton()
    {
        m_endTurnButton.interactable = false;
    }
    private void Update()
    {
        if (m_slideKevin)
        {
            m_time += Time.deltaTime;
            if (m_time >= 2 * Mathf.PI) m_time -= 2 * Mathf.PI;
            float newKevin = (Mathf.Sin(m_time * m_speedKevin) + 1f) / 2f;
            float newKevinLerped = Mathf.Lerp(m_kevinMin, m_kevinMax, newKevin);

            m_material.SetFloat(m_kevinID, newKevinLerped);
        }
    }
}
