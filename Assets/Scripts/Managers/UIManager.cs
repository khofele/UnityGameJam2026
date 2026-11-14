using TMPro;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private EntityPlayer m_player;
    [SerializeField] private Pfadvisualisierung m_pf;
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
    [SerializeField] private TextMeshProUGUI m_roundCounterDisplayText;//TODO anzeigen lassen
    [SerializeField] private TextMeshProUGUI m_playerHPDisplayText;//Slider
    [SerializeField] private TextMeshProUGUI m_gameRunningPlayerCurrentActionPointsText;
    [SerializeField] private Image m_gameRunningPlayerCurrentActionPointsBGImage;
    [SerializeField] private Button m_endTurnButton;
    [SerializeField] private Button m_leaveLevelButton;
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
    [SerializeField] private TextMeshProUGUI m_enemyNameText;
    [SerializeField] private RawImage m_enemyTextureRawImage;
    [SerializeField] private RenderTexture[] m_enemyRenderTextures;
    [SerializeField] private EntityAnimController[] m_enemyRenderTextureEntityAnimControllers;//bissl lang
    [SerializeField] private TextMeshProUGUI m_enemyHealthText;//Slider
    [SerializeField] private TextMeshProUGUI m_enemyOORWarningText;
    [SerializeField] private Button m_attackButton;
    //[SerializeField] private TextMeshProUGUI m_playerEnemyCurrentActionPointsText;

    private void Start()
    {
        m_kevinID = Shader.PropertyToID("_kevin");
        if(m_playingField.TryGetComponent(out MeshRenderer rend))
        {
            m_material = rend.sharedMaterial;//.sharedMaterial wenn 'globales' Material geändert werden soll
        }
        m_material.SetFloat(m_kevinID, 0f);

        ResetEACs();
        CloseAllMenues();
        m_endTurnButton.onClick.AddListener(LevelManager.Instance.HandlePlayerTurnEnd);
        m_endTurnButton.interactable = false;
        UpdateRoundCounter(1,LevelManager.Instance.MaxRoundCounter);
    }
    public void CloseAllMenues()
    {
        m_pauseMenu.SetActive(false);
        m_pathMenu.SetActive(false);
        m_enemyMenu.SetActive(false);

        m_pf.ShowPath(false);
        m_slideKevin = false;
        m_material.SetFloat(m_kevinID, 0f);
        if (m_player.TryGetComponent(out EntityPlayer ep))
            UpdateCurrentAPText(ep.CurrentActionPoints);
        //ResetEACs();//Upsi führt zu crash
        
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
        m_pf.ShowPath(true);

        m_pathMenu.SetActive(true);
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
        DisableEndRoundButton();
        CloseAllMenues();
        //Debug.Log("End Turn Button Pressed");
        //LevelManager.Instance.NextEntityTurn();//mit Karo abgleichen
    }
    public void HandleEnemyClicked(EntityEnemy _enemy)
    {
        if (_enemy.IsAlive == false) return;
        
        m_enemyNameText.text = _enemy.gameObject.name;
        m_enemyTextureRawImage.texture = m_enemyRenderTextures[_enemy.RenderTextureIndex];
        m_enemyHealthText.text = _enemy.CurrentHealth.ToString();//als slider
        if (m_player.CurrentActionPoints <= 0)
        {
            m_enemyOORWarningText.text = "You're out of Action Points!";
            m_enemyOORWarningText.color = Color.red;
            m_attackButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            m_attackButton.onClick.RemoveAllListeners();
            m_attackButton.interactable = false;
        }
        else
        {
            if ((m_player.CurrentNodePos - _enemy.CurrentNodePos).magnitude > LevelManager.Instance.MaxPlayerAttackReach)
            {
                m_enemyOORWarningText.text = "You're out of Reach!";
                m_enemyOORWarningText.color = Color.red;
                m_attackButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                m_attackButton.onClick.RemoveAllListeners();
                m_attackButton.interactable = false;
            }
            else if ((m_player.CurrentNodePos - _enemy.CurrentNodePos).magnitude > LevelManager.Instance.AttackReach)
            {
                m_enemyOORWarningText.text = "You're almost out of reach!\nYour damage will be halved.";
                m_enemyOORWarningText.color = Color.orangeRed;
                m_attackButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellowGreen;
                m_attackButton.onClick.RemoveAllListeners();
                m_attackButton.onClick.AddListener(() =>
                {
                    LevelManager.Instance.ConfirmEntityAttack(m_player, _enemy, LevelManager.Instance.RollDamage(true) / 2f);
                    //UpdateAttackUI(_enemy);//in enemy entity
                    m_attackButton.interactable = false;
                    m_player.PlayerMovement.CanClick = false;
                    m_player.ReduceActionPoints(1);
                });
                m_attackButton.interactable = true;
            }
            else
            {
                m_enemyOORWarningText.text = "";
                m_attackButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                m_attackButton.onClick.RemoveAllListeners();
                m_attackButton.onClick.AddListener(() =>
                {
                    LevelManager.Instance.ConfirmEntityAttack(m_player, _enemy, LevelManager.Instance.RollDamage(true));
                    m_attackButton.interactable = false;
                    m_player.PlayerMovement.CanClick = false;
                    m_player.ReduceActionPoints(1);
                });
                m_attackButton.interactable = true;
            }
        }
        CloseAllMenues();
        OpenEnemyMenu();
    }

    public void UpdateAttackUI(EntityEnemy _enemy, bool _dies)
    {
        HandleEnemyClicked(_enemy);
        if (_dies) m_enemyRenderTextureEntityAnimControllers[_enemy.RenderTextureIndex].SetDeathTrigger();
        m_enemyRenderTextureEntityAnimControllers[_enemy.RenderTextureIndex].SetHitTrigger();
        m_player.PlayerMovement.CanClick = true;
    }

    public void ResetEACs()
    {
        foreach (EntityAnimController EAC in m_enemyRenderTextureEntityAnimControllers)
        {
            EAC.ResetAnimator();
        }
        CloseAllMenues();
    }

    public void HandleGroundClicked(Path _path)
    {
        if (_path != null)
        {
            m_pathLenghtText.text = "Distance: " + (_path.GetSize() -1).ToString();
            if (m_player.TryGetComponent(out EntityPlayer ep))
            {
                m_playerPathCurrentActionPointsText.text = "Remaining Action Points: " + ep.CurrentActionPoints.ToString();
                m_pf.DrawPath(_path, ep.CurrentActionPoints +1);
                if (ep.CurrentActionPoints +1 < _path.GetSize())
                {
                    m_moveButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellowGreen;
                    m_pathLenghtWarningText.color = Color.red;
                    m_pathLenghtText.color = Color.red;
                    m_pathLenghtWarningText.text = "You don't have enough action points to walk this far!";
                    //m_pf.PathVis(_path, false);//veraltet
                }
                else
                {
                    m_moveButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                    m_pathLenghtText.color = Color.green;
                    m_pathLenghtWarningText.text = "";
                    //m_pf.PathVis(_path, true);//veraltet
                }
                m_moveButton.onClick.RemoveAllListeners();
                m_moveButton.onClick.AddListener(() =>
                {
                    LevelManager.Instance.ConfirmEntityMove(ep);
                    CloseAllMenues();
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
    public void UpdateCurrentAPText(int _ap)
    {
        m_gameRunningPlayerCurrentActionPointsText.text = _ap.ToString();
        m_playerPathCurrentActionPointsText.text = _ap.ToString();
    }
    public void UpdateRoundCounter(int _currentRound, int _maxRound)
    {
        m_roundCounterDisplayText.text = _currentRound + "/" + _maxRound;
    }
    public void UpdatePlayerHPDisplay(float _hp)
    {
        m_playerHPDisplayText.text = _hp.ToString();
    }
    public void ShowLeaveLevelButton()
    {
        m_leaveLevelButton.gameObject.SetActive(true);
        StartCoroutine(BlinkError(m_leaveLevelButton.gameObject, 15));
    }
    public void OnLeaveLevelButtonPressed()
    {
        GameManager.Instance.LoadScene((int)Scenes.Tutorial);
    }
    private IEnumerator BlinkError(GameObject _go, int _blinks = 2)
    {
        if (_go.TryGetComponent(out Image img))
        {
            Color originalColor = img.color;

            for (int i = 0; i < _blinks; i++)
            {
                img.color = new Color(1f, 0.5f, 0.5f);
                yield return new WaitForSeconds(0.1f);

                img.color = originalColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
