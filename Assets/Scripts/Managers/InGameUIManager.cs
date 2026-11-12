using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : Manager<InGameUIManager>
{
    // TODO move to UI Manager
    [SerializeField] private Button m_btnEndRound = null;

    private void Start()
    {
        m_btnEndRound.onClick.AddListener(LevelManager.Instance.HandlePlayerTurnEnd); // TODO EndTurnButton in UIManager
        m_btnEndRound.interactable = false;
    }

    // TODO RAUS
    private void HandlePlayerTurnChanged(bool _isPlayerTurn)
    {
        // TODO make button only available if player has no actionpoints left
        m_btnEndRound.interactable = _isPlayerTurn;
    }

    public void EnableEndRoundButton()
    {
        m_btnEndRound.interactable = true;
    }

    public void DisableEndRoundButton()
    {
        m_btnEndRound.interactable = false;
    }
}
